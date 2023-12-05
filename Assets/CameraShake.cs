using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [Header("Intensity of camera shake")]
    public float magnitude;

    private Vector3 originalPos;
    private Quaternion originalRot;

    private void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    private void Update()
    {   
        float x = Random.Range(-1f, 1f) * magnitude / 2;
        float y = Random.Range(-1f, 1f) * magnitude / 2;
        float z = Random.Range(-1f, 1f) * magnitude / 2;

        transform.localPosition = originalPos + new Vector3(x, y, z) * Time.timeScale;

        float rx = Random.Range(-1f, 1f) * magnitude;
        float ry = Random.Range(-1f, 1f) * magnitude;
        float rz = Random.Range(-1f, 1f) * magnitude;

        transform.localRotation = originalRot * Quaternion.Euler(rx, ry, rz);

        //StartCoroutine(Shake(1000f, 0.005f));
    }


    /*
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        Quaternion originalRot = transform.localRotation;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, z);

            float rx = Random.Range(-1f, 1f) * magnitude;
            float ry = Random.Range(-1f, 1f) * magnitude;
            float rz = Random.Range(-1f, 1f) * magnitude;

            transform.localRotation = originalRot * Quaternion.Euler(rx, ry, rz);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        transform.localRotation = originalRot;
    }

    */
}
