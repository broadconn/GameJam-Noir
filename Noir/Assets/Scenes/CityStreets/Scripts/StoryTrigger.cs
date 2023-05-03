using System;
using System.Collections;
using UnityEngine;

public class StoryTrigger : MonoBehaviour {
    [SerializeField] private StoryId storyId;
    [SerializeField] private float spawnRotation; 
    
    /// <summary>
    /// The rotation the player will take upon entering the City scene at this location
    /// </summary>
    public float SpawnRotation => spawnRotation;

    public StoryId GetID() {
        return storyId;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PlayerCityToken"))
            StoryController.StartConversation(storyId);
    }
}
