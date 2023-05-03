using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For code-driven edits to post processing, fog etc.
/// </summary>
public class CityVisualsController : MonoBehaviour {
    [Header("Fog values are divided by 1000 when applied.")]
    [SerializeField] private float fogDensityMap = 0.01f;
    [SerializeField] private float fogDensityPlayer = 0.1f;
    [Space(5)]
    [SerializeField] private float fogShiftSpeed = 5f;
    private float _curFogLevel, _tgtFogLevel; 

    void Start()
    {
        _curFogLevel = RenderSettings.fogDensity;
        _tgtFogLevel = RenderSettings.fogDensity;
    }
 
    void Update() {
        if (Mathf.Abs(_curFogLevel - _tgtFogLevel) > 0.001f) {
            _curFogLevel = Mathf.MoveTowards(_curFogLevel, _tgtFogLevel, Time.deltaTime * fogShiftSpeed);
            RenderSettings.fogDensity = _curFogLevel / 1000f;
        }
    }

    public void SetMode(CityMode mode) { 
        _tgtFogLevel = mode == CityMode.Map ? fogDensityMap : fogDensityPlayer;
    }
}
