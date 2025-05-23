using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class Hay : Traversal
{
    private Collider[] _hitColliders;
    private List<Tile> _hitTiles;
    private Player _player;
    private AudioSource _source;
    private string _mode;
    private Vector3 _entryPointPlayerPosition;
    [SerializeField] private Tile linkedTile;
    [SerializeField] private List<AudioClip> audioClips;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _hitColliders = new Collider[2];
        _hitTiles = new List<Tile>();
        _source = GetComponent<AudioSource>();
        PlaySound();
    }

    public override void Use()
    {
        PlaySound();
        _entryPointPlayerPosition = _player.transform.position;
        
        _player.transform.position = playerTransform.position;
        
        _mode = _entryPointPlayerPosition.y < transform.position.y ? "up" : "down";
        
        linkedTile = ScanForAvailableTile(_mode);
        
        _player.GetComponent<AIPath>().canMove = false;
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
                centerY = 2.5f;
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
        _player.GetComponent<AIPath>().canMove = true;
        PlaySound();
    }

    IEnumerator SendPlayerBack()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _player.CurrentState = _player.IdleState;
        _player.transform.position = _entryPointPlayerPosition;
        _player.activeTile.SetNewPath();
        _player.GetComponent<AIPath>().canMove = true;
        PlaySound();
    }

    private void PlaySound()
    {
        _source.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Count)]);
    }
}
