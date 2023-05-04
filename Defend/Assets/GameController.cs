using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private Transform GridHighlighter;

    private Camera _camera;

    private int _numCellsWideToHighlight = 1; // e.g. if placing a 1x1 or 2x2-width tower

    private void Awake() {
        _camera = Camera.main;
    } 
 
    void Update() {
        var layerMask = 1 << LayerMask.NameToLayer("MouseRaycastLayer");
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit, 100, layerMask)) {
            var hitPos = hit.point;
            var hitPosMod = new Vector3(Mathf.Floor(hitPos.x), hitPos.y, Mathf.Floor(hitPos.z));
            hitPosMod += GetHighlightOffset();
            GridHighlighter.position = hitPosMod;
        }
    }

    private Vector3 GetHighlightOffset() {
        var offset = _numCellsWideToHighlight / 2f;
        return new Vector3(offset, 0, offset);
    }
}
