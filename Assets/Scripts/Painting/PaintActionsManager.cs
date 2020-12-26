using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Painting;
using UnityEngine;
using UnityEngine.UI;

public class PaintActionsManager : MonoBehaviour
{
    private bool isFilling;
    private bool isErasing;
    private bool isLines;
    
    private GameObject eraseButton;
    private GameObject fillButton;
    private GameObject lineButton;
    
    private LinesManager linesManager;
    [SerializeField] private HSVPicker.ColorPicker colorPicker;
    // Start is called before the first frame update
    void Start()
    {
        isErasing = false;
        isFilling = false;
        isLines = false;
        fillButton = GameObject.Find("FillButton");
        eraseButton = GameObject.Find("EraserButton");
        lineButton = GameObject.Find("CreateButton");
        linesManager = GetComponent<LinesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleFill()
    {
        isErasing = false;
        isLines = false;
        isFilling = !isFilling;
        ShowToggle(fillButton, eraseButton, lineButton);
    }

    public void ToggleErase()
    {
        isFilling = false;
        isLines = false;
        isErasing = !isErasing;
        ShowToggle(eraseButton, fillButton, lineButton);
    }

    public void ToggleLines()
    {
        isFilling = false;
        isErasing = false;
        if (!isLines)
        {
            linesManager.EnableLinesMode();
            isLines = true;
        }
        else
        {
            linesManager.DisableLinesMode();
            isLines = false;
        }
        ShowToggle(lineButton, fillButton, eraseButton);
    }

    private void ShowToggle(GameObject toToggle, params GameObject[] toDeactivate)
    {
        if (toToggle)
        {
            toToggle.transform.Find("BackPlate").gameObject
                .SetActive(!toToggle.transform.Find("BackPlate").gameObject.activeInHierarchy);
        }
        foreach (var o in toDeactivate)
        {
            o.transform.Find("BackPlate").gameObject.SetActive(false);
        }
    }

    public void DeactivateAll()
    {
        isErasing = false;
        isFilling = false;
        isLines = false;
        linesManager.DisableLinesMode();
        ShowToggle(null, eraseButton, fillButton, lineButton);
    }

    public void HandleClick(GameObject target)
    {
        if(isErasing)
        {
            Erase(target);
        }
        else if (isFilling)
        {
            Fill(target);
        }
    }

    private void Erase(GameObject target)
    {
        Destroy(target);
    }

    private void Fill(GameObject target)
    {
        if (target.TryGetComponent(out Line line))
        {
            line.SetMaterial(colorPicker.CurrentColor);
        }
        else
        {
            target.GetComponent<MeshRenderer>().material.SetColor("_Color", colorPicker.CurrentColor);
        }
    }

    public void ToggleManipulators(bool isManipulable)
    {
        var manipulableObjects = GameObject.FindGameObjectsWithTag("Manipulable");
        foreach (var o in manipulableObjects)
        {
            o.GetComponent<ObjectManipulator>().enabled = isManipulable;
        }
    }
}
