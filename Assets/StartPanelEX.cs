using UnityEngine;

public class StartPanelEX : MonoBehaviour
{
    public void FadeIntoNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
