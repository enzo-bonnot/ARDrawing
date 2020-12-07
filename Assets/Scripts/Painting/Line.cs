using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace Painting
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private float minimumDistance = 0.02f;
        private LineRenderer line;
        private List<Vector3> points = new List<Vector3>();
        public int NbPoints => line.positionCount;

        void Start()
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.material = material;
            line.startWidth = line.endWidth = 0.05f;
        }

        public void Update()
        {
            line.SetPositions(points.ToArray());
        }

        public void AddPoint(Vector3 pos)
        {
            if (points.Any() && Vector3.Distance(pos, points.Last()) < minimumDistance) return;
        
            points.Add(pos);
            line.positionCount = points.Count;
        }
    }
}