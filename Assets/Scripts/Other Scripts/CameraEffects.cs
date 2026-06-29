using UnityEngine;
using Unity.Cinemachine;
using PrimeTween;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance { get; private set; }

    [Header("Cinemachine References")]
    [SerializeField] private CinemachineCamera vCam;
    private CinemachineBasicMultiChannelPerlin camNoise;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        if (vCam != null)
        {
            camNoise = vCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.2f)
    {
        if (camNoise == null)
        {
            Debug.LogWarning("CameraEffects: No Noise component found on the Virtual Camera!");
            return;
        }

        // Stop any current camera shake tweens so rapid hits don't conflict
        Tween.StopAll(camNoise);

        // Spike the amplitude to the requested magnitude instantly
        camNoise.AmplitudeGain = magnitude;

        // Tween the amplitude back to 0 smoothly over the given duration
        Tween.Custom(this, startValue: magnitude, endValue: 0f, duration: duration, 
            onValueChange: (target, val) => target.camNoise.AmplitudeGain = val);
    }
}