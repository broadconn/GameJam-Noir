using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCursor : MonoBehaviour {
    [SerializeField] private Transform upNav;
    [SerializeField] private Transform rightNav;
    [SerializeField] private Transform downNav;
    [SerializeField] private Transform leftNav;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAtMapNode(MapNode node) {
        transform.position = node.transform.position;
    }

    public void ShowNavArrows(bool up, bool right, bool down, bool left) {
        upNav.gameObject.SetActive(up);
        rightNav.gameObject.SetActive(right);
        downNav.gameObject.SetActive(down);
        leftNav.gameObject.SetActive(left);
    }
}
