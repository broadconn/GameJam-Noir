using UnityEngine;

namespace GameStates {
    public interface IGameplayProcessorReferencingGameObject {
        void SetReferenceObject(GameObject gameObject); //e.g. the placing building state needs a building to move around and place
    }
}