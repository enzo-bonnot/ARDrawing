using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
	[SerializeField] private GameObject cubePrefab;

	[SerializeField] private GameObject spherePrefab;

	[SerializeField] private GameObject capsulePrefab;

	[SerializeField] private GameObject cylinderPrefab;

	private GlobalActionsManager globalActionsManager;

	private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        globalActionsManager = GetComponentInParent<GlobalActionsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCube()
    {
	    Vector3 pos = camera.transform.forward;
	    pos.z += 0.2f;
	    var obj = Instantiate(cubePrefab, pos, new Quaternion());
	    AddHandlers(obj);
    }

    public void AddSphere()
    {
	    Vector3 pos = camera.transform.forward;
	    pos.z += 0.2f;
	    var obj = Instantiate(spherePrefab, pos, new Quaternion());
	    AddHandlers(obj);
    }

    public void AddCapsule()
    {
	    Vector3 pos = camera.transform.forward;
	    pos.z += 0.2f;
	    var obj= Instantiate(capsulePrefab, pos, new Quaternion());
	    AddHandlers(obj);
    }

    public void AddCylinder()
    {
	    Vector3 pos = camera.transform.forward;
	    pos.z += 0.2f;
	    var obj= Instantiate(cylinderPrefab, pos, new Quaternion());
	    AddHandlers(obj);
    }

    private void AddHandlers(GameObject target)
    {
	    target.AddComponent<NearInteractionGrabbable>();
	    target.AddComponent<ConstraintManager>();
	    target.AddComponent<ObjectManipulator>();
	    var interactable = target.GetComponent<Interactable>();
	    var temp = target;
	    interactable.OnClick.AddListener(() => globalActionsManager.HandleInput(temp));
    }

}
