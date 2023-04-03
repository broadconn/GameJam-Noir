using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use for scene-agnostic things, e.g. game state, audio settings etc
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [SerializeField] private GameConfigScriptableObject gameConfig;
    
    [SerializeField] private List<StoryConversation> story;
    private const string LastStoryIdPrefName = "StoryID";
    private StoryId? _lastStoryId = null; // should be set at the end of every conversation

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
        // saved shader values
        Shader.SetGlobalFloat(WorldBendMagnitudeShaderId, gameConfig.WorldShaderCurveAmount);
        
        // load story beat played last session
        _lastStoryId = (StoryId) PlayerPrefs.GetInt(LastStoryIdPrefName);
    }
    #endregion

    public StoryId GetNextStory()
    {
        return _lastStoryId ?? StoryId.Intro;
    }

    public void StartConversation(string storyId)
    {
        
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