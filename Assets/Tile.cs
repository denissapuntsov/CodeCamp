using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public bool hasPlayer;
    public GameObject currentItem;

    private Material _material;
    private PlayerInventory _player;

    private void Start()
    {
        _material = GetComponentInParent<MeshRenderer>().material;
        _player = FindAnyObjectByType<PlayerInventory>();
    }

    private void Update()
    {
        _material.color = hasPlayer || currentItem ? Color.red : Color.white;
        transform.parent.gameObject.layer = currentItem ? LayerMask.NameToLayer("Unwalkable") : LayerMask.NameToLayer("Tile");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = true;
            _player.activeTile = this;
        }

        if (other.GetComponent<InteractableFramework>())
        {
            currentItem = other.gameObject;
            other.gameObject.GetComponent<InteractableFramework>().currentTile = this;
        }
    }

    public void Clear()
    {
        currentItem = null;
        AstarPath.active.Scan();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasPlayer = false;
            _player.activeTile = null;
        }
    }
}
