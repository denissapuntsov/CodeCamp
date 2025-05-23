using UnityEngine;

public class CogAudio : MonoBehaviour
{
    [SerializeField] private AudioSource steamSource, cogSource;
    
    public void Stop()
    {
        cogSource.Stop();
        steamSource.Play();
    }
}
