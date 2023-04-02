using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private InputAction cameraRotateAction;
    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 10;
    [SerializeField] private float rotateSpeed = 40;

    private CameraSettings settings;
    private readonly CameraSettings streetSettings = new()
    {
        Distance = 7,
        Pitch = 9,
        DefaultYRotation = 0,
        ManualRotationEnabled = true
    };
    private readonly CameraSettings mapSettings = new()
    {
        Distance = 500,
        Pitch = 89,
        DefaultYRotation = 0,
        ManualRotationEnabled = false
    };

    private bool _isRotating = false;
    private float _rotateDir = 0;
    private float _tgtYRot = 0, _smoothedYRot =0; 
    private float _tgtXRot = 0, _smoothedXRot =0; 
    private float _tgtDistance, _smoothedDist;

    public float CameraFacingAngle => _smoothedYRot;

    private void Awake()
    {
        // subscribing to input
        cameraRotateAction = actions.FindActionMap("Player_Map").FindAction("CameraRotate");
        cameraRotateAction.Enable();
        cameraRotateAction.started += RotateInputStarted; 
        cameraRotateAction.canceled += RotateInputCancelled;

        SetMode(CityMode.Street);
    }   
    
    #region Input Handling
    private void RotateInputStarted(InputAction.CallbackContext input)
    {
        _isRotating = true;
        _rotateDir = input.ReadValue<float>();
    }
    private void RotateInputCancelled(InputAction.CallbackContext input) { _isRotating = false; }
    #endregion

    void Start()
    {
        _smoothedYRot = _tgtYRot;
    }

    void Update()
    {
        ApplyInput();
        UpdateSmoothedValues();
        ApplySmoothedValues();
    }

    private void ApplyInput()
    {
        // yaw manual rotation
        if (_isRotating && settings.ManualRotationEnabled)
            _tgtYRot += _rotateDir * rotateSpeed * Time.deltaTime;
    }

    private void UpdateSmoothedValues()
    {
        // smooth yaw
        _smoothedYRot = Mathf.LerpAngle(_smoothedYRot, _tgtYRot, Time.deltaTime * smoothSpeed);

        // smooth pitch
        _smoothedXRot = Mathf.LerpAngle(_smoothedXRot, _tgtXRot, Time.deltaTime * smoothSpeed);

        // smooth distance
        _smoothedDist = Mathf.Lerp(_smoothedDist, _tgtDistance, Time.deltaTime * smoothSpeed);
    }

    private void ApplySmoothedValues()
    {
        // apply rotation
        playerCamera.transform.localEulerAngles = new Vector3(_smoothedXRot, _smoothedYRot, 0);
        
        // apply distance
        var componentBase = playerCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer transposer)
        {
            transposer.m_CameraDistance = _smoothedDist; 
        }
    }

    private void SetSettingsTargets()
    {
        _tgtYRot = settings.DefaultYRotation;
        _tgtXRot = settings.Pitch;
        _tgtDistance = settings.Distance;
    }

    public void SetMode(CityMode mode)
    {
        settings = mode == CityMode.Street ? streetSettings : mapSettings;
        SetSettingsTargets();
    }
}

internal struct CameraSettings
{
    public float Distance { get; set; }
    public float Pitch { get; set; } // X Rotation
    public bool ManualRotationEnabled { get; set; }
    public float DefaultYRotation { get; set; }
}
