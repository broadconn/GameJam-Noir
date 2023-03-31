using UnityEditor;
using UnityEngine;

public class EditorMenuItems : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("**Noir**/Reset shader position")]
    static void ResetShader()
    {
        var player = GameObject.FindWithTag("PlayerCityToken"); 
        player.GetComponent<PlayerCityToken>().SetGlobalShaderPosition(); 
    }
}
