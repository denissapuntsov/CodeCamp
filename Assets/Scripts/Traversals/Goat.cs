using System;
using UnityEngine;

public class Goat : Traversal
{
    private Collider[] _hitColliders;
    [SerializeField] private GameObject model;
    [SerializeField] private Tile linkedTile;

    private Vector3[] _offsets;

    private void Start()
    {
        _offsets = new Vector3[]
        {
            new Vector3(5, 0, 0),
            new Vector3(0, 0, 5),
            new Vector3(-5, 0, 0),
            new Vector3(0, 0, -5)
        };
    }

    public override void Use()
    {
        linkedTile = ScanForAvailableTile();
    }

    private Tile ScanForAvailableTile()
    {
        Tile foundTile = null;
        int collidersHit = 0;
        foreach (Vector3 offset in _offsets)
        {
            collidersHit += Physics.OverlapBoxNonAlloc(
                center: transform.position + offset,
                halfExtents: Vector3.one,
                results: _hitColliders,
                orientation: Quaternion.identity,
                mask: LayerMask.GetMask("TileTrigger"),
                queryTriggerInteraction: QueryTriggerInteraction.Collide);
        }
        
        for (int i = collidersHit; i > 0; i--)
        {
            Tile tile = _hitColliders[i].GetComponentInChildren<Tile>();
            if (tile.CurrentState == tile.WalkState) foundTile = tile;
        }
        
        return foundTile;
    }
}
