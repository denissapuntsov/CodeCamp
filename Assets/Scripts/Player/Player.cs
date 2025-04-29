using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IPointerClickHandler
{
    [Header("Level Position")] 
    public Tile activeTile;
    [SerializeField] private GameObject positioningCube;

    [Header("Item Slots")]
    public GameObject headgear;
    
    [Header("Transforms")]
    [SerializeField] private Transform headGearParent;

    private Popup _popup;
    private Collider[] _hitColliders = new Collider[18];
    private List<Tile> _hitTiles;
    
    // FSM
    private PlayerBaseState _currentState;
    
    public PlayerIdleState IdleState = new PlayerIdleState();
    /*public PlayerWalkState walkState = new PlayerWalkState();
    public PlayerPlaceState placeState = new PlayerPlaceState();*/

    private void Start()
    {
        _popup = GetComponentInChildren<Popup>();
        _hitTiles = new List<Tile>();

        _currentState = IdleState;
        _currentState.EnterState(this);
    }

    public void PutOnHeadgear(GameObject item)
    {
        // turn off trigger for tiles and collider for pointers
        if (item.GetComponentsInChildren<Collider>() != null)
        {
            foreach (Collider c in item.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
        }
        
        item.transform.SetParent(headGearParent, false);
        item.GetComponent<Rigidbody>().isKinematic = true;
        ResetTransform(item);
        headgear = item;
    }

    public void RemoveHeadgear(Tile tileTarget)
    {
        if (!headgear) return;

        headgear.transform.SetParent(p: null);
        headgear.GetComponent<Rigidbody>().isKinematic = false;
        ResetTransform(headgear);
        
        if (headgear.GetComponentsInChildren<Collider>() != null)
        {
            foreach (Collider c in headgear.GetComponentsInChildren<Collider>()) c.enabled = true;
        }
        
        headgear.transform.position = tileTarget.transform.position;
        headgear.transform.SetParent(p: tileTarget.parent.transform);
        headgear = null;
        
        foreach (Tile tile in _hitTiles) tile.SwitchState(tile.WalkState);
        tileTarget.SwitchState(tileTarget.HoldState);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("pressed left click on player");
            HandleLeftClick();
        }
    }

    private void OnMouseOver()
    {
        if (!headgear) return;
        _popup.Appear();
    }

    private void OnMouseExit()
    {
        if(!headgear) return;
        _popup.Disappear();
    }

    private void HandleLeftClick()
    {
        if (!headgear) return;
        _hitTiles.Clear();

        //Collider[] hitColliders = new Collider[8];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, 5f, _hitColliders, LayerMask.GetMask("Tile"));

        for (int i = 0; i < collidersHit; i++)
        {
            Tile tile = _hitColliders[i].GetComponent<Tile>();
            if (!tile) continue;
            
            if (!tile.hasPlayer && !tile.currentInteractable)
            {
                _hitTiles.Add(tile);
            }
        }
        
        foreach (Tile tile in _hitTiles)
        {
            tile.SwitchState(tile.PlaceState);
        }
    }
    
    private void ResetTransform(GameObject obj)
    { 
        obj.transform.localPosition = Vector3.zero; 
        obj.transform.localRotation = Quaternion.identity;
    }

    public void SwitchState(PlayerBaseState newState)
    {
        _currentState = newState;
        newState.EnterState(this);
    }
}
