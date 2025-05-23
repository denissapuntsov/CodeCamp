using System.Collections.Generic;
using UnityEngine;

public class PortalAudio : MonoBehaviour
{
    [SerializeField] private List<AudioSource> portalSources = new List<AudioSource>();

    public void ActivateSources()
    {
        foreach (AudioSource source in portalSources) source.Stop();
        foreach (AudioSource source in portalSources) gameObject.SetActive(true);
    }
    
    public void PlayScrapeSound()
    {
        portalSources[0].Play();
    }

    public void PlayPortalSound()
    {
        portalSources[1].Play();
    }

    public void EnableHum()
    {
        portalSources[2].enabled = true;
    }
}
