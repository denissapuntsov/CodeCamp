using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hay : Traversal
{
    private Collider[] _hitColliders;
    private List<Tile> _hitTiles;
    private Player _player;
    private string _mode;
    private Vector3 _entryPointPlayerPosition;
    [SerializeField] private Tile linkedTile;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _hitColliders = new Collider[2];
        _hitTiles = new List<Tile>();
    }

    public override void Use()
    {
        _entryPointPlayerPosition = _player.transform.position;
        
        _player.transform.position = playerTransform.position;
        
        _mode = _entryPointPlayerPosition.y < transform.position.y ? "up" : "down";
        
        linkedTile = ScanForAvailableTile(_mode);
        if (linkedTile == null) 
        {
            StartCoroutine(SendPlayerBack());
            return; 
        }

        StartCoroutine(SendPlayerToTile());
    }

    private Tile ScanForAvailableTile(string mode)
    {
        float centerY = 0f;
        
        switch (mode)
        {
            case "up":
                centerY = 5f;
                break;
            case "down":
                centerY = -0.5f;
                break;
        }
        
        Tile foundTile = null;
        int collidersHit = Physics.OverlapBoxNonAlloc(
            center: transform.position + new Vector3(0, centerY, 0), 
            halfExtents: new Vector3(1f, 1f, 7f), 
            results: _hitColliders, 
            orientation: Quaternion.identity, 
            mask: LayerMask.GetMask("TileTrigger"),
            queryTriggerInteraction: QueryTriggerInteraction.Collide);

        for (int i = 0; i < collidersHit; i++)
        {
            Tile tile = _hitColliders[i].GetComponentInChildren<Tile>();
            if (tile.CurrentState == tile.WalkState) foundTile = tile;continue;
        }
        
        return foundTile;
    }

    IEnumerator SendPlayerToTile()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _player.CurrentState = _player.IdleState;
        _player.transform.position = linkedTile.transform.position;
        linkedTile.SetNewPath();
    }

    IEnumerator SendPlayerBack()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _player.CurrentState = _player.IdleState;
        _player.transform.position = _entryPointPlayerPosition;
        _player.activeTile.SetNewPath();
    }
}
