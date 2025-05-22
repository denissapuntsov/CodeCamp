using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuParent;
    public Menu activeMenuGroup, pauseMenuGroup;
    public Menu captionMenuGroup;
    List<GameObject> _foundObjects = new List<GameObject>();

    public static MenuManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Clear();
        captionMenuGroup.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeMenuGroup != null)
            {
                CloseActiveMenu();
                return;
            }
            OpenMenu(pauseMenuGroup);
        }
    }
    
    private void Clear()
    {
        foreach (Menu child in menuParent.transform.GetComponentsInChildren<Menu>())
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OpenMenu(Menu menuGroup)
    {
        _foundObjects = GameObject.FindGameObjectsWithTag("3DUI").ToList();
        foreach (GameObject foundObject in _foundObjects)
        {
            foundObject.GetComponentInChildren<MeshRenderer>().enabled = false; 
            foundObject.GetComponentInChildren<Light>().enabled = false;
        }
        
        activeMenuGroup = menuGroup;
        Clear();
        activeMenuGroup.Open();
    }
    
    public void CloseActiveMenu()
    {
        foreach (GameObject foundObject in _foundObjects)
        {
            foundObject.GetComponentInChildren<MeshRenderer>().enabled = true; 
            foundObject.GetComponentInChildren<Light>().enabled = true;
        }
        if (!activeMenuGroup)
        {
            return;
        }

        if (activeMenuGroup.isSubmenu)
        {
            activeMenuGroup.Close();
            return;
        }
        activeMenuGroup.Close();
        activeMenuGroup = null;
    }
}
