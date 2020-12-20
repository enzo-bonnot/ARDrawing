using System.Collections;
using System.Collections.Generic;
using Painting;
using UnityEngine;

public class PaintActionsManager : MonoBehaviour
{
    private bool isFilling;
    private bool isErasing;
    [SerializeField] private HSVPicker.ColorPicker colorPicker;
    // Start is called before the first frame update
    void Start()
    {
        isErasing = false;
        isFilling = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleFill()
    {
        isErasing = false;
        isFilling = !isFilling;
    }

    public void ToggleErase()
    {
        isFilling = false;
        isErasing = !isErasing;
    }

    public void DeactivateAll()
    {
        isErasing = false;
        isFilling = false;
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
        Debug.Log("In eraser");
        Debug.Log(target);
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
            // target.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", colorPicker.CurrentColor);
        }
    }
}
