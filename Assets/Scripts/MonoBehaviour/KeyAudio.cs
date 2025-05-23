using UnityEngine;

public class KeyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip clipSuccess, clipFail;
    
    public void PlayAudioSuccess()
    {
        GetComponent<AudioSource>().PlayOneShot(clipSuccess);
    }

    public void PlayAudioFail()
    {
        GetComponent<AudioSource>().PlayOneShot(clipFail);
    }
}
