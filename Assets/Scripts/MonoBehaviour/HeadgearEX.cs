using UnityEngine;

public class HeadgearEx : MonoBehaviour
{
    [SerializeField] private GameObject onModel, offModel;
    
    public void SwitchModels(bool mode)
    {
        if (mode)
        {
            offModel.SetActive(false);
            onModel.SetActive(true);
            return;
        }
        offModel.SetActive(true);
        onModel.SetActive(false);

    }
}
