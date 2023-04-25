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
    [SerializeField] private GameObject enterZonePrompt;
    [Space(5)] 
    [SerializeField] private CitySceneController cityController;
    [SerializeField] private CityGui cityGui;
    [SerializeField] private PlayerCityToken player;
    [SerializeField] private float nodeTransitionTime = 3f;

    private List<MapNode> _nodes;
    private MapNode _curNode, _nextNode;

    private CityMode _curMode;
    private float _timeSwitchedModes = float.MinValue; 
    
    private StoryId? _lastDoneStoryId, _nextStoryId;
    private CityLocation _curCityLocation, _nextCityLocation;
    private ArrowDir? _nextStoryDir;
    
    private bool _isTransitioning;
    private float _timeTriggeredTransition = float.MinValue;
    
    public const string LocationPrefName = "MapLocation";

    private void Awake() {
        _nodes = nodesParent.GetComponentsInChildren<MapNode>(includeInactive: true).ToList();
        
        _lastDoneStoryId = GameController.Instance.StoryController.GetLastStoryId();
        _nextStoryId = GameController.Instance.StoryController.GetNextStoryId();
        _curCityLocation = GameController.Instance.StoryController.StoryIdToLocation(_lastDoneStoryId);
        _nextCityLocation = GameController.Instance.StoryController.StoryIdToLocation(_nextStoryId);
    }

    void Update() {
        if (_curMode == CityMode.Map) {
            if(!_isTransitioning)
                HandleMapInput();
            
            enterZonePrompt.SetActive(!_isTransitioning && IsAtNextStoryNode());
        }
        
        UpdateUiFade();

        if(_isTransitioning)
            UpdateNodeTransition();
    }

    private bool IsAtNextStoryNode() {
        return _curNode == _nextNode;
    }

    public void SetMode(CityMode mode) {
        if (mode == _curMode) return;
        _curMode = mode;
        _timeSwitchedModes = Time.time + (mode == CityMode.Map ? uiFadeInDelay : 0);

        if (mode == CityMode.Street) return;
        SetupMap();
        UpdateOtherUI();
    }

    private void UpdateOtherUI() {
        cityGui.ShowMapPrompt(false);
    }

    private void SetupMap() {
        _curNode = _nodes.FirstOrDefault(n => n.CitySpawnLocation == _curCityLocation);
        _nextStoryDir = GetNextStoryDir(_curNode);
 
        // map cursor
        if (_curNode != null) {
            mapCursor.SetAtMapNode(_curNode);
            mapCursor.transform.position = _curNode.transform.position;
        }
        mapCursor.gameObject.SetActive(_curNode != null);
        mapCursor.EmphasizeDir(_nextStoryDir);
        
        // set destination highlight
        var nextNode = _nodes.FirstOrDefault(n => n.CitySpawnLocation == _nextCityLocation);
        if(nextNode != null)
            mapHighlight.transform.position = nextNode.transform.position;
        mapHighlight.gameObject.SetActive(nextNode != null); 
    }

    private ArrowDir? GetNextStoryDir(MapNode curNode) {
        if (curNode == null) return null; 
        var nodeNodes = curNode.NavNodes;
        var destinationNode = nodeNodes.Where(n => n.Node != null)
            .FirstOrDefault(n => n.Node.CitySpawnLocation == _nextCityLocation);  
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
 
        if (Math.Abs(percThrough - 1) < 0.001f) {
            OnMapTransitionEnd();
        }
    }

    private void OnMapTransitionEnd() {
        _isTransitioning = false;
        mapCursor.SetAtMapNode(_nextNode);
        _curNode = _nextNode;
        PlayerPrefs.SetInt(LocationPrefName, (int)_curNode.CitySpawnLocation);
    }

    /// Only allows navigation if you press the button that goes to the next story location.
    /// The plan for a full game would be to allow navigation anywhere, let the player hunt for the location. 
    private void HandleMapInput() {
        if (_nextStoryDir == null) return;

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            print("Hit down arrow");
            cityController.SetMode(CityMode.Street);
            GameController.Instance.MusicController.OnLocationChange();
        }

        switch (_nextStoryDir) {
            case ArrowDir.Up:
                if (Input.GetKeyDown(KeyCode.W)) 
                    GoToNextLocation();  
                break;
            case ArrowDir.Right:
                if (Input.GetKeyDown(KeyCode.D)) 
                    GoToNextLocation();
                break;
            case ArrowDir.Left:
                if (Input.GetKeyDown(KeyCode.A))  
                    GoToNextLocation();  
                break;
            case ArrowDir.Down:
                if (Input.GetKeyDown(KeyCode.S))
                    GoToNextLocation();
                break;
        }
    }

    /// <summary>
    /// Initiates moving the player and map cursor from the current node to the next story node
    /// </summary>
    private void GoToNextLocation() {
        var nextNodeDir = GetNodeInDir(_curNode, _nextStoryDir);
        if (nextNodeDir == null) return;

        _nextNode = nextNodeDir;
        
        mapCursor.SetTransitionMode();

        _isTransitioning = true;
        _timeTriggeredTransition = Time.time;
    }

    private static MapNode GetNodeInDir(MapNode curNode, ArrowDir? dir) { 
        return curNode.NavNodes
            .FirstOrDefault(n => n.Direction == dir)
            .Node;
    }
}
