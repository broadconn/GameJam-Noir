using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenuController : MonoBehaviour {
    [SerializeField] private GameObject uiGameObject;
    private bool _uiIsShowing;
 
    void Start() {
        ShowUI(false);
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleUI();
        }
    }

    public void ShowUI(bool b) {
        _uiIsShowing = b;
        uiGameObject.SetActive(b);
    }

    public void ToggleUI() {
        _uiIsShowing = !_uiIsShowing;
        ShowUI(_uiIsShowing);
    }

    public void ResetGame() {
        GameController.Instance.StartNewGame();
        ShowUI(false);
    }
}
