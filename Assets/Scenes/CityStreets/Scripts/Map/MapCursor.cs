using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MapCursor : MonoBehaviour {
    [SerializeField] private Pulse upNav;
    [SerializeField] private Pulse rightNav;
    [SerializeField] private Pulse downNav;
    [SerializeField] private Pulse leftNav; 

    public void SetAtMapNode(MapNode node) {
        transform.position = node.transform.position;
        ShowNavArrows(node.NavUp != null, node.NavRight != null, node.NavDown != null, node.NavLeft != null);
    }

    public void SetTransitionMode() {
        ShowNavArrows(false, false, false, false);
    }
    
    private void ShowNavArrows(bool up, bool right, bool down, bool left) {
        upNav.gameObject.SetActive(up);
        rightNav.gameObject.SetActive(right);
        downNav.gameObject.SetActive(down);
        leftNav.gameObject.SetActive(left);
    }

    public void EmphasizeDir(ArrowDir? dir) {
        upNav.SetPulsing(dir == ArrowDir.Up);
        rightNav.SetPulsing(dir == ArrowDir.Right);
        leftNav.SetPulsing(dir == ArrowDir.Left);
        downNav.SetPulsing(dir == ArrowDir.Down);
    }
}

public enum ArrowDir {
    Up,
    Right,
    Left,
    Down
}
