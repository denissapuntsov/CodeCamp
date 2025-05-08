using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpSequencer : MonoBehaviour
{
    [SerializeField] private List<Animator> lights = new List<Animator>();
    [SerializeField] private float stepDelay = 0.5f;
    private static readonly int IsLitUp = Animator.StringToHash("isLitUp");

    public void LightUp()
    {
        StartCoroutine(LightUpSequence());
    }

    private IEnumerator LightUpSequence()
    {
        foreach (Animator animator in lights)
        {
            animator.SetBool(IsLitUp, true);
            yield return new WaitForSeconds(stepDelay);
        }
    }
}
