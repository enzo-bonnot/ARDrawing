﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Painting
{
    public class LinesManager : MonoBehaviour
    {
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private HSVPicker.ColorPicker colorPicker;
        private GameObject currentLine;
        private PointerHandler pointerHandler;

        public void Start()
        {
            pointerHandler = gameObject.AddComponent<PointerHandler>();

            // Make this a global input handler, otherwise this object will only receive events when it has input focus
            CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        }

        private void CreateNewLine(MixedRealityPointerEventData arg0)
        {
            RaycastHit hit;
            var mask = LayerMask.GetMask("Menu");
            if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, mask))
            {
                var line = Instantiate(linePrefab, arg0.Pointer.Position + Camera.main.transform.forward*0.2f, Quaternion.identity);
                line.GetComponent<Line>().SetMaterial(colorPicker.CurrentColor);
                currentLine = line;
            }
        }

        public void EnableLinesMode()
        {
            pointerHandler.OnPointerDown.AddListener(CreateNewLine);
            pointerHandler.OnPointerUp.AddListener(OnUp);
            pointerHandler.OnPointerDragged.AddListener(OnDrag);
        }

        public void DisableLinesMode()
        {
            pointerHandler.OnPointerDown.RemoveListener(CreateNewLine);
            pointerHandler.OnPointerUp.RemoveListener(OnUp);
            pointerHandler.OnPointerDragged.RemoveListener(OnDrag);
        }

        private void OnUp(MixedRealityPointerEventData arg0)
        {
            if (currentLine)
            {
                if (currentLine.GetComponent<Line>().NbPoints <= 2)
                {
                    Destroy(currentLine);    
                }
                else //Line is valid, activate interactions
                {
                    var meshCollider = currentLine.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    meshCollider.sharedMesh = currentLine.GetComponent<Line>().GetMesh();

                    currentLine.AddComponent<NearInteractionGrabbable>();
                    currentLine.AddComponent<ConstraintManager>();
                    var manipulator = currentLine.AddComponent<ObjectManipulator>();
                    manipulator.enabled = false;
                    var inputManager = GetComponentInParent<GlobalActionsManager>();
                    var interactable = currentLine.GetComponent<Interactable>();
                    var temp = currentLine;
                    interactable.OnClick.AddListener(() => inputManager.HandleInput(temp));
                }
            }
            currentLine = null;
        }

        private void OnDrag(MixedRealityPointerEventData evt)
        {
            if (currentLine)
            {
                currentLine.GetComponent<Line>().AddPoint(evt.Pointer.Position + Camera.main.transform.forward*0.2f);
            }
        }
    }
}
