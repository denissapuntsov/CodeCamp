using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuParent;
    public Menu activeMenuGroup, pauseMenuGroup;
    List<GameObject> _foundObjects = new List<GameObject>();

    public static MenuManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Clear();
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
        foreach (Transform child in menuParent.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OpenMenu(Menu menuGroup)
    {
        //menuParent.SetActive(true);
        _foundObjects = GameObject.FindGameObjectsWithTag("3DUI").ToList();
        foreach (GameObject foundObject in _foundObjects) foundObject.GetComponent<MeshRenderer>().enabled = false;
        //if (activeMenuGroup) return;
        
        activeMenuGroup = menuGroup;
        Clear();
        activeMenuGroup.gameObject.SetActive(true);
    }
    
    public void CloseActiveMenu()
    {
        foreach (GameObject foundObject in _foundObjects) foundObject.GetComponent<MeshRenderer>().enabled = true;
        if (!activeMenuGroup)
        {
            return;
        }

        activeMenuGroup.Close();
        activeMenuGroup = null;
    }
}
