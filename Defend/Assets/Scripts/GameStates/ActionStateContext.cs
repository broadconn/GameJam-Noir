using UnityEngine;

namespace GameStates {
    public class ActionStateContext {
        public Camera MainCamera;
        public GridHighlighter GridHighlighter;

        public bool StateChanged = false;
        private ActionState _nextState;
        public ActionState NextState {
            get => _nextState;
            set {
                _nextState = value;
                StateChanged = true;
            }
        }
    }
}