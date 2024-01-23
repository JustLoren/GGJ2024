using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThief : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase vCam;

    private void OnTriggerEnter(Collider other)
    {
        CameraSwitcher.Switch(vCam);
    }
}
