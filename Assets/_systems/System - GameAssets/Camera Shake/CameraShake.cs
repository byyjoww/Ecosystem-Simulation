using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeScreen(duration, magnitude));
    }

    private IEnumerator ShakeScreen(float duration, float magnitude)
    {
        Vector2 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
   }
}
