using UnityEditor;
using UnityEngine;

namespace Editor
{
    static class OnUnityOpen
    {
        private static readonly int WorldBendMagnitude = Shader.PropertyToID("_WorldBendMagnitude");

        /// <summary>
        /// Contains the code we want to execute when we open Unity.
        /// </summary>
        [InitializeOnLoadMethod]
        static void MyStartupCode ()
        {
            if (SessionState.GetBool("FirstInitDone", false)) 
                return;
        
            Debug.Log("Executing init code.");
            RunStartupCode();
            SessionState.SetBool("FirstInitDone", true);
        }

        private static void RunStartupCode()
        {
            // set the world curve shader property to the value saved in the scriptable object
            var configSettings =
                (GameConfigScriptableObject)AssetDatabase.LoadAssetAtPath("Assets/Settings/Game/GameConfig.asset",
                    typeof(GameConfigScriptableObject));
            Shader.SetGlobalFloat(WorldBendMagnitude, configSettings.WorldShaderCurveAmount);
        }
    }
}