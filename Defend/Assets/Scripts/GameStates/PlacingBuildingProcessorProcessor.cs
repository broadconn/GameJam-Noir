using UnityEngine;

namespace GameStates {
    public class PlacingBuildingProcessorProcessor : GameplayProcessor, IGameplayProcessorReferencingGameObject {
        public PlacingBuildingProcessorProcessor(GameplayStateContext ctx) : base(ctx) { }
        
        public override void OnEnterState() {
            base.OnEnterState();
            Ctx.GridHighlighter.gameObject.SetActive(true);
        }

        public override void HandleKeyboardInput() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Ctx.NextState = GameplayState.Normal;
            }
        }

        public override void Update() {
            var mouseWorldPos = GetMouseWorldPosition();
            Ctx.GridHighlighter.SetMouseWorldPos(mouseWorldPos);
        }

        private Vector3 GetMouseWorldPosition() {
            const int maxCastDistance = 100;
            var layerMask = 1 << LayerMask.NameToLayer("MouseRaycastLayer");
            var mouseToWorldRay = Ctx.MainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(mouseToWorldRay, out var hit, maxCastDistance, layerMask) ? hit.point : Vector3.positiveInfinity;
        }

        public void SetReferenceObject(GameObject gameObject) {
            Ctx.ReferenceGameObject = gameObject;
        }

        public override void OnExitState() {
            Ctx.GridHighlighter.gameObject.SetActive(false); 
        }
    }
}