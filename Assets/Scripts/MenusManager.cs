using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private GameObject objectsMenu;

    [SerializeField] private GameObject paintMenu;
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        objectsMenu.SetActive(false);
        paintMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        objectsMenu.SetActive(false);
        paintMenu.SetActive(false);
    }

    public void ActivateObjectsMenu()
    {
        mainMenu.SetActive(false);
        objectsMenu.SetActive(true);
    }

    public void ActivatePaintMenu()
    {
        mainMenu.SetActive(false);
        paintMenu.SetActive(true);
    }
}
