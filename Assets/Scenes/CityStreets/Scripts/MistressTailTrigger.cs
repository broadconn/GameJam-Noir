using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistressTailTrigger : MonoBehaviour {
    [SerializeField] private MistressCityToken mistress;
    [SerializeField] private CameraController playerCamera;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerCityToken")) {
            mistress.StartMoving();
            playerCamera.SetCameraLookTarget(mistress.transform, false);
        }
    }
}
