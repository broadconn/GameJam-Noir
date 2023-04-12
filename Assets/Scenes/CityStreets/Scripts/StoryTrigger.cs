using System;
using System.Collections;
using UnityEngine;

public class StoryTrigger : MonoBehaviour {
    [SerializeField] private StoryId storyId;

    public StoryId GetID() {
        return storyId;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PlayerCityToken"))
            GameController.Instance.StartConversation(storyId);
    }
}
