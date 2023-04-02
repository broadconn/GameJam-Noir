using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySceneController : MonoBehaviour
{
    [SerializeField] private PlayerCityToken player;

    [SerializeField] private CameraController cameraController;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
