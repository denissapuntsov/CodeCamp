using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject parent;
    
    public bool hasPlayer;
    public GameObject currentInteractable;

    private Material _material;
    private PlayerInventory _player;

    private TileBaseState _currentState;
    private TileWalkState _walkState = new TileWalkState();
    private TilePlaceState _placeState = new TilePlaceState();
    private TileHoldState _holdState = new TileHoldState();

    private void Start()
    {
        _material = GetComponentInParent<MeshRenderer>().material;
        _player = FindAnyObjectByType<PlayerInventory>();
        
        currentInteractable = parent?.GetComponentInChildren<InteractableFramework>()?.gameObject;
        parent.name = currentInteractable != null ? $"Tile ({currentInteractable.name})" : "Tile (Empty)";

        _currentState = _walkState;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _material.color = Color.green;
        Debug.Log(name + " " + currentInteractable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = true;
            _player.activeTile = this;
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
