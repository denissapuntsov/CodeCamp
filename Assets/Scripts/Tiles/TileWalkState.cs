using UnityEngine;

public class TileWalkState : TileBaseState
 {
     public override void EnterState(Tile tile)
     {
         tile.parent.name = "Tile (Empty)";
     }
 
     public override void UpdateState(Tile tile)
     {
         return;
     }
 
     public override void OnMouseEnter(Tile tile)
     {
         return;
     }

     public override void OnMouseExit(Tile tile)
     {
         return;
     }

     public override void HandleLeftClick(Tile tile)
     {
         if (tile.player.CurrentState != tile.player.IdleState) return;
         tile.aiDestinationSetter.target = tile.transform;
     }

     public override void HandleRightClick(Tile tile)
     {
         return;
     }
 }
