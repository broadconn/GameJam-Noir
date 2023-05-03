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
    [Header("General Settings")]
    [SerializeField] private float smoothSpeed = 10;
    [SerializeField] private float rotateSpeed = 40;
    [Header("State Settings")]
    [SerializeField] private float playingDistance = 7;
    [SerializeField] private float playingPitch = 0;
    [Space(5)]
    [SerializeField] private float mapDistance = 500;
    [SerializeField] private float mapPitch = 30;

    private CinemachineCollider _cameraCollider;

    private CameraSettings _streetSettings;
    private CameraSettings _mapSettings = new()
    {
        Distance = 500,
        Pitch = 30,
        DefaultYRotation = 0,
        ManualRotationEnabled = false
    };

    private bool _manualRotationEnabled;
    private bool _rotateInputReceived;
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
        
        // setting the settings
        _streetSettings = new CameraSettings {
            Distance = playingDistance,
            Pitch = playingPitch,
            DefaultYRotation = 0,
            ManualRotationEnabled = true
        };
        _mapSettings = new CameraSettings {
            Distance = mapDistance,
            Pitch = mapPitch,
            DefaultYRotation = 0,
            ManualRotationEnabled = false
        };
        
        _smoothedYRot = _tgtYRot;

        _cameraCollider = playerCamera.GetComponent<CinemachineCollider>();
        
        SetMode(CityMode.Street); 
    }

    #region Input Event Handling
    private void RotateInputStarted(InputAction.CallbackContext input)
    {
        _rotateInputReceived = true;
        _rotateDir = input.ReadValue<float>();
    }
    private void RotateInputCancelled(InputAction.CallbackContext input) { _rotateInputReceived = false; }
    #endregion

    void Update() {
        UpdateValuesViaInput();
        UpdateSmoothedValues();
        ApplyValues();
    }

    private void UpdateValuesViaInput()
    {
        // yaw manual rotation
        if (_manualRotationEnabled && _rotateInputReceived)
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
        playerCamera.transform.localEulerAngles = new Vector3(_smoothedXRot, _smoothedYRot, 0); // beware, this doesn't work if LookAt is set
        
        // apply distance
        var componentBase = playerCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer transposer) {
            transposer.m_CameraDistance = _smoothedDist; 
        }
    }

    public void SetCameraLookTarget(Transform t, bool allowCollisions) {
        playerCamera.LookAt = t;
        _cameraCollider.enabled = allowCollisions; 
    }

    public void SetMode(CityMode mode)
    {
        var settings = mode == CityMode.Street ? _streetSettings : _mapSettings;
        _cameraCollider.enabled = mode == CityMode.Street; 
        SetTargetValues(settings);
    }

    private void SetTargetValues(CameraSettings cs)
    {
        _tgtYRot = cs.DefaultYRotation; // consider replacing this with a setting on the map node or current car park
        _manualRotationEnabled = cs.ManualRotationEnabled;
        
        _tgtXRot = cs.Pitch;
        _tgtDistance = cs.Distance;
    }

    public void ForceCameraRotation(float spawnRotation) {
        _tgtYRot = spawnRotation;
        playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, spawnRotation, 0);
    }
}

internal struct CameraSettings
{
    public float Distance { get; set; }
    public float Pitch { get; set; } // X Rotation
    public bool ManualRotationEnabled { get; set; }
    public float DefaultYRotation { get; set; }  
}
