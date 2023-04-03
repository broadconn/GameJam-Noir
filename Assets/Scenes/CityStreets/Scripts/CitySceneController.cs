using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySceneController : MonoBehaviour
{
    [SerializeField] private PlayerCityToken player;

    [SerializeField] private CameraController cameraController;

    private string curProgressId = string.Empty;
    
    // Start is called before the first frame update
    void Start()
    {
        // check which story trigger should be active
        //  - get the last done story ID from the gameController
        //  - get the next story ID from the gameController (maybe just need to do this)
        //  - activate the waypoint with this ID
        
        // check where the player should be
        //  - car park | where the story was initiated | map
        //  - ask the gameController for this
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SetMode(CityMode.Map);
        if(Input.GetKeyDown(KeyCode.DownArrow))
            SetMode(CityMode.Street);
    }

    private void SetMode(CityMode mode)
    {
        player.SetMode(mode);
        cameraController.SetMode(mode);
    }
}

public enum CityMode
{
    Street,
    Map
}
