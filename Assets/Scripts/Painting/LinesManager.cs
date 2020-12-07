using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
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

            foreach (var line in lines.Where(line => line.GetComponent<Line>().NbPoints <= 2))
            {
                Destroy(line);
                lines.Remove(line);
            }
        }

        private void OnUp(MixedRealityPointerEventData arg0)
        {
            if (currentLine && currentLine.GetComponent<Line>().NbPoints <= 2)
            {
                lines.Remove(lines.Last());
                Destroy(currentLine);
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
