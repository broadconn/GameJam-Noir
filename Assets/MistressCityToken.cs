using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class MistressCityToken : MonoBehaviour {
    [SerializeField] private SplineContainer splinePath;
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float rotateSpeed = 20f;

    private Transform _myTransform;
    private Vector3 _posLastFrame;

    // spline following stuff
    private float _splineTime = 0;
    private bool _isPaused = true;
    private float _resumeTime = float.MaxValue; // time we get auto-unpaused

    private GameObject light;
    
    // Start is called before the first frame update
    void Start() {
        _myTransform = transform; // Rider says repeatedly accessing transform is inefficient
        _posLastFrame = _myTransform.position;

        light = GetComponentInChildren<Light>().GameObject();
        light.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        MoveAlongSpline();
        RotateModelWithMovement();
    }

    public void StartMoving() {
        _isPaused = false;
        light.SetActive(true);
    }

    private void MoveAlongSpline() {
        if (_isPaused && Time.time > _resumeTime)
            _isPaused = false;
        
        if (!_isPaused) {
            _splineTime += Time.deltaTime * moveSpeed;
            _splineTime = Mathf.Clamp01(_splineTime);
        }

        var splinePosXZ = splinePath.Spline.EvaluatePosition(_splineTime);
        _myTransform.position = new Vector3(splinePosXZ.x, _myTransform.position.y, splinePosXZ.z);
    }

    private void RotateModelWithMovement() {
        // smoothly face movement direction
        var movementThisFrame = _myTransform.position - _posLastFrame;
        if (movementThisFrame.magnitude > 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementThisFrame),
                Time.deltaTime * rotateSpeed);
    }

    private void LateUpdate() {
        _posLastFrame = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        var pathPauser = other.GetComponent<MistressCityPathPauser>();
        if (pathPauser != null) {
            var pauseTime = pathPauser.PauseTime;
            _isPaused = true;
            _resumeTime = Time.time + pauseTime;
        }
    }
}
