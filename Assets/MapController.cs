 using UnityEngine; 

public class MapController : MonoBehaviour {
    [SerializeField] private CanvasGroup uiGroup;    
    [SerializeField] private float uiFadeTime = 2f;
    [SerializeField] private float uiFadeInDelay = 0.7f;
    
    private CityMode _curMode;
    private float _timeSwitchedModes; 
 
    void Update() {
        var timePassed = Time.time - _timeSwitchedModes;
        var percThrough = Mathf.Clamp01(timePassed / uiFadeTime); 
        uiGroup.alpha = Mathf.Lerp(_curMode == CityMode.Map ? 0 : 1, _curMode == CityMode.Map ? 1 : 0, percThrough); 
    }

    public void SetMode(CityMode mode) {
        _curMode = mode;
        _timeSwitchedModes = Time.time + (mode == CityMode.Map ? uiFadeInDelay : 0); 
    }
    
    // highlight target destination
    // show navigation icon
}
