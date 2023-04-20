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
    [Space(5)] 
    [SerializeField] private PlayerCityToken player;
    [SerializeField] private float nodeTransitionTime = 3f;

    private List<MapNode> _nodes;
    
    private CityMode _curMode;
    private float _timeSwitchedModes = float.MinValue;
    private MapNode _curNode, _nextNode;
    private ArrowDir? _nextStoryDir;

    private bool _isTransitioning = false;
    private float _timeTriggeredTransition = float.MinValue;

    private void Awake() {
        _nodes = nodesParent.GetComponentsInChildren<MapNode>(includeInactive: true).ToList();
    }

    void Update() {
        UpdateUiFade();

        if(_curMode == CityMode.Map)
            HandleMapInput();
        
        if(_isTransitioning)
            UpdateNodeTransition();
    }

    public void SetMode(CityMode mode) {
        if (mode == _curMode) return;
        _curMode = mode;
        _timeSwitchedModes = Time.time + (mode == CityMode.Map ? uiFadeInDelay : 0);

        if (mode == CityMode.Street) return;
        SetupMap();
    }
 
    private void SetupMap() {
        var curStoryId = GameController.Instance.StoryController.GetLastStoryId();
        var nextStoryId = GameController.Instance.StoryController.GetNextStoryId();
        var curCityLocation = GameController.Instance.StoryController.StoryIdToLocation(curStoryId);
        var nextCityLocation = GameController.Instance.StoryController.StoryIdToLocation(nextStoryId);
        _curNode = _nodes.FirstOrDefault(n => n.CitySpawnLocation == curCityLocation);
        _nextStoryDir = GetNextStoryDir(_curNode, nextStoryId);
 
        // set cursor position 
        if(_curNode != null)
            mapCursor.transform.position = _curNode.transform.position;
        mapCursor.gameObject.SetActive(_curNode != null);
        mapCursor.EmphasizeDir(_nextStoryDir);
        
        // set destination highlight
        var nextNode = _nodes.FirstOrDefault(n => n.CitySpawnLocation == nextCityLocation);
        if(nextNode != null)
            mapHighlight.transform.position = nextNode.transform.position;
        mapHighlight.gameObject.SetActive(nextNode != null); 
    }

    ArrowDir? GetNextStoryDir(MapNode curNode, StoryId nextStoryId) {
        if (curNode == null) return null;
        var nextCityLocation = GameController.Instance.StoryController.StoryIdToLocation(nextStoryId);
        var nodeNodes = curNode.NavNodes;
        var destinationNode = nodeNodes.Where(n => n.Node != null)
            .FirstOrDefault(n => n.Node.CitySpawnLocation == nextCityLocation);  
        return destinationNode.Node == null ? null : destinationNode.Direction; 
    }

    private void UpdateUiFade() {
        var timePassed = Time.time - _timeSwitchedModes;
        var percThrough = Mathf.Clamp01(timePassed / uiFadeTime);
        uiGroup.alpha = Mathf.Lerp(_curMode == CityMode.Map ? 0 : 1, _curMode == CityMode.Map ? 1 : 0, percThrough);
    }

    // Moving between nodes / navigating the map
    private void UpdateNodeTransition() {
        var timePassed = Time.time - _timeTriggeredTransition;
        var percThrough = Mathf.Clamp01(timePassed / nodeTransitionTime);

        mapCursor.transform.position = Vector2.Lerp(_curNode.transform.position, _nextNode.transform.position, percThrough);
        player.transform.position = Vector3.Lerp(_curNode.WorldDestination.position, _nextNode.WorldDestination.position, percThrough);

        // check for transition end
        if (Math.Abs(percThrough - 1) < 0.001f) {
            _isTransitioning = false;
            mapCursor.SetAtMapNode(_nextNode);
            _curNode = _nextNode;
        }
    }
    
    /// Only allows navigation if you press the button that goes to the next story location.
    /// The plan for a full game would be to allow navigation anywhere. 
    private void HandleMapInput() {
        if (_nextStoryDir == null) return; 
        
        switch (_nextStoryDir) {
            case ArrowDir.Up:
                if (Input.GetKeyDown(KeyCode.W)) 
                    GoToNextLocation();  
                break;
            case ArrowDir.Right:
                if (Input.GetKeyDown(KeyCode.D)) { 
                    GoToNextLocation();
                }
                break;
            case ArrowDir.Left:
                if (Input.GetKeyDown(KeyCode.A))  
                    GoToNextLocation();  
                break;
            case ArrowDir.Down:
                if (Input.GetKeyDown(KeyCode.S)) {
                    GoToNextLocation();
                }
                break;
        }
    }

    private void GoToNextLocation() {
        var nextNodeDir = GetNodeInDir(_curNode, _nextStoryDir);
        if (nextNodeDir == null) return;

        _nextNode = nextNodeDir;
        
        mapCursor.SetTransitionMode();

        _isTransitioning = true;
        _timeTriggeredTransition = Time.time;
    }

    private MapNode GetNodeInDir(MapNode curNode, ArrowDir? dir) { 
        return curNode.NavNodes.FirstOrDefault(n => n.Direction == dir).Node;
    }
}
