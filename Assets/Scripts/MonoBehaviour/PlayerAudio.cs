using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    #region Audio

    [Header("Audio")] 
    [SerializeField] private AudioSource footstepSource, bookSource, hatSource;
    [SerializeField] private List<AudioClip> stoneFootsteps, woodFootsteps, bookClips, hatOnClips, hatOffClips;
    private List<AudioClip> _activeFootstepClips = new List<AudioClip>();
    
    #endregion
    
    public void PlayFootstep()
    {
        switch (GetComponentInParent<Player>().activeTile.material)
        {
            case TileMaterial.Stone:
                _activeFootstepClips = stoneFootsteps;
                break;
            case TileMaterial.Wood:
                _activeFootstepClips = woodFootsteps;
                break;
        }
        
        footstepSource.clip = _activeFootstepClips[Random.Range(0, _activeFootstepClips.Count)];
        footstepSource.Play();
    }

    public void PlayBook()
    {
        bookSource.clip = bookClips[Random.Range(0, bookClips.Count)];
        bookSource.Play();
    }


    public void PlayPutOn()
    {
        hatSource.clip = hatOnClips[Random.Range(0, hatOnClips.Count)];
        hatSource.Play();
    }

    public void PlayTakeOff()
    {
        hatSource.clip = hatOffClips[Random.Range(0, hatOffClips.Count)];
        hatSource.Play();
    }
    
}
