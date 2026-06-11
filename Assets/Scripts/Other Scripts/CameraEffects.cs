using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance { get; private set; }

    private Vector3 originalPos;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        
        originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.2f)
    {
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        var elapsed = 0.0f;
        while (elapsed < duration)
        {
            var x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            var y = originalPos.y + Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.unscaledDeltaTime;
            
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}