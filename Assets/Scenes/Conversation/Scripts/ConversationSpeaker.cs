using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationSpeaker : MonoBehaviour {
    public string id;
    public float lightIntensitySpeaking = 200;
    public float lightIntensitySilent = 100;
    public float lightIntensityChangeSpeed = 10;

    private Light _speakerIlluminatingLight;
    private float _lightTgtIntensity = 0;
    private float _curLightIntensity = 0;
    
    public bool IsVisible => _isVisible;
    private bool _isVisible;

    private void Awake() {
        _speakerIlluminatingLight = GetComponentInChildren<Light>();
    }

    private void Start() {
        _speakerIlluminatingLight.intensity = 0;
        _isVisible = false;
    }

    private void Update() {
        _curLightIntensity = Mathf.Lerp(_curLightIntensity, _lightTgtIntensity, Time.deltaTime * lightIntensityChangeSpeed);
        _speakerIlluminatingLight.intensity = _curLightIntensity; 
    }

    public void Show() {
        _isVisible = true;
        _lightTgtIntensity = lightIntensitySilent; // turn light on, but the dull setting
    }

    public void Hide() {
        _isVisible = false;
        _lightTgtIntensity = 0;
    }
 
    public void SetSpeaking(bool isSpeaking) {
        if (!_isVisible) return; // a bit of a side-effect. Need to call Show() before this.
        _lightTgtIntensity = isSpeaking ? lightIntensitySpeaking : lightIntensitySilent;
    }
}
