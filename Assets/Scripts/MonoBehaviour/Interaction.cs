using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [SerializeField] private UnityEvent onRightClick, onLeftClick;

    public void OnRightClick()
    {
        onRightClick?.Invoke();
        Debug.Log($"{gameObject.name} is right-clicked");
    }

    public void OnLeftClick()
    {
        onLeftClick?.Invoke();
        Debug.Log($"{gameObject.name} is left-clicked");
    }
}
