using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThoughtsTrigger : MonoBehaviour {
    [TextArea]
    [SerializeField] private string textToDisplay;

    private GameObject _player;

    private void Start() {
        _player = GameObject.FindGameObjectWithTag("PlayerCityToken");
    }

    private void OnTriggerEnter(Collider other) {  
        _player.GetComponentInChildren<PlayerCityThoughts>().ShowText(textToDisplay); 
        Destroy(this);
    }
}
