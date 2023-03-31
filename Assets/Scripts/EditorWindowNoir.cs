using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using Random = UnityEngine.Random;

class NoirDevToolsEditor : EditorWindow {
    private float _worldShaderBendAmount = 2;
    private GameObject[] _spawnPoints = Array.Empty<GameObject>();

    private static readonly int WorldBendMagnitude = Shader.PropertyToID("_WorldBendMagnitude");

    [MenuItem ("**Noir**/Dev Tools")]
    public static void  ShowWindow () {
        GetWindow(typeof(NoirDevToolsEditor));
    }
    
    void OnGUI () {
        ShowShaderSettings();
        GUILayout.Space(15);
        ShowWayPoints();
    }

    private void ShowShaderSettings()
    {
        GUILayout.Label ("Shader settings", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal(); 
        _worldShaderBendAmount = EditorGUILayout.Slider ("World Bend Amount", _worldShaderBendAmount, 0, 20);
        if (GUILayout.Button("Apply", GUILayout.Width(100)))
        { 
            Shader.SetGlobalFloat(WorldBendMagnitude, -1 * _worldShaderBendAmount);
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal(); 
        GUILayout.Label("Set bend origin to player position");
        if (GUILayout.Button("Apply", GUILayout.Width(100)))
        {
            var player = GameObject.FindWithTag("PlayerCityToken");
            player.GetComponent<PlayerCityToken>().SetGlobalShaderPosition();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("(use if you manually move the player)", EditorStyles.miniLabel);
    }

    private void ShowWayPoints()
    {
        GUILayout.Label("City Spawn Points", EditorStyles.boldLabel);
        if(_spawnPoints.Length == 0)
            if (GUILayout.Button("List spawn points")) 
                _spawnPoints = GameObject.FindGameObjectsWithTag("CitySpawnPoint"); 
        
        foreach (var wp in _spawnPoints)
        {
            EditorGUILayout.BeginHorizontal(); 
            if (GUILayout.Button(wp.name, GUILayout.Width(100)))
            {
                // move player to the gameobject location
                var player = GameObject.FindWithTag("PlayerCityToken");
                player.transform.position = wp.transform.position;
                
                // update the world bend shader position
                player.GetComponent<PlayerCityToken>().SetGlobalShaderPosition();
                
                // bring the scene camera with us
                var playerPos = player.transform.position;
                SceneView.lastActiveSceneView.pivot = playerPos + new Vector3(0, 0, -10);
                SceneView.lastActiveSceneView.LookAt(playerPos, Quaternion.Euler(45, 0, 0));
            }
            GUILayout.Label(wp.transform.position.ToString(), EditorStyles.miniLabel);
            EditorGUILayout.EndHorizontal();
        }
        if(_spawnPoints.Length > 0)
            GUILayout.Label("Click to set player position", EditorStyles.miniLabel);
    }
}