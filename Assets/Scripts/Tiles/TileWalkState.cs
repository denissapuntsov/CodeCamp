using System.Collections;
using System.Security.Principal;
using UnityEngine;

public class TileWalkState : TileBaseState
 {
     public override void EnterState(Tile tile)
     {
         tile.parent.name = "Tile (Empty)";
         tile.SetColliderSize("Empty");
     }
 
     public override void UpdateState(Tile tile)
     {
         return;
     }
 
     public override void OnMouseEnter(Tile tile)
     {
         if (tile.player.CurrentState != tile.player.IdleState) return;
         tile.selection.SetActive(true);
     }

     public override void OnMouseExit(Tile tile)
     {
         tile.selection.SetActive(false);
     }

     public override void HandleLeftClick(Tile tile)
     {
         if (tile.player.CurrentState != tile.player.IdleState) return;
         tile.selection.SetActive(false);
         tile.player.distanceThreshold = 2.5f;
         tile.player.aiPath.destination = tile.transform.position;
         tile.player.CurrentState = tile.player.WalkState;
         //tile.aiDestinationSetter.target = tile.transform;
     }

     public override void HandleRightClick(Tile tile)
     {
         return;
     }
 }
