using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
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
    
    public void AddObject(GameObject gameObj)
    {
	    Vector3 pos = camera.transform.forward;
	    pos.z += 0.2f;
	    var obj= Instantiate(gameObj, pos, gameObj.transform.rotation);
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
