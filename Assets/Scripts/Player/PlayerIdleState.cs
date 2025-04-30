using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.gameObject.name = player.headgear ? $"Player (Idle) ({player.headgear.name})" : "Player (Idle)";
    }

    public override void UpdateState(Player player)
    {
        return;
    }

    public override void OnMouseOver(Player player)
    {
        if (player.headgear) player.ShowPopup("Remove");
    }

    public override void OnMouseExit(Player player)
    {
        if (player.headgear) player.popup.Disappear();
    }

    public override void HandleLeftClick(Player player)
    {
        if (player.headgear)
        {
            player.popup.Disappear();
            player.CalculatePlacementTiles();
            player.CurrentState = player.PlaceState;
        }
    }

    public override void HandleKeyboardInput(Player player)
    {
        return;
    }
}
