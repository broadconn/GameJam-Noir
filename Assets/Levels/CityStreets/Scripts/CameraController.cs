using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private float rotateSpeed = 40;
    private float tgtYRot = 0;
    private float realYRot;

    public float CameraFacingAngle => realYRot;

    void Start()
    {
        realYRot = tgtYRot;
    }

    void Update()
    {
        realYRot = Mathf.LerpAngle(realYRot, tgtYRot, Time.deltaTime * rotateSpeed);
        playerCamera.transform.localEulerAngles = new Vector3(0, realYRot, 0);
    }

    /// <summary>
    /// Sign is positive or negative. Camera will be rotated 90 degrees according to the sign.
    /// </summary>
    /// <param name="sign"></param>
    public void RotateCamera(float sign)
    {
        var dir = Mathf.Sign(sign); // can't trust the caller :)
        tgtYRot += 90 * dir;
    }
}
