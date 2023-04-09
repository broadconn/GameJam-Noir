using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Essentially a singleton hub to get easy references to specific controllers. 
/// Use for scene-agnostic things, e.g. game state, audio settings etc
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [SerializeField] private GameConfigScriptableObject gameConfig;
    public SceneFader SceneFader;
    public StoryController StoryController;

    private static readonly int WorldBendMagnitudeShaderId = Shader.PropertyToID("_WorldBendMagnitude");

    #region Startup
    void Awake()
    {
        EnsureOneInstance();
        SetStartupGameConfig();
    }

    private void EnsureOneInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject); // survive scene changes
    }

    private void SetStartupGameConfig()
    {
        Shader.SetGlobalFloat(WorldBendMagnitudeShaderId, gameConfig.WorldShaderCurveAmount);
    }
    #endregion

    public void StartConversation(StoryId storyId) {
        SceneFader.FadeToScene("Conversation");
    }
}

[Serializable]
public class StoryConversation
{
    public StoryId Id;
    [TextArea]
    public string Dialogue;
}

public enum StoryId
{
    Intro,
    Widow,
    Morgue,
    Bar,
    Mistress,
    Eavesdrop,
    Warehouse
}