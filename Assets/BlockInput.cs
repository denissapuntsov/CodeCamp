using UnityEngine;

public class BlockInput : MonoBehaviour
{
    [SerializeField] GameObject inputBlock;

    public void Enable()
    {
        inputBlock?.SetActive(true);
    }

    public void Disable()
    {
        inputBlock?.SetActive(false);
    }
}
