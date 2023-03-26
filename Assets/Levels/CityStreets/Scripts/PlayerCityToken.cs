using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCityToken : MonoBehaviour
{
    private static readonly int PlayerPosShaderRef = Shader.PropertyToID("_PlayerPos");
    
    [SerializeField] private InputActionAsset actions;
    private InputAction _moveAction;

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
        var moveVector = _moveAction.ReadValue<Vector2>();
        print(moveVector);
    }

    void LateUpdate()
    {
        Shader.SetGlobalVector(PlayerPosShaderRef, transform.position);
    }
}
