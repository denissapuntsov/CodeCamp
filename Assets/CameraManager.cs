using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private List<CinemachineCamera> _cameraList;

    public void SwitchCamera(CinemachineCamera camera)
    {
        if (!_cameraList.Contains(camera)) return;
        foreach (CinemachineCamera cam in _cameraList) cam.Prioritize();
    }
}
