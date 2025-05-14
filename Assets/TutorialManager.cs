using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    
    private List<String> _passedTutorials = new List<String>();

    private string _activeTutorial;
    
    public string ActiveTutorial
    {
        get => _activeTutorial;
        private set
        {
            if (_activeTutorial == value) return;
            CaptionManager.Instance.DisplaySubtitle(value);
            _activeTutorial = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetActiveTutorial("walk");
    }

    public void SetActiveTutorial(string tutorial)
    {
        if (_passedTutorials.Contains(tutorial)) return;
        ActiveTutorial = CaptionManager.Instance.GetCaptionTextByID(tutorial);
        _passedTutorials.Add(tutorial);
    }

    public void SetActiveTutorial(string tutorial, float timeToDisappear)
    {
        if (_passedTutorials.Contains(tutorial)) return;
        _passedTutorials.Add(tutorial);
        _activeTutorial = CaptionManager.Instance.GetCaptionTextByID(tutorial);
        CaptionManager.Instance.DisplaySubtitle(_activeTutorial, timeToDisappear);
    }

    private void ClearActiveTutorial()
    {
        CaptionManager.Instance.ClearSubtitle();
        _activeTutorial = null;
    }

    public void ClearTutorialByID(string id)
    {
        if (ActiveTutorial == CaptionManager.Instance.GetCaptionTextByID(id))
        {
            ClearActiveTutorial();
        }
    }
}

