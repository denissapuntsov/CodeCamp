using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public bool hasPlayer;
    public bool isOccupied;

    private Material _material;
    private PlayerInventory _player;

    private void Start()
    {
        _material = GetComponentInParent<MeshRenderer>().material;
        _player = FindAnyObjectByType<PlayerInventory>();
    }

    private void Update()
    {
        _material.color = hasPlayer ? Color.red : Color.white;
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
            isOccupied = true;
        }
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
