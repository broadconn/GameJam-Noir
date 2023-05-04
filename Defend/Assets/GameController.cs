using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private Transform GridHighlighter;

    private Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
 
    void Update() {
        var layerMask = 1 << LayerMask.NameToLayer("MouseRaycastLayer");
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit, 10000, layerMask)) {
            var hitPos = hit.point;
            var hitPosMod = new Vector3(Mathf.Floor(hitPos.x) + 0.5f, hitPos.y, Mathf.Floor(hitPos.z) + 0.5f);
            GridHighlighter.position = hitPosMod;
        }
    }
    
    // void FixedUpdate()
    // {
    //     var ray = _camera.ScreenPointToRay(Input.mousePosition);
    //
    //     if (Physics.Raycast(ray.origin, ray.direction, out var hit, 100.0f)) {
    //         print("Found an object - distance: " + hit.distance);
    //         GridHighlighter.position = hit.point;
    //     }
    // }
}
