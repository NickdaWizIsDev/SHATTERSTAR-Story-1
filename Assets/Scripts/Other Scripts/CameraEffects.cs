using UnityEngine;
using PrimeTween;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.2f)
    {
        Tween.ShakeCamera(Camera.main, magnitude, duration);
    }
}