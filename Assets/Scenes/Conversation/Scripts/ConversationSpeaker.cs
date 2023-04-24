using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationSpeaker : MonoBehaviour {
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public string id;
    public float lightIntensitySpeaking = 200;
    public float lightIntensitySilent = 100;
    public float lightIntensityChangeSpeed = 10;

    private Light _speakerIlluminatingLight;
    private float _lightTgtIntensity = 0;
    private float _curLightIntensity = 0;
 
    private float _timeTriggeredTransition = float.MinValue;
    private const float TransitionLength = 0.7f;

    private Transform _model;
    
    public bool IsVisible { get; private set; }

    private void Awake() {
        _speakerIlluminatingLight = GetComponentInChildren<Light>();
        _model = transform.Find("Model");
    }

    private void Start() {
        _speakerIlluminatingLight.intensity = 0;
        IsVisible = false;
    }

    private void Update() {
        _curLightIntensity = Mathf.Lerp(_curLightIntensity, _lightTgtIntensity, Time.deltaTime * lightIntensityChangeSpeed);
        _speakerIlluminatingLight.intensity = _curLightIntensity;

        var timePassed = Time.time - _timeTriggeredTransition;
        var percThroughAnim = Mathf.Clamp01(timePassed / TransitionLength);
        if (!IsVisible) percThroughAnim = 1 - percThroughAnim; // reverse anim if hiding
        var curScale = curve.Evaluate(percThroughAnim);
        _model.localScale = new Vector3(curScale, curScale, curScale);
    }

    public void Show() {
        IsVisible = true;
        _lightTgtIntensity = lightIntensitySilent; // turn light on, but the dull setting 
        _timeTriggeredTransition = Time.time;
    }

    public void Hide(bool instant = false) {
        IsVisible = false;
        _lightTgtIntensity = 0; 
        _timeTriggeredTransition = instant ? float.MinValue : Time.time;
    }
 
    public void SetSpeaking(bool isSpeaking) {
        if (!IsVisible) return; // a bit of a side-effect. Need to call Show() before this.
        _lightTgtIntensity = isSpeaking ? lightIntensitySpeaking : lightIntensitySilent;
    }
}
