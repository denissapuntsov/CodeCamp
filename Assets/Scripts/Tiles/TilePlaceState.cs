using UnityEngine;

public class TilePlaceState : TileBaseState
{
    public override void EnterState(Tile tile)
    {
        tile.parent.name = "Tile (Placeable)";
    }
     
    public override void UpdateState(Tile tile)
    {
        
    }
     
    public override void OnMouseEnter(Tile tile)
    {
        tile.selection.SetActive(true);
        tile.cross.SetActive(true);
    }

    public override void OnMouseExit(Tile tile)
    {
        tile.selection.SetActive(false);
        tile.cross.SetActive(false);
    }

    public override void HandleLeftClick(Tile tile)
    {
        tile.selection.SetActive(false);
        tile.cross.SetActive(false);
        tile.player.RemoveHeadgear(tile);
    }

    public override void HandleRightClick(Tile tile)
    {
        
    }
}
