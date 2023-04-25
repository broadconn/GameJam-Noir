using System;
using UnityEngine;

/// <summary>
/// Essentially a singleton hub to get easy references to specific controllers. 
/// Use for scene-agnostic things, e.g. overall game state, audio settings etc
/// </summary>
public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }
    
    [SerializeField] private GameConfigScriptableObject gameConfig;
    
    public SceneFader SceneFader { get; private set; }
    public StoryController StoryController { get; private set; }
    public MusicController MusicController { get; private set; }

    public const string CitySceneName = "City";
    public const string ConversationSceneName = "Conversation";
    private static readonly int WorldBendMagnitudeShaderId = Shader.PropertyToID("_WorldBendMagnitude");

    #region Singleton stuff
    void Awake()
    {
        EnsureOneInstance();
        SceneFader = GetComponent<SceneFader>();
        StoryController = GetComponent<StoryController>();
        MusicController = GetComponent<MusicController>();
        SetStartupGameConfig();
    }

    public void StartNewGame() {
        PlayerPrefs.DeleteAll(); 
        StoryController.StartConversation(StoryId.Intro);
    }

    private void EnsureOneInstance()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); 
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

public enum CityLocation
{
    Undefined,
    Diner,
    Morgue,
    Bar,
    Warehouse
}