using UnityEngine;

/// <summary>
/// Settings that finalize how the game works or looks. Not for users.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameConfigScriptableObject", order = 1)]
public class GameConfigScriptableObject : ScriptableObject
{
    public float WorldShaderCurveAmount = 0;
}