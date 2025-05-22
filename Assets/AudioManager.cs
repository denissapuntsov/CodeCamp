using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource footstepSource, bookSource;

    [SerializeField] public List<AudioClip> stoneFootsteps, woodFootsteps;
}
