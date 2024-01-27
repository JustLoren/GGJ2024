using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegister : MonoBehaviour
{
    private void OnEnable()
    {
        var cam = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
        cam.Follow = Character.Instance.transform;
        cam.LookAt = Character.Instance.transform;
        CameraSwitcher.Register(cam);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(GetComponent<Cinemachine.CinemachineVirtualCameraBase>());
    }
}
