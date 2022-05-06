using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraEffects : MonoBehaviour
{

    private CinemachineVirtualCamera cam;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float seconds)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        Invoke("ResetCameraShake", seconds);
    }

    public void ResetCameraShake()
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
}
