using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [SerializeField] private GameConfigScriptableObject gameConfig;

    private static readonly int WorldBendMagnitudeShaderId = Shader.PropertyToID("_WorldBendMagnitude");

    #region Startup
    void Awake()
    {
        EnsureOneInstance();
        DontDestroyOnLoad(gameObject); // survive scene changes
        
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
    }

    private void SetStartupGameConfig()
    {
        Shader.SetGlobalFloat(WorldBendMagnitudeShaderId, gameConfig.WorldShaderCurveAmount);
    }
    #endregion

    public void SwitchToMap()
    {
        
    }
}
