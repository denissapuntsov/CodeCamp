using System;using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuParent;
    public Menu activeMenuGroup, pauseMenuGroup;

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
            SetMenu(pauseMenuGroup);
        }
    }
    
    private void Clear()
    {
        foreach (Transform child in menuParent.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void SetMenu(Menu menuGroup)
    {
        menuParent.SetActive(true);
        
        //if (activeMenuGroup) return;
        
        activeMenuGroup = menuGroup;
        Clear();
        activeMenuGroup.gameObject.SetActive(true);
    }
    
    public void CloseActiveMenu()
    {
        if (!activeMenuGroup)
        {
            return;
        }

        activeMenuGroup.CloseMenu();
        activeMenuGroup = null;
    }
}
