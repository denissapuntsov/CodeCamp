using UnityEngine;

public class CogSolvedAudio : MonoBehaviour
{
    [SerializeField] private AudioSource cogSource, steamSource;
    
    public void Begin()
    {
        cogSource.Play();
    }

    public void End()
    {
        cogSource.Stop();
        steamSource.Play();
    }
}
