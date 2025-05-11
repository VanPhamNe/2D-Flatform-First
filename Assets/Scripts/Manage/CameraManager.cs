using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [Header("Camera shake")]
    [SerializeField] private Vector2 shakeVelocity;
    private CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            impulseSource = GetComponent<CinemachineImpulseSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShakeCamera(float shakeDirection)
    {
        impulseSource.m_DefaultVelocity = new Vector2(shakeVelocity.x*shakeDirection, shakeVelocity.y);
        impulseSource.GenerateImpulse();
    }
}
