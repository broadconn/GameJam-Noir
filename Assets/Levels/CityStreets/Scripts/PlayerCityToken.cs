using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCityToken : MonoBehaviour
{
    private static readonly int PlayerPosShaderRef = Shader.PropertyToID("_PlayerPos");
    
    [SerializeField] private InputActionAsset actions;
    private InputAction _moveAction;

    [SerializeField] private float moveSpeed = 10;

    private void Awake()
    {
        _moveAction = actions.FindActionMap("Player_Map").FindAction("Movement");
        _moveAction.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var inputVector = _moveAction.ReadValue<Vector2>();
        var moveVector = new Vector3(inputVector.x, 0, inputVector.y) * (Time.deltaTime * moveSpeed);
        transform.position += moveVector;
    }

    void LateUpdate()
    {
        Shader.SetGlobalVector(PlayerPosShaderRef, transform.position);
    }
}
