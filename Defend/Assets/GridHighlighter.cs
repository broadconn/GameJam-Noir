using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlighter : MonoBehaviour {
    [SerializeField] private int _numCellsWideToHighlight = 2; // e.g. if placing a 1x1 or 2x2-width tower
    private Renderer _highlighterRenderer;
    private Camera _camera;

    private Vector3 _gridHighlightTgtPos;
    private static readonly int HighlightedSquareShaderId = Shader.PropertyToID("_HighlightedSquare");
    private static readonly int NumSquaresShaderId = Shader.PropertyToID("_HighlightedSquareSize");

    private void Awake() {
        _camera = Camera.main;
        _highlighterRenderer = GetComponent<Renderer>();
    } 
 
    void Update() {
        var layerMask = 1 << LayerMask.NameToLayer("MouseRaycastLayer");
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit, 100, layerMask)) {
            var hitPos = hit.point;
            var hitPosMod = new Vector3(Mathf.Floor(hitPos.x), hitPos.y, Mathf.Floor(hitPos.z));
            hitPosMod += GetHighlightOffset();
            _gridHighlightTgtPos = hitPosMod;
        }
        
        transform.position = Vector3.Lerp(transform.position, _gridHighlightTgtPos, Time.deltaTime * 15);
        _highlighterRenderer.material.SetVector(HighlightedSquareShaderId, new Vector4(_gridHighlightTgtPos.x, _gridHighlightTgtPos.z, 0, 0));
        _highlighterRenderer.material.SetFloat(NumSquaresShaderId, _numCellsWideToHighlight);
    }

    private Vector3 GetHighlightOffset() {
        var offset = _numCellsWideToHighlight / 2f;
        return new Vector3(offset, 0, offset);
    }
}
