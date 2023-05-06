using UnityEngine;

namespace GameStates {
    public class GameplayStateContext {
        public Camera MainCamera;
        public GridHighlighter GridHighlighter;
        public GameObject ReferenceGameObject;

        public bool StateChanged;
        private GameplayState _nextState;
        public GameplayState NextState {
            get => _nextState;
            set {
                _nextState = value;
                StateChanged = true;
            }
        }
    }
}