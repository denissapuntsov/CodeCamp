using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    [SerializeField] private UnityEvent onMenuClosed;

    public void CloseMenu()
    {
        onMenuClosed?.Invoke();
        gameObject.SetActive(false);
    }
}
