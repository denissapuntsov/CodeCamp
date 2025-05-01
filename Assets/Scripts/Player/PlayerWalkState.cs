using Pathfinding;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.gameObject.name = "Player (Walking)";
    }

    public override void UpdateState(Player player)
    {
        /*// if within a distance to the last node, disable rotation of pathfinding, transition to idle state
        // switch (tileState):
        //  case WalkState => 2.5
        //  case HoldState => 5.1
        /*if (Vector3.Distance(player.transform.position, player.aiPath.endOfPath) <= player.distanceThreshold)
        {
            Debug.Log(player.aiPath.endOfPath);
            Debug.Log("Reached threshold");
            player.aiPath.enableRotation = false;
            player.CurrentState = player.IdleState;
            return;
        }#1#

        player.seeker.StartPath(player.transform.position, )

        player.aiPath.enableRotation = true;*/

        if (player.aiPath.reachedEndOfPath)
        {
            Debug.Log("reached destination");
            player.CurrentState = player.IdleState;
        }
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
        return;
    }
}
