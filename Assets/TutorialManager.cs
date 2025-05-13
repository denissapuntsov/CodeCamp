using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    
    [SerializeField] public string walk, approach, wear, change, remove, inspect;
    
    private List<String> _passedTutorials = new List<String>();

    private string _activeTutorial;
    
    public string ActiveTutorial
    {
        get => _activeTutorial;
        private set
        {
            if (_activeTutorial == value) return;
            SubtitleManager.instance.DisplaySubtitle(value);
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
        //ActiveTutorial = walk;
    }

    public void SetActiveTutorial(string tutorial)
    {
        string tutorialToActivate = String.Empty;
        
        // this is some toby fox level of bullshit, but I am not using reflections 
        switch (tutorial)
        {
            case "walk":
                tutorialToActivate = walk;
                break;
            case "approach":
                tutorialToActivate = approach;
                break;
            case "wear":
                tutorialToActivate = wear;
                break;
            case "change":
                tutorialToActivate = change;
                break;
            case "remove":
                tutorialToActivate = remove;
                break;
            case "inspect":
                tutorialToActivate = inspect;
                break;
        }
        
        ActiveTutorial = tutorialToActivate;
    }

    public void ClearActiveTutorial()
    {
        SubtitleManager.instance.ClearSubtitle();
        _activeTutorial = null;
    }
}

