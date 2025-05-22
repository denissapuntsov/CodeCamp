using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadNextLevel()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;

        if (activeScene < SceneManager.sceneCountInBuildSettings - 1) SceneManager.LoadScene(activeScene + 1);
        else SceneManager.LoadScene(0);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void FreezeTime(bool state)
    {
        Time.timeScale = state ? 0f : 1f;
    }
}
