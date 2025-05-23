using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipPanel : MonoBehaviour
{
    [SerializeField] private bool _hasClickedOnce = false;
    [SerializeField] private Animator fadeOutAnimator;
    [SerializeField] private int targetIndex;
    private Animator _skipAnimator;

    private void Start()
    {
        _skipAnimator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Debug.Log("Click");
        
        if (_hasClickedOnce)
        {
            SceneManager.LoadScene(targetIndex);
            return;
        }
        _skipAnimator.Play("ShowWarning");
        StartCoroutine(DelayInputRead());
    }

    private IEnumerator DelayInputRead()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _hasClickedOnce = true;
        yield return new WaitForSecondsRealtime(3f);
        _skipAnimator.Play("HideWarning");
        _hasClickedOnce = false;
        Debug.Log("Has not clicked once");
    }
}
