using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Painting
{
    public class LinesManager : MonoBehaviour
    {
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject mainMenuPrefab;
        private List<GameObject> lines;
        private GameObject currentLine;
        private PointerHandler pointerHandler;

        public void Start()
        {
            lines = new List<GameObject>();
            
            pointerHandler = gameObject.AddComponent<PointerHandler>();

            // Make this a global input handler, otherwise this object will only receive events when it has input focus
            CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(pointerHandler);
        }

        private void CreateNewLine(MixedRealityPointerEventData arg0)
        {
            var line = Instantiate(linePrefab);
            lines.Add(line);
            currentLine = line;
        }

        public void LinesButton()
        {
            pointerHandler.OnPointerDown.AddListener(CreateNewLine);
            pointerHandler.OnPointerUp.AddListener(OnUp);
            pointerHandler.OnPointerDragged.AddListener(OnDrag);
        }

        private void DisableLinesMode()
        {
            pointerHandler.OnPointerDown.RemoveListener(CreateNewLine);
            pointerHandler.OnPointerUp.RemoveListener(OnUp);
            pointerHandler.OnPointerDragged.RemoveListener(OnDrag);

            //Need a classic for to remove while iterating
            for(var i = 0 ; i < lines.Count ; i++)
            {
                if (lines[i].GetComponent<Line>().NbPoints > 2) continue;
                
                Destroy(lines[i]);
                lines.RemoveAt(i);
            }
        }

        private void OnUp(MixedRealityPointerEventData arg0)
        {
            if (currentLine)
            {
                if (currentLine.GetComponent<Line>().NbPoints <= 2)
                {
                    lines.Remove(lines.Last());
                    Destroy(currentLine);    
                }
                else //Line is valid, activate interactions
                {
                    var meshCollider = currentLine.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    meshCollider.sharedMesh = currentLine.GetComponent<Line>().GetMesh();

                    currentLine.AddComponent<NearInteractionGrabbable>();
                    currentLine.AddComponent<ConstraintManager>();
                    currentLine.AddComponent<ObjectManipulator>();
                }
            }
            currentLine = null;
        }

        private void OnDrag(MixedRealityPointerEventData evt)
        {
            if (currentLine)
            {
                currentLine.GetComponent<Line>().AddPoint(evt.Pointer.Position);
            }
        }

        public void Eraser()
        {
            DisableLinesMode();
            Debug.Log("WIP");
        }

        public void Return()
        {
            Instantiate(mainMenuPrefab);
            Destroy(gameObject);
        }
    }
}
