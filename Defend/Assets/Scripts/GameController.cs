using System;
using System.Collections;
using System.Collections.Generic;
using GameStates;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GridHighlighter gridHighlighter; 

    private GameplayProcessor _gameplayProcessor;
    private Dictionary<GameplayState, GameplayProcessor> _gameplayStateToProcessorMappings;

    private void Awake() {
        var ctx = new GameplayStateContext {
            MainCamera = Camera.main,
            GridHighlighter = gridHighlighter
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
        _gameplayProcessor.HandleKeyboardInput();
        _gameplayProcessor.Update();
        
        if (_gameplayProcessor.StateChanged) 
            SetGameplayState(_gameplayProcessor.NextState); 
    }

    public void ClickedBuildBuildingButton(string buildingId) {
        // TODO: check if the player can afford this building

        GameObject buildingTower = null;
        SetGameplayState(GameplayState.PlacingBuilding, buildingTower);
    }

    private void SetGameplayState(GameplayState gameplayState, GameObject referenceObject = null) {
        _gameplayProcessor?.OnExitState();
        
        _gameplayProcessor = _gameplayStateToProcessorMappings[gameplayState]; // TODO: check mapping exists
        if(_gameplayProcessor is IGameplayProcessorReferencingGameObject processor)
            processor.SetReferenceObject(referenceObject);
        
        _gameplayProcessor.OnEnterState();
    }
}
