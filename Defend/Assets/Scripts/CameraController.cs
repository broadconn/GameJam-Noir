using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour {
    [SerializeField] private float dragMultiplier = 2; 
    [SerializeField] private float dragHeightMultiplier = 0.05f;
    [SerializeField] private float dragDeadZone;
    [SerializeField] private float scrollSpeed = 100;
    [SerializeField] private Vector2 cameraYMinMax = new Vector2(5, 30);
    private bool IsDragging => Input.GetMouseButton(0) && !_isDragInDeadZone;
    public bool StoppedDraggingThisFrame { get; private set; }
    private bool _wasDraggingLastFrame;

    private Vector3 _dragScreenOrigin;
    private bool _isDragInDeadZone; // e.g. so minuscule drags dont prevent tower placement
    private const int CameraDragMouseBtn = (int) MouseButton.RightMouse;

    private Vector3 _lastInputMousePos;

    private Transform _myTransform;

    private void Awake() {
        _myTransform = transform;
    }

    // Manually called by GameController's Update()
    public void DoUpdate() {
        UpdateDrag();
        UpdateZoom();
    }

    private void UpdateDrag() {
        StoppedDraggingThisFrame = false;

        // potential start of drag
        if (Input.GetMouseButtonDown(CameraDragMouseBtn)) {
            _dragScreenOrigin = Input.mousePosition;
            _isDragInDeadZone = true;
        }

        if (Input.GetMouseButton(CameraDragMouseBtn)) {
            var draggedFromDragOrigin = Input.mousePosition - _dragScreenOrigin;
            if (draggedFromDragOrigin.magnitude > dragDeadZone)
                _isDragInDeadZone = false;

            var heightMultiplier = transform.position.y * dragHeightMultiplier;
            var multiplier = dragMultiplier * heightMultiplier;
            var dragThisFrame = (Input.mousePosition - _lastInputMousePos) * (multiplier * Time.deltaTime);
            var draggedWorldVector = new Vector3(dragThisFrame.x, 0, dragThisFrame.y);
            
            if (!_isDragInDeadZone)
                transform.position -= draggedWorldVector;
        }

        if (Input.GetMouseButtonUp(CameraDragMouseBtn) && _wasDraggingLastFrame)
            StoppedDraggingThisFrame = true;
    }

    private void UpdateZoom() {
        var mouseScroll = Input.mouseScrollDelta.y;

        if (mouseScroll == 0 ||
            mouseScroll < 0 && transform.position.y >= cameraYMinMax.y ||
            mouseScroll > 0 && transform.position.y <= cameraYMinMax.x)
            return;

        var transformPosition = _myTransform.position;
        var heightMultiplier = transformPosition.y * dragHeightMultiplier;
        var zoomAmount = _myTransform.forward * (mouseScroll * scrollSpeed * heightMultiplier * Time.deltaTime);
        transformPosition += zoomAmount;
        
        var yClamp = Mathf.Clamp(transformPosition.y, cameraYMinMax.x, cameraYMinMax.y);
        transformPosition = new Vector3(transformPosition.x, yClamp, transformPosition.z);
        _myTransform.position = transformPosition;
    }

    private void LateUpdate() {
        _wasDraggingLastFrame = IsDragging;
        _lastInputMousePos = Input.mousePosition;
    }
}
