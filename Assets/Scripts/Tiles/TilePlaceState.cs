using UnityEngine;

public class TilePlaceState : TileBaseState
{
    public override void EnterState(Tile tile)
    {
        tile.parent.name = "Tile (Placeable)";
    }
     
    public override void UpdateState(Tile tile)
    {
        return;
    }
     
    public override void OnMouseEnter(Tile tile)
    {
        tile.selection.SetActive(true);
    }

    public override void OnMouseExit(Tile tile)
    {
        tile.selection.SetActive(false);
    }

    public override void HandleLeftClick(Tile tile)
    {
        Debug.Log("Tile in placeState clicked");
        tile.selection.SetActive(false);
        tile.player.RemoveHeadgear(tile);
    }

    public override void HandleRightClick(Tile tile)
    {
        return;
    }
}
