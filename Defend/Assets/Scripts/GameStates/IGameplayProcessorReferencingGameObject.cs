using UnityEngine;

namespace GameStates {
    public interface IGameplayProcessorReferencingGameObject {
        void SetReferenceObject(GameObject gameObject); //e.g. the placing build state needs a building to move around and place
    }
}