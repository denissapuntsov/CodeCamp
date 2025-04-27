using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour, IPointerClickHandler
{
    [Header("Debug")] [SerializeField] private GameObject debugCube;
    
    [Header("Level Position")] 
    public Tile activeTile;
    
    [Header("Item Slots")]
    [SerializeField] private GameObject clothes;
    
    [Header("Transforms")]
    [SerializeField] private Transform headGearParent;

    private Popup _popup;
    
    Collider[] _hitColliders = new Collider[18];
    public List<Tile> _hitTiles;

    private void Start()
    {
        _popup = GetComponentInChildren<Popup>();
        _hitTiles = new List<Tile>();
    }

    public void PutOn(GameObject item)
    {
        if (clothes) return;
        
        // turn off trigger for tiles and collider for pointers
        item.GetComponent<BoxCollider>().enabled = false;
        if (item.GetComponentInChildren<MeshCollider>()) item.GetComponentInChildren<MeshCollider>().enabled = false;
        
        item.GetComponent<InteractableFramework>().currentTile.Clear();
        item.GetComponent<InteractableFramework>().currentTile = null;
        
        item.transform.SetParent(headGearParent, false);
        item.GetComponent<Rigidbody>().isKinematic = true;
        ResetTransform(item);
        clothes = item;

        AstarPath.active.Scan();
    }

    private void ResetTransform(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandleLeftClick();
        }
    }

    private void OnMouseOver()
    {
        if (!clothes) return;
        _popup.Appear();
    }

    private void OnMouseExit()
    {
        if(!clothes) return;
        _popup.Disappear();
    }

    private void HandleLeftClick()
    {
        if (!clothes) return;
        Debug.Log("Pressed left click");
        _hitTiles.Clear();

        //Collider[] hitColliders = new Collider[8];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, 5f, _hitColliders, LayerMask.GetMask("Tile"));

        for (int i = 0; i < collidersHit; i++)
        {
            Tile tile = _hitColliders[i].GetComponent<Tile>();
            if (!tile) continue;
            
            if (!tile.hasPlayer && tile.currentItem == null)
            {
                Debug.Log("Tile");
                _hitTiles.Add(tile);
            }
        }

        // debug
        foreach (Tile tile in _hitTiles)
        {
            Instantiate(debugCube, tile.transform, false);
        }
    }
}
