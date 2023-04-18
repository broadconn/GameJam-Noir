using System;
using UnityEngine;

/// <summary>
/// Essentially a singleton hub to get easy references to specific controllers. 
/// Use for scene-agnostic things, e.g. overall game state, audio settings etc
/// </summary>
public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }
    
    [SerializeField] private GameConfigScriptableObject gameConfig;
    
    public SceneFader SceneFader => _sceneFader;
    private SceneFader _sceneFader;
    public StoryController StoryController => _storyController;
    private StoryController _storyController; 

    public static readonly string CitySceneName = "City";
    public static readonly string ConversationSceneName = "Conversation";
    private static readonly int WorldBendMagnitudeShaderId = Shader.PropertyToID("_WorldBendMagnitude");

    #region Singleton stuff
    void Awake()
    {
        EnsureOneInstance();
        _sceneFader = GetComponent<SceneFader>();
        _storyController = GetComponent<StoryController>();
        SetStartupGameConfig();
    }

    public void StartNewGame() {
        PlayerPrefs.DeleteAll(); 
        StoryController.StartConversation(StoryId.Intro);
    }

    private void EnsureOneInstance()
    {
        if (Instance != null && Instance != this) {
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
    Undefined,
    Intro,
    Diner,
    Morgue,
    Bar,
    Mistress,
    Eavesdrop,
    Warehouse
}