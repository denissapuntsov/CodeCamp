using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    
    private List<String> _passedTutorials = new List<String>();

    private string _activeTutorial;
    
    public string ActiveTutorial
    {
        get => _activeTutorial;
        private set
        {
            if (_activeTutorial == value) return;
            CaptionManager.instance.DisplaySubtitle(value);
            _activeTutorial = value;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetActiveTutorial("walk");
    }

    public void SetActiveTutorial(string tutorial)
    {
        ActiveTutorial = CaptionManager.instance.GetCaptionTextByID(tutorial);
    }

    public void ClearActiveTutorial()
    {
        CaptionManager.instance.ClearSubtitle();
        _activeTutorial = null;
    }
}

