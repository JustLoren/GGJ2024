using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThief : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase vCam;

    private void OnTriggerStay(Collider other)
    {
        if (Character.Instance.IsGrounded)
            CameraSwitcher.Switch(vCam);
    }
}
