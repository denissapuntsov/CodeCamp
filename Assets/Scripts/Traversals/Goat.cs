using System;
using UnityEngine;

public class Goat : Traversal
{
    [SerializeField] private Collider[] _hitColliders;
    [SerializeField] private GameObject model;
    [SerializeField] private Tile linkedTile;
    [SerializeField] private Transform _offsetTransform;

    private Vector3[] _offsets;

    private void Start()
    {
        _hitColliders = new Collider[1];

        linkedTile = ScanForAvailableTile();
    }

    private Tile ScanForAvailableTile()
    {
        Tile foundTile = null;
        int collidersHit = 0;
        
        collidersHit = Physics.OverlapBoxNonAlloc(
            center: _offsetTransform.position,
            halfExtents: new Vector3(1f, 5, 1f),
            results: _hitColliders,
            orientation: Quaternion.identity,
            mask: LayerMask.GetMask("TileTrigger"),
            queryTriggerInteraction: QueryTriggerInteraction.Collide);
        
        foreach (Collider trigger in _hitColliders)
        {
            Debug.Log(trigger.gameObject.name);
            foundTile = trigger.gameObject.GetComponent<Tile>();
            /*if (trigger.gameObject.GetComponent<Tile>())
            {
                Tile tile = trigger.gameObject.GetComponent<Tile>();
                if (tile.CurrentState == tile.WalkState) foundTile = tile;
            }*/
        }
        
        return foundTile;
    }

    public override void Use()
    {
        throw new NotImplementedException();
    }
}
