using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTokenThoughtsTrigger : MonoBehaviour {
    [TextArea]
    [SerializeField] private string textToDisplay; 
    [SerializeField] private string tagToTriggerOn;

    private PlayerTokenThoughts _playerTokenThoughts;

    private void Awake() {
        var player = GameObject.FindGameObjectWithTag("PlayerCityToken");
        _playerTokenThoughts = player.GetComponentInChildren<PlayerTokenThoughts>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag(!string.IsNullOrWhiteSpace(tagToTriggerOn) ? tagToTriggerOn : "PlayerCityToken")) 
            return;
        
        _playerTokenThoughts.ShowText(textToDisplay); 
        Destroy(this);
    }
}
