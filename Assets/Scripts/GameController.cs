using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [SerializeField] private GameConfigScriptableObject gameConfig;

    private static readonly int WorldBendMagnitudeShaderId = Shader.PropertyToID("_WorldBendMagnitude");
    
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
    }

    private void SetStartupGameConfig()
    {
        Shader.SetGlobalFloat(WorldBendMagnitudeShaderId, gameConfig.WorldShaderCurveAmount);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
