using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GridHighlighter gridHighlighter; 
    private Camera _camera;

    private ActionState _actionState;
    
    private void Awake() {
        _camera = Camera.main;
    }

    private void Start() {
        SetActionState(ActionState.Normal);
    }

    private void SetActionState(ActionState actionState) {
        _actionState = actionState;

        switch (_actionState) {
            case ActionState.Normal:
                gridHighlighter.gameObject.SetActive(false);
                break;
            case ActionState.PlacingBuilding:
                gridHighlighter.gameObject.SetActive(true);
                break;
        }
    }

    private void Update() {
        switch (_actionState) {
            case ActionState.Normal:
                HandleNormalState();
                break;
            case ActionState.PlacingBuilding:
                HandlePlacingBuildingState();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void HandleNormalState() {
        
    }

    private void HandlePlacingBuildingState() {
        var mouseWorldPos = GetMouseWorldPosition();
        gridHighlighter.SetMouseWorldPos(mouseWorldPos);
    }

    private Vector3 GetMouseWorldPosition() {
        const int maxCastDistance = 100;
        var layerMask = 1 << LayerMask.NameToLayer("MouseRaycastLayer");
        var mouseToWorldRay = _camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(mouseToWorldRay, out var hit, maxCastDistance, layerMask) ? hit.point : Vector3.positiveInfinity;
    }
}

public enum ActionState {
    Normal,
    PlacingBuilding
}
