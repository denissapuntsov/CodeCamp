using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    [SerializeField] private UnityEvent onMenuOpened, onMenuClosed;
    
    public void Close()
    {
        onMenuClosed?.Invoke();
        gameObject.SetActive(false);
    }
}
