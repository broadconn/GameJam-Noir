using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class PathfindingTest : MonoBehaviour {
    [SerializeField] private Transform PathStart, PathEnd, DebugDotsParent;
    private PathController _pathController;

    private void Awake() {
        _pathController = new PathController(PathStart.position.WorldXZToV2Int(), PathEnd.position.WorldXZToV2Int(), DebugDotsParent);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _pathController.RefreshPath();
        }
    }
}
