using UnityEngine;

public class PlayerPlaceState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.name = $"Player (Placing {player.headgear.name})";
    }

    public override void UpdateState(Player player)
    {
        HandleKeyboardInput(player);
    }

    public override void OnMouseOver(Player player)
    {
        return;
    }

    public override void OnMouseExit(Player player)
    {
        return;
    }

    public override void HandleLeftClick(Player player)
    {
        return;
    }

    public override void HandleKeyboardInput(Player player)
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { player.ClearPlacementTiles();}
    }
}
