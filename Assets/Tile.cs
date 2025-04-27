using System;
using UnityEngine;
public class Tile : MonoBehaviour
{
    [SerializeField] private bool hasPlayer;
    [SerializeField] private GameObject currentItem;

    private void OnTriggerEnter(Collider other)
    {
        hasPlayer = other.gameObject.CompareTag("Player Parent");
        if (!other.GetComponent<InteractableFramework>()) return;
        currentItem = other.gameObject;
    }

    public void Clear()
    {
        currentItem = null;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentItem)
        {
            currentItem = null;
        }
    }
}
