using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CitySceneController : MonoBehaviour
{
    [SerializeField] private PlayerCityToken player;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private CityVisualsController visualsController;
    [SerializeField] private MapController mapController;
    [SerializeField] private Transform storyTriggersParent;

    private StoryTrigger[] _storyTriggers;
     
    void Start() {
        _storyTriggers = storyTriggersParent.GetComponentsInChildren<StoryTrigger>(includeInactive: true);
        EnableNextStoryTrigger();
        SetPlayerAtLastStoryTrigger();
    }

    private void EnableNextStoryTrigger()
    { 
        var nextStoryId = GameController.Instance.StoryController.GetNextStoryId();
        foreach (Transform t in storyTriggersParent) {
            t.gameObject.SetActive(t.GetComponent<StoryTrigger>()?.GetID() == nextStoryId); 
        }
    }

    /// <summary>
    /// Lets the player resume city gameplay from where they started the last conversation
    /// </summary>
    private void SetPlayerAtLastStoryTrigger()
    {
        var lastStoryId = GameController.Instance.StoryController.GetLastStoryId() ?? StoryId.Intro;

        var lastStoryTrigger = _storyTriggers
            .FirstOrDefault(st => st.GetID() == lastStoryId);

        if (lastStoryTrigger != null) {
            player.TeleportToPosition(lastStoryTrigger.transform.position);
            cameraController.ForceCameraRotation(lastStoryTrigger.SpawnRotation);
            Debug.Log("Placing player at story trigger for " + lastStoryId + " " + lastStoryTrigger.transform.position);
        }
        else
            Debug.LogWarning("Couldn't figure out where to put the player for ID " + lastStoryId);
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            SetMode(CityMode.Map);
            GameController.Instance.MusicController.FadeOutMusic();
        }
    }

    public void SetMode(CityMode mode)
    {
        player.SetMode(mode);
        cameraController.SetMode(mode);
        visualsController.SetMode(mode);
        mapController.SetMode(mode);
    }
}

public enum CityMode
{
    Street,
    Map
}
