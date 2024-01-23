using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegister : MonoBehaviour
{
    private void OnEnable()
    {
        CameraSwitcher.Register(GetComponent<Cinemachine.CinemachineVirtualCameraBase>());
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(GetComponent<Cinemachine.CinemachineVirtualCameraBase>());
    }
}
