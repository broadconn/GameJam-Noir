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

    private bool _isRotating = false;
    private float _rotateDir = 0;
    private float _tgtYRot = 0;
    private float _realYRot;

    public float CameraFacingAngle => _realYRot;

    private void RotateStarted(InputAction.CallbackContext ctx)
    {
        _isRotating = true;
        _rotateDir = ctx.ReadValue<float>();
    }
    private void RotateCancelled(InputAction.CallbackContext ctx) { _isRotating = false; }

    private void Awake()
    { 
        cameraRotateAction = actions.FindActionMap("Player_Map").FindAction("CameraRotate");
        cameraRotateAction.Enable();
        cameraRotateAction.started += RotateStarted; 
        cameraRotateAction.canceled += RotateCancelled; 
    }

    void Start()
    {
        _realYRot = _tgtYRot;
    }

    void Update()
    {
        if (_isRotating) 
            _tgtYRot += _rotateDir * rotateSpeed * Time.deltaTime; 
        
        _realYRot = Mathf.LerpAngle(_realYRot, _tgtYRot, Time.deltaTime * smoothSpeed);
        playerCamera.transform.localEulerAngles = new Vector3(0, _realYRot, 0);
    } 
}
