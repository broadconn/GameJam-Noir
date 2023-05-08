using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using GameStates;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GridHighlighter gridHighlighter; 
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform enemySpawnTransform;
    [SerializeField] private Transform enemyGoalTransform;
    [SerializeField] private Transform debugPathDotsParent;

    private GameplayProcessor _gameplayProcessor;
    private Dictionary<GameplayState, GameplayProcessor> _gameplayStateToProcessorMappings;

    private void Awake() {
        var ctx = new GameplayStateContext {
            MainCamera = Camera.main,
            GridHighlighter = gridHighlighter,
            PathController = new PathController(enemySpawnTransform.position.WorldXZToV2Int(), enemyGoalTransform.position.WorldXZToV2Int(), debugPathDotsParent),
            CameraController = cameraController
        };
        _gameplayStateToProcessorMappings = new Dictionary<GameplayState, GameplayProcessor> {
            { GameplayState.Normal, new NormalGameplayProcessor(ctx) },
            { GameplayState.PlacingBuilding, new PlacingBuildingProcessorProcessor(ctx) }
        };
    }

    private void Start() {
        SetGameplayState(GameplayState.Normal);
    }

    private void Update() {
        cameraController.DoUpdate();
        _gameplayProcessor.Update();
        
        if (_gameplayProcessor.StateChanged) 
            SetGameplayState(_gameplayProcessor.NextState); 
    }

    private void SetGameplayState(GameplayState gameplayState, GameObject referenceObject = null) {
        _gameplayProcessor?.OnExitState();
        
        _gameplayProcessor = _gameplayStateToProcessorMappings[gameplayState]; // TODO: check mapping exists
        if(_gameplayProcessor is IGameplayProcessorReferencingGameObject processor)
            processor.SetReferenceObject(referenceObject);
        
        _gameplayProcessor.OnEnterState();
    }

    public void ClickedBuildBuildingButton(GameObject buildingPrefab) {
        // TODO: check if the player can afford this building
        SetGameplayState(GameplayState.PlacingBuilding, buildingPrefab);
    }
}

public static class UtilityExtensions {
    public static Vector2Int WorldXZToV2Int(this Vector3 pos) {
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.z));
    }
}