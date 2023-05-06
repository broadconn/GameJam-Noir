using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameStates {
    public class PlacingBuildingProcessorProcessor : GameplayProcessor, IGameplayProcessorReferencingGameObject {
        private const float BuildingSize = 2; // Num squares the current building takes up. Will probably always be 2.
        private Vector3 _mouseWorldPos; // should be set at the start of Update();
        
        public PlacingBuildingProcessorProcessor(GameplayStateContext ctx) : base(ctx) {
            Ctx.GridHighlighter.gameObject.SetActive(false);
        }
        
        public override void OnEnterState() {
            base.OnEnterState();
            
            Ctx.GridHighlighter.gameObject.SetActive(true);
            Ctx.GridHighlighter.SetPos(GetMouseWorldPosition(), instant: true);
            
            // TODO: set reference building visuals to ethereal
        }

        public override void Update() {
            _mouseWorldPos = GetMouseWorldPosition(); 
            var cellsRequested = GetSelectedCellsFromMousePos(_mouseWorldPos);
            var canPlaceHere = Ctx.PathController.CheckCellsAreFree(cellsRequested); // TODO: only check this when the hovered cell changes

            Ctx.GridHighlighter.SetPos(_mouseWorldPos);
            Ctx.GridHighlighter.SetColoringValid(canPlaceHere);
            
            PlaceBuildingVisualizerAtMousePos();

            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse)) {
                if (canPlaceHere) {
                    // place the building
                    Ctx.PathController.SetCellsOccupied(cellsRequested);
                    var newBuilding = Object.Instantiate(Ctx.ReferenceGameObject);
                    newBuilding.transform.position = Ctx.ReferenceGameObject.transform.position;

                    // if not holding shift, switch back to normal mode
                    if (!Input.GetKey(KeyCode.LeftShift))
                        ExitBuildMode();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ExitBuildMode();
            }
        }

        /// <summary>
        /// The visual representation of where a building would be created if placed where the mouse is.
        /// </summary>
        private void PlaceBuildingVisualizerAtMousePos() {
            var flooredWorldPos = new Vector3(Mathf.Floor(_mouseWorldPos.x), _mouseWorldPos.y, Mathf.Floor(_mouseWorldPos.z));
            var buildingPos = flooredWorldPos + new Vector3(BuildingSize / 2f, 0, BuildingSize / 2f);
            Ctx.ReferenceGameObject.transform.position = buildingPos;
        }

        /// <summary>
        /// Assumes the mouse's world pos corresponds to the building's bottom left corner, and adds cells based on the building size.
        /// </summary>
        /// <param name="mouseWorldPos"></param>
        /// <returns></returns>
        private static List<Vector2> GetSelectedCellsFromMousePos(Vector3 mouseWorldPos) {
            var mousePosV2 = new Vector2(Mathf.Floor(mouseWorldPos.x), Mathf.Floor(mouseWorldPos.z));

            var occupiedCells = new List<Vector2>();
            for (var i = 0; i < BuildingSize; i++) 
                for (var j = 0; j < BuildingSize; j++) 
                    occupiedCells.Add(new Vector2(mousePosV2.x + i, mousePosV2.y + j));
            return occupiedCells;
        }

        private Vector3 GetMouseWorldPosition() {
            const int maxCastDistance = 100;
            var layerMask = 1 << LayerMask.NameToLayer("MouseRaycastLayer");
            var mouseToWorldRay = Ctx.MainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(mouseToWorldRay, out var hit, maxCastDistance, layerMask) 
                ? hit.point : Vector3.positiveInfinity;
        }

        public void SetReferenceObject(GameObject gameObject) {
            Ctx.ReferenceGameObject = gameObject;
            Ctx.ReferenceGameObject.SetActive(true);
        }

        public override void OnExitState() {
            Ctx.GridHighlighter.gameObject.SetActive(false); 
            Ctx.ReferenceGameObject.SetActive(false);
        }

        private void ExitBuildMode() {
            Ctx.NextState = GameplayState.Normal;
        }
    }
}