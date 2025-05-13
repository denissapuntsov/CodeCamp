using System;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ProcessInteractable(InteractionData objectData)
    {
        Debug.Log(objectData.inspectionText);
    }
}
