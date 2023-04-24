using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Pulse : MonoBehaviour {
    [SerializeField] private PulseMode mode;
    [SerializeField][Min(0)] private float pulseMagnitude = 1; 
    [SerializeField][Min(0)] private float pulseTime = 0.5f;
    private float _baseSize;
    private bool _enabled = true;

    private void Awake() {
        _baseSize = transform.localScale.x; // assumes uniform scale
    }

    private void Update() {
        if (!_enabled) return;
 
        float tgtScale;
        if (mode == PulseMode.Sharp) {
            var animPerc = (Time.time % pulseTime) / pulseTime;
            var movement = animPerc * pulseMagnitude;
            var halfMovement = movement / 2f;
            tgtScale = Mathf.Lerp(_baseSize + halfMovement, _baseSize - halfMovement, animPerc);
        }
        else { // smooth mode
            var movement = Mathf.Sin(Time.time * pulseTime) * pulseMagnitude;
            tgtScale = _baseSize + movement;
        } 
        SetScale(tgtScale);
    } 

    public void SetPulsing(bool b) {
        _enabled = b;
        
        // if(!_enabled)
        //     SetScale(0.5f);
    }

    private void SetScale(float f) {
        transform.localScale = new Vector3(f, f, f);
    }

    private enum PulseMode {
        Sharp,
        Soft
    }
} 