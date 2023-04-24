using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGui : MonoBehaviour {
    [SerializeField] private GameObject mapPrompt;

    private void Awake() {
        mapPrompt.SetActive(false);
    }

    public void ShowMapPrompt(bool b) {
        mapPrompt.SetActive(b);
    }
}
