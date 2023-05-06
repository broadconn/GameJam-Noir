using UnityEngine.PlayerLoop;

namespace GameStates {
    public abstract class GameplayStateProcessor {
        protected readonly GameplayStateContext Ctx;

        protected GameplayStateProcessor(GameplayStateContext ctx) {
            Ctx = ctx;
        }

        public virtual void OnEnterState() {
            Ctx.StateChanged = false;   
        }
        public abstract void HandleKeyboardInput();
        public abstract void Update();

        public bool StateChanged => Ctx.StateChanged;
        public GameplayState NextState => Ctx.NextState;
    }

    public enum GameplayState {
        Normal,
        PlacingBuilding 
    }
}