using UnityEngine.PlayerLoop;

namespace GameStates {
    public abstract class ActionStateProcessor {
        protected readonly ActionStateContext Ctx;

        protected ActionStateProcessor(ActionStateContext ctx) {
            Ctx = ctx;
        }
        
        public abstract void OnEnterState();
        public abstract void HandleKeyboardInput();
        public abstract void Update();

        public bool StateChanged => Ctx.StateChanged;
        public ActionState NextState => Ctx.NextState;
    }

    public enum ActionState {
        Normal,
        PlacingBuilding 
    }
}