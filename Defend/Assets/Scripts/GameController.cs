using System;
using System.Collections;
using System.Collections.Generic;
using GameStates;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GridHighlighter gridHighlighter; 

    private GameplayStateProcessor _gameplayStateProcessor;
    private Dictionary<GameplayState, GameplayStateProcessor> _gameplayStateMappings;

    private void Awake() {
        var ctx = new GameplayStateContext {
            MainCamera = Camera.main,
            GridHighlighter = gridHighlighter
        };
        _gameplayStateMappings = new Dictionary<GameplayState, GameplayStateProcessor> {
            { GameplayState.Normal, new NormalGameplayStateProcessor(ctx) },
            { GameplayState.PlacingBuilding, new PlacingBuildingStateProcessor(ctx) }
        };
    }

    private void Start() {
        SetGameplayState(GameplayState.Normal);
    }

    private void Update() {
        _gameplayStateProcessor.HandleKeyboardInput();
        _gameplayStateProcessor.Update();
        
        if (_gameplayStateProcessor.StateChanged) 
            SetGameplayState(_gameplayStateProcessor.NextState); 
    }

    private void SetGameplayState(GameplayState gameplayState) {
        _gameplayStateProcessor = _gameplayStateMappings[gameplayState]; // TODO: check mapping exists
        _gameplayStateProcessor.OnEnterState();
    }

    public void ClickedBuildBuildingButton(string buildingId) {
        // TODO: check if the player can afford this building
        SetGameplayState(GameplayState.PlacingBuilding);
    }
}
