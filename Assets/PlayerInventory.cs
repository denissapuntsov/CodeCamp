using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerInventory : MonoBehaviour, IPointerClickHandler
{
    [Header("Level Position")] 
    public Tile activeTile;
    [SerializeField] private GameObject positioningCube;
    
    [Header("Item Slots")]
    [SerializeField] private GameObject clothes;
    
    [Header("Transforms")]
    [SerializeField] private Transform headGearParent;

    private Popup _popup;
    
    private readonly Collider[] _hitColliders = new Collider[18];
    private List<Tile> _hitTiles;
    private List<GameObject> _cubes;

    private void Start()
    {
        _popup = GetComponentInChildren<Popup>();
        _cubes = new List<GameObject>();
        _hitTiles = new List<Tile>();
    }

    public void PutOn(GameObject item)
    {
        if (clothes) return;
        
        // turn off trigger for tiles and collider for pointers
        item.GetComponent<BoxCollider>().enabled = false;
        if (item.GetComponentInChildren<Collider>()) item.GetComponentInChildren<Collider>().enabled = false;
        
        item.transform.SetParent(headGearParent, false);
        item.GetComponent<Rigidbody>().isKinematic = true;
        ResetTransform(item);
        clothes = item;
    }

    public void PlaceAt(Transform placementCubeTransform)
    {
        if (!clothes) return;

        clothes.transform.SetParent(p: null);
        clothes.GetComponent<Rigidbody>().isKinematic = false;
        ResetTransform(clothes);
        
        if (clothes.GetComponentInChildren<Collider>()) clothes.GetComponentInChildren<Collider>().enabled = true;
        clothes.GetComponent<BoxCollider>().enabled = true;
        
        clothes.transform.position = placementCubeTransform.position;

        clothes = null;
        
        foreach (GameObject cube in _cubes) Destroy(cube);
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
        _hitTiles.Clear();

        //Collider[] hitColliders = new Collider[8];
        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, 5f, _hitColliders, LayerMask.GetMask("Tile"));

        for (int i = 0; i < collidersHit; i++)
        {
            Tile tile = _hitColliders[i].GetComponent<Tile>();
            if (!tile) continue;
            
            if (!tile.hasPlayer && !tile.isOccupied)
            {
                _hitTiles.Add(tile);
            }
        }

        _cubes.Clear();
        // debug
        foreach (Tile tile in _hitTiles)
        {
            GameObject newCube = Instantiate(positioningCube, tile.transform, false);
            _cubes.Add(newCube);
        }
    }
}
