using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
	[SerializeField] private Object cubePrefab;

	[SerializeField] private GameObject spherePrefab;

	[SerializeField] private GameObject capsulePrefab;

	[SerializeField] private GameObject cylinderPrefab;

	private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCube()
    {
	    Vector3 pos = camera.transform.position;
	    pos.z += 0.2f;
	    Instantiate(cubePrefab, pos, new Quaternion());
    }

    public void AddSphere()
    {
	    Vector3 pos = camera.transform.position;
	    pos.z += 0.2f;
	    Instantiate(spherePrefab, pos, new Quaternion());
    }

    public void AddCapsule()
    {
	    Vector3 pos = camera.transform.position;
	    pos.z += 0.2f;
	    Instantiate(capsulePrefab, pos, new Quaternion());
    }

    public void AddCylinder()
    {
	    Vector3 pos = camera.transform.position;
	    pos.z += 0.2f;
	    Instantiate(cylinderPrefab, pos, new Quaternion());
    }

}
