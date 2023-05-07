using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GameStates {
    public abstract class GameplayProcessor {
        protected readonly GameplayStateContext Ctx;

        protected GameplayProcessor(GameplayStateContext ctx) {
            Ctx = ctx;
        }

        public virtual void OnEnterState() {
            Ctx.TimeEnteredState = Time.time;
            Ctx.StateChanged = false; 
        }

        public abstract void Update();
        public abstract void OnExitState();

        public bool StateChanged => Ctx.StateChanged;
        public GameplayState NextState => Ctx.NextState;
    }

    public enum GameplayState {
        Normal,
        PlacingBuilding 
    }
}