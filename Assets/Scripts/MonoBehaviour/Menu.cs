using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    [SerializeField] private UnityEvent onMenuOpened, onMenuClosed;

    public void Open()
    {
        gameObject.SetActive(true);
        onMenuOpened?.Invoke();
    }
    
    public void Close()
    {
        onMenuClosed?.Invoke();
        gameObject.SetActive(false);
    }
}
