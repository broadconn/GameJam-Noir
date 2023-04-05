using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCityToken : MonoBehaviour
{
    private static readonly int PlayerPosShaderRef = Shader.PropertyToID("_PlayerPos");

    [SerializeField] private CameraController cameraController;
    
    [SerializeField] 
    private InputActionAsset actions;
    private InputAction _moveInput;

    [SerializeField] private float groundMoveSpeed = 10;
    [SerializeField] private float mapMoveSpeed = 1000;
    [SerializeField] private float rotateSpeed = 10;

    private CityMode mode = CityMode.Street;

    private void Awake()
    {
        _moveInput = actions.FindActionMap("Player_Map").FindAction("Movement");
        _moveInput.Enable();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // movement 
        var inputVector = _moveInput.ReadValue<Vector2>();
        if (inputVector.magnitude > 1) 
            inputVector = inputVector.normalized;
        var inputVectorXZ = new Vector3(inputVector.x, 0, inputVector.y);
        var cameraMoveDir = Quaternion.AngleAxis(cameraController.CameraFacingAngle, Vector3.up) * inputVectorXZ; // rotates the input XZ direction around the Up vector by CameraFacingAngle degrees. So 'forward' input is always in the direction the camera faces.
        var movementThisFrame = cameraMoveDir * (Time.deltaTime * (mode == CityMode.Map ? mapMoveSpeed : groundMoveSpeed));
        transform.position += movementThisFrame;

        // model rotation - smoothly face movement direction
        if (movementThisFrame.magnitude > 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementThisFrame),
                Time.deltaTime * rotateSpeed);
    }
    
    void LateUpdate()
    {
        SetGlobalShaderPosition();
    }

    public void SetGlobalShaderPosition()
    {
        Shader.SetGlobalVector(PlayerPosShaderRef, transform.position);
    }

    public void SetMode(CityMode m)
    {
        mode = m;
    }
}