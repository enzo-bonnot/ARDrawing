using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalActionsManager : MonoBehaviour
{
    [SerializeField] private GameObject paintMenu;

    private PaintActionsManager paintActionsManager;
    // Start is called before the first frame update
    void Start()
    {
        paintActionsManager = paintMenu.GetComponent<PaintActionsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInput(GameObject target)
    {
        if (paintMenu.activeInHierarchy)
        {
            paintActionsManager.HandleClick(target);
        }
    }
}
