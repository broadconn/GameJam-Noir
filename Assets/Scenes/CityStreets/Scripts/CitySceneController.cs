using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CitySceneController : MonoBehaviour
{
    [SerializeField] private PlayerCityToken player;

    [SerializeField] private CameraController cameraController;

    [SerializeField] private Transform storyTriggersParent;
    
    // Start is called before the first frame update
    void Start()
    {
        EnableNextStoryTrigger();
        SetPlayerAtLastStoryTrigger();
    }

    private void EnableNextStoryTrigger()
    { 
        var nextStoryId = GameController.Instance.GetNextStoryId();
        foreach (Transform t in storyTriggersParent) {
            t.gameObject.SetActive(t.GetComponent<StoryTrigger>()?.GetID() == nextStoryId); 
        }
    }

    private void SetPlayerAtLastStoryTrigger()
    {
        var lastStoryId = GameController.Instance.GetLastStoryId() ?? StoryId.Intro;
        
        var lastStoryTrigger = storyTriggersParent.GetComponentsInChildren<StoryTrigger>()
            .FirstOrDefault(st => st.GetID() == lastStoryId);
        
        if (lastStoryTrigger != null)
            player.transform.position = lastStoryTrigger.transform.position;
        else
            Debug.LogWarning("Couldn't figure out where to put the player 🤔");
    }

    // Update is called once per frame
    void Update()
    {
        # if UNITY_EDITOR
        HandleDebugInput();
        # endif
    }

    private void HandleDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
            SetMode(CityMode.Map);
        if (Input.GetKeyDown(KeyCode.DownArrow))
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
