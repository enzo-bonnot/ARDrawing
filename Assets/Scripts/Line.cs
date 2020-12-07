using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using TreeEditor;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float minimumDistance = 0.02f;
    private LineRenderer line;
    private List<Vector3> points = new List<Vector3>();
    private Camera camera;
    private GameObject rayCamera; 
    void Start()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.material = material;
        line.startWidth = line.endWidth = 0.05f;

        rayCamera = GameObject.Find("UIRaycastCamera");
        
        camera = Camera.main;
        
        PointerHandler pointerHandler = gameObject.AddComponent<PointerHandler>();
        pointerHandler.OnPointerDragged.AddListener(OnDrag);
        // pointerHandler.OnPointerClicked.AddListener(OnAirTap);
        // Make this a global input handler, otherwise this object will only receive events when it has input focus
        CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);
    }

    private void Update()
    {
        line.SetPositions(points.ToArray());
    }
    
    private void OnDrag(MixedRealityPointerEventData evt)
    {
        Vector3 pos = evt.Pointer.Position;//camera.transform.position + camera.transform.forward;
        //pos.z += .2f;

        if (points.Any() && Vector3.Distance(pos, points.Last()) < minimumDistance) return;
        
        points.Add(pos);
        line.positionCount = points.Count;
    }
    
    private void OnAirTap(MixedRealityPointerEventData evt)
    {
        Vector3 pos = camera.transform.position + camera.transform.forward;
        pos.z += .2f;
        points.Add(pos);
        line.positionCount = points.Count;
    }
}