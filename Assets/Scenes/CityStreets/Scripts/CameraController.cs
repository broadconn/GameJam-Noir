using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private InputAction cameraRotateAction;
    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 10;
    [SerializeField] private float rotateSpeed = 40;

    private readonly CameraSettings _streetSettings = new()
    {
        Distance = 7,
        Pitch = 0,
        DefaultYRotation = 0,
        ManualRotationEnabled = true
    };
    private readonly CameraSettings _mapSettings = new()
    {
        Distance = 500,
        Pitch = 30,
        DefaultYRotation = 0,
        ManualRotationEnabled = false
    };

    private bool _manualRotationEnabled;
    private bool _isRotating;
    private float _rotateDir;
    private float _tgtYRot, _smoothedYRot; 
    private float _tgtXRot, _smoothedXRot; 
    private float _tgtDistance, _smoothedDist;

    public float CameraFacingAngle => playerCameraTransform.localEulerAngles.y;

    private void Awake()
    {
        // subscribing to input
        cameraRotateAction = actions.FindActionMap("Player_Map").FindAction("CameraRotate");
        cameraRotateAction.Enable();
        cameraRotateAction.started += RotateInputStarted; 
        cameraRotateAction.canceled += RotateInputCancelled;

        SetMode(CityMode.Street);
    }   
    
    #region Input Event Handling
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
        UpdateValuesViaInput();
        UpdateSmoothedValues();
        ApplyValues();
    }

    private void UpdateValuesViaInput()
    {
        // yaw manual rotation
        if (_manualRotationEnabled && _isRotating)
            _tgtYRot += _rotateDir * rotateSpeed * Time.deltaTime;
    }

    private void UpdateSmoothedValues()
    {
        // smooth yaw
        _smoothedYRot = Mathf.LerpAngle(_smoothedYRot, _tgtYRot, Time.deltaTime * smoothSpeed);

        // smooth pitch (e.g. when switching to map view)
        _smoothedXRot = Mathf.LerpAngle(_smoothedXRot, _tgtXRot, Time.deltaTime * smoothSpeed);

        // smooth distance
        _smoothedDist = Mathf.Lerp(_smoothedDist, _tgtDistance, Time.deltaTime * smoothSpeed);
    }

    private void ApplyValues()
    {
        // apply rotation
        playerCamera.transform.localEulerAngles = new Vector3(_smoothedXRot, _smoothedYRot, 0); // this doesn't work if LookAt is set
        
        // apply distance
        var componentBase = playerCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer transposer) {
            transposer.m_CameraDistance = _smoothedDist; 
        }
    }

    public void SetCameraLookTarget(Transform t) {
        playerCamera.LookAt = t;
    }

    public void SetMode(CityMode mode)
    {
        var settings = mode == CityMode.Street ? _streetSettings : _mapSettings;
        SetTargetValues(settings);
    }

    private void SetTargetValues(CameraSettings cs)
    {
        _tgtYRot = cs.DefaultYRotation;
        _tgtXRot = cs.Pitch;
        _tgtDistance = cs.Distance;
        _manualRotationEnabled = cs.ManualRotationEnabled;
    }

    public void ForceCameraRotation(float spawnRotation) {
        print("Setting spawn rotation: " + spawnRotation);
        _tgtYRot = spawnRotation;
        playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, spawnRotation, playerCamera.transform.localEulerAngles.z);
    }
}

internal struct CameraSettings
{
    public float Distance { get; set; }
    public float Pitch { get; set; } // X Rotation
    public bool ManualRotationEnabled { get; set; }
    public float DefaultYRotation { get; set; } // can probably remove this
}
