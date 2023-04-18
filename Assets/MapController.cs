 using System;
 using System.Collections.Generic;
 using System.Linq;
 using UnityEngine; 

public class MapController : MonoBehaviour {
    [SerializeField] private CanvasGroup uiGroup;    
    [SerializeField] private float uiFadeTime = 2f;
    [SerializeField] private float uiFadeInDelay = 0.7f;
    [Space(5)] 
    [SerializeField] private Transform nodesParent;
    [SerializeField] private MapCursor mapCursor;
    [SerializeField] private Transform mapHighlight;

    private List<MapNode> nodes;
    
    private CityMode _curMode;
    private MapNode _curNode;
    private float _timeSwitchedModes;

    private void Awake() {
        nodes = nodesParent.GetComponentsInChildren<MapNode>(includeInactive: true).ToList();
    }

    void Update() {
        UpdateUiFade();

        if(_curMode == CityMode.Map)
            HandleMapInput();
    }

    private void UpdateUiFade() {
        var timePassed = Time.time - _timeSwitchedModes;
        var percThrough = Mathf.Clamp01(timePassed / uiFadeTime);
        uiGroup.alpha = Mathf.Lerp(_curMode == CityMode.Map ? 0 : 1, _curMode == CityMode.Map ? 1 : 0, percThrough);
    }

    private void HandleMapInput() {
        // if(Input.GetKeyDown(KeyCode.W)) 
    }

    public void SetMode(CityMode mode) {
        _curMode = mode;
        _timeSwitchedModes = Time.time + (mode == CityMode.Map ? uiFadeInDelay : 0);

        if (mode == CityMode.Street) return;
        SetupMap();
    }
 
    void SetupMap() {
        var curStoryId = GameController.Instance.StoryController.GetLastStoryId();
        var nextStoryId = GameController.Instance.StoryController.GetNextStoryId();
 
        // set cursor position 
        _curNode = nodes.FirstOrDefault(n => n.CitySpawnLocation == curStoryId);
        if(_curNode != null)
            mapCursor.transform.position = _curNode.transform.position;
        mapCursor.gameObject.SetActive(_curNode != null);
        
        // set destination highlight
        var nextNode = nodes.FirstOrDefault(n => n.CitySpawnLocation == nextStoryId);
        if(nextNode != null)
            mapHighlight.transform.position = nextNode.transform.position;
        mapHighlight.gameObject.SetActive(nextNode != null);

        // TODO set cursor arrows
        

    }
}
