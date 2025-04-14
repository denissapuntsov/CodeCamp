using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private GameObject _hitObject;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    _hitObject = hit.collider.transform.gameObject;
                    _hitObject.GetComponent<Interactable>().OnLeftClick();
                }
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    _hitObject = hit.collider.transform.gameObject;
                    _hitObject.GetComponent<Interactable>().OnRightClick();
                }
            }
        }
    }
}
