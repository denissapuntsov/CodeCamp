using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject clothes;

    public void PutOn(GameObject item)
    {
        if (clothes) return;
        item.transform.SetParent(transform);
        clothes = item;
    }
}
