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
         if (tile.player.CurrentState != tile.player.IdleState && tile.player.CurrentState != tile.player.WalkState) return;
         tile.selection.SetActive(true);
     }

     public override void OnMouseExit(Tile tile)
     {
         tile.selection.SetActive(false);
     }

     public override void HandleLeftClick(Tile tile)
     {
         if (tile.player.CurrentState != tile.player.IdleState && tile.player.CurrentState != tile.player.WalkState) return;
         tile.selection.SetActive(false);

         tile.SetNewPath();
     }

     public override void HandleRightClick(Tile tile)
     {
         return;
     }
 }
