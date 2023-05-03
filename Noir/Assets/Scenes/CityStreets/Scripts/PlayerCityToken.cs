using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerCityToken : MonoBehaviour
{
    private static readonly int PlayerPosShaderRef = Shader.PropertyToID("_PlayerPos");

    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform model;
    [SerializeField] private float yPos = 0.7f; // fixed y position
    private Transform _myTransform;
    
    [SerializeField] 
    private InputActionAsset actions;
    private InputAction _moveInput;

    [SerializeField] private float groundMoveSpeed = 10;
    [SerializeField] private float mapMoveSpeed = 1000;
    [SerializeField] private float rotateSpeed = 10; 

    private CityMode _mode = CityMode.Street; // In map mode we still control the player token. Kinda hacky but it works :)

    private CharacterController _cc;

    private void Awake()
    {
        _moveInput = actions.FindActionMap("Player_Map").FindAction("Movement");
        _moveInput.Enable();

        _myTransform = transform;
        _cc = GetComponent<CharacterController>();
    } 
 
    void Update() {
        if (!_cc.enabled) return;
        
        // movement 
        var inputVector = _moveInput.ReadValue<Vector2>();
        if (inputVector.magnitude > 1) 
            inputVector = inputVector.normalized;
        var inputVectorXZ = new Vector3(inputVector.x, 0, inputVector.y);
        var cameraMoveDir = Quaternion.AngleAxis(cameraController.CameraFacingAngle, Vector3.up) * inputVectorXZ; // rotates the input XZ direction around the Up vector by CameraFacingAngle degrees. So 'forward / w' input always moves in the direction the camera faces.
        var movementThisFrame = cameraMoveDir * (Time.deltaTime * (_mode == CityMode.Map ? mapMoveSpeed : groundMoveSpeed));
        _cc.Move(movementThisFrame); 
        
        TeleportToPosition(transform.position); // just forcing the y position. It could've changed via the character controller code.

        // model rotation - smoothly face movement direction
        if (movementThisFrame.magnitude > 0)
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, Quaternion.LookRotation(movementThisFrame),
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
        _mode = m;
        _cc.enabled = _mode == CityMode.Street;
        _cc.detectCollisions = _mode == CityMode.Street;
    }

    public void TeleportToPosition(Vector3 position) {
        _cc.enabled = false;
        _myTransform.position = new Vector3(position.x, yPos, position.z);
        _cc.enabled = true;
    }
}