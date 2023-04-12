using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistressTailTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerCityToken")) {
            
        }
    }
}
