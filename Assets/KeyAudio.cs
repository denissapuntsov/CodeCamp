using UnityEngine;

public class KeyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip clipSuccess;
    
    public void PlayAudioSuccess()
    {
        GetComponent<AudioSource>().PlayOneShot(clipSuccess);
    }
}
