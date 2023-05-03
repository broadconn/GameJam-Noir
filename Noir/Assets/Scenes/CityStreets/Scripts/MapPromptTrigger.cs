using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPromptTrigger : MonoBehaviour {
    private CityGui _cityGui;

    private void Awake() {
        var player = GameObject.FindGameObjectWithTag("CityUI");
        _cityGui = player.GetComponentInChildren<CityGui>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("PlayerCityToken")) 
            return;
        
        _cityGui.ShowMapPrompt(true);
        Destroy(this);
    }
}
