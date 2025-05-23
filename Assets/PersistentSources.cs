using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSources : MonoBehaviour
{
    [SerializeField] private List<int> sceneIndexList;
    public static PersistentSources Instance { get; private set; }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneIndexList.Contains(scene.buildIndex)) return;
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
