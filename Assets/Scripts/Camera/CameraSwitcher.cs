using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraSwitcher
{
    private static HashSet<CinemachineVirtualCameraBase> knownCameras = new();
    public static CinemachineVirtualCameraBase ActiveCamera = null;
    public static bool IsActiveCamera(CinemachineVirtualCameraBase cam)
    {
        return ActiveCamera == cam;
    }

    public static void Register(CinemachineVirtualCameraBase cam)
    {
        knownCameras.Add(cam);
    }

    public static void Unregister(CinemachineVirtualCameraBase cam)
    {
        knownCameras.Remove(cam);
    }

    public static void Switch(CinemachineVirtualCameraBase cam)
    {
        if (IsActiveCamera(cam))
            return;

        cam.Priority = 10;
        ActiveCamera = cam;

        foreach(var c in knownCameras)
        {
            if (c != cam)
                c.Priority = 0;
        }
    }
}