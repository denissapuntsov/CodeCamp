using UnityEngine;

public class PortalEx : MonoBehaviour
{
    [SerializeField] private Animator playerDummyAnimator;

    public void RemovePlayer()
    {
        FindAnyObjectByType<Player>().gameObject.SetActive(false);
    }

    public void TogglePlayerWalk()
    {
        playerDummyAnimator.SetBool("isWalking", !playerDummyAnimator.GetBool("isWalking"));
    }
    
    public void TransitionToNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
