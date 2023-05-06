using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlighter : MonoBehaviour {
    private int _numCellsWideToHighlight = 2; // e.g. if placing a 1x1 or 2x2-width tower
    private Renderer _highlighterRenderer;

    private Vector3 _gridHighlightTgtPos;
    private static readonly int HighlightedSquareShaderId = Shader.PropertyToID("_HighlightedSquare");
    private static readonly int NumSquaresShaderId = Shader.PropertyToID("_HighlightedSquareSize");

    private void Awake() {
        _highlighterRenderer = GetComponent<Renderer>();
    } 
 
    private void Update() {
        transform.position = Vector3.Lerp(transform.position, _gridHighlightTgtPos, Time.deltaTime * 15);
        _highlighterRenderer.material.SetVector(HighlightedSquareShaderId, new Vector4(_gridHighlightTgtPos.x, _gridHighlightTgtPos.z, 0, 0));
    }
    
    public void SetMouseWorldPos(Vector3 mouseWorldPos, bool instant = false){
        var worldPosFloored = new Vector3(Mathf.Floor(mouseWorldPos.x), 0, Mathf.Floor(mouseWorldPos.z));
        _gridHighlightTgtPos = worldPosFloored + GetHighlightOffset();

        if (instant) 
            transform.position = _gridHighlightTgtPos; 
    }

    public void SetBuildingWidth(int buildingWidth) {
        _numCellsWideToHighlight = buildingWidth;
        _highlighterRenderer.material.SetFloat(NumSquaresShaderId, _numCellsWideToHighlight);
    }

    /// <summary>
    /// For modifying the visual's position depending on how wide the building being placed is
    /// </summary>
    /// <returns></returns>
    private Vector3 GetHighlightOffset() {
        var offset = _numCellsWideToHighlight / 2f;
        return new Vector3(offset, 0, offset);
    }
}
