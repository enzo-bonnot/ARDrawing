using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesManager : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;

    public void AddLine()
    {
        Instantiate(linePrefab);
    }
}
