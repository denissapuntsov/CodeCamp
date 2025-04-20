using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuParent;
    public GameObject activeMenuGroup;

    private void Start()
    {
        Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseActiveMenu();
        }
    }
    
    private void Clear()
    {
        foreach (Transform child in menuParent.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void SetMenu(GameObject menuGroup)
    {
        if (activeMenuGroup) return;
        
        activeMenuGroup = menuGroup;
        Clear();
        activeMenuGroup.SetActive(true);
    }
    
    public void CloseActiveMenu()
    {
        if (!activeMenuGroup)
        {
            Debug.LogWarning("No menu group active.");
            return;
        }

        activeMenuGroup.SetActive(false);
        activeMenuGroup = null;
    }
}
