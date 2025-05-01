using Pathfinding;
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
         
         Path p = tile.player.seeker.StartPath(tile.player.transform.position, tile.transform.position, p =>
         {
             Debug.Log("Calculated path");
             tile.player.aiPath.SetPath(p);
             tile.player.CurrentState = tile.player.WalkState;
             tile.selection.SetActive(false);
         });
         
         /*tile.player.distanceThreshold = 2.5f;
         tile.player.aiPath.destination = tile.transform.position;
         tile.player.CurrentState = tile.player.WalkState;#1#*/
     }

     public override void HandleRightClick(Tile tile)
     {
         return;
     }
 }
