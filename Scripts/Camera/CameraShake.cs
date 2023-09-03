using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private CinemachineVirtualCamera cmVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cmBasicMultiChannelPerlin;

    private float moveTime;
    private float totalMoveTime;
    private float inicialIntensity;

    private void Awake()
    {
        Instance = this;
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cmBasicMultiChannelPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveTime > 0)
        {
            moveTime -= Time.deltaTime;
            cmBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(inicialIntensity, 0, 1 - (moveTime / totalMoveTime));
        }
    }

    public void Shake(float intensity, float frequency, float time)
    {
        cmBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        cmBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        inicialIntensity = intensity;
        totalMoveTime = time;
        moveTime = time;
    }
}
