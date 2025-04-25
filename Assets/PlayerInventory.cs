using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Item Slots")]
    [SerializeField] private GameObject clothes;
    
    [Header("Transforms")]
    [SerializeField] private Transform headGearParent;
    
    public void PutOn(GameObject item)
    {
        if (clothes) return;
        item.transform.SetParent(headGearParent, false);
        item.GetComponent<Rigidbody>().isKinematic = true;
        ResetTransform(item);
        clothes = item;
    }

    private void ResetTransform(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }
}
