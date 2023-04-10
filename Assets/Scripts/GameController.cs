using System;
using DefaultNamespace;
using UnityEngine;

/// <summary>
/// Essentially a singleton hub to get easy references to specific controllers. 
/// Use for scene-agnostic things, e.g. overall game state, audio settings etc
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    public static readonly string CitySceneName = "City";
    public static readonly string ConversationSceneName = "Conversation";
    
    [SerializeField] private GameConfigScriptableObject gameConfig;
    public SceneFader SceneFader;
    public StoryController StoryController; // should this be in ConversationController? I'm thinking so.

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
        PlayerPrefs.SetInt(StoryController.EnteringStoryIdPrefName, (int)storyId);
        SceneFader.FadeToScene(ConversationSceneName);
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