using UnityEngine;

public class PortalEx : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator;

    public void RemovePlayer()
    {
        FindAnyObjectByType<Player>().gameObject.SetActive(false);
    }

    public void FadeOut()
    {
        panelAnimator.Play("FadeOutPanel");
    }
    
    public void TransitionToNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
