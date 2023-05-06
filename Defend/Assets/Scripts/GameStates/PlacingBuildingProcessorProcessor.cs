using UnityEngine;
using UnityEngine.UIElements;

namespace GameStates {
    public class PlacingBuildingProcessorProcessor : GameplayProcessor, IGameplayProcessorReferencingGameObject {
        private const float BuildingSize = 2; // Num squares the current building takes up. Will probably always be 2.
        
        public PlacingBuildingProcessorProcessor(GameplayStateContext ctx) : base(ctx) {
            Ctx.GridHighlighter.gameObject.SetActive(false);
        }
        
        public override void OnEnterState() {
            base.OnEnterState();
            
            var mouseWorldPos = GetMouseWorldPosition();
            Ctx.GridHighlighter.gameObject.SetActive(true);
            Ctx.GridHighlighter.SetMouseWorldPos(mouseWorldPos, true);
            
            // TODO: set reference building visuals to ethereal
        }

        public override void HandleKeyboardInput() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Ctx.NextState = GameplayState.Normal;
            }

            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse)) {
                // TODO: set building visual to solid
                // TODO: enable building attack logic
                var newBuilding = Object.Instantiate(Ctx.ReferenceGameObject);
                newBuilding.transform.position = Ctx.ReferenceGameObject.transform.position;
                
                if(!Input.GetKey(KeyCode.LeftShift)) // keep placing towers if shift is held
                    Ctx.NextState = GameplayState.Normal;
            }
        }

        public override void Update() {
            var mouseWorldPos = GetMouseWorldPosition();
            Ctx.GridHighlighter.SetMouseWorldPos(mouseWorldPos);

            var flooredWorldPos =
                new Vector3(Mathf.Floor(mouseWorldPos.x), mouseWorldPos.y, Mathf.Floor(mouseWorldPos.z));
            var buildingPos = flooredWorldPos + new Vector3(BuildingSize / 2f, 0, BuildingSize / 2f);
            Ctx.ReferenceGameObject.transform.position = buildingPos;
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
            Ctx.ReferenceGameObject.gameObject.SetActive(true);
        }

        public override void OnExitState() {
            Ctx.GridHighlighter.gameObject.SetActive(false); 
        }
    }
}