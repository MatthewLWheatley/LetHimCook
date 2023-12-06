using UnityEngine;

public class ScaleBorder : MonoBehaviour
{
    [SerializeField] private float targetWidth = 1920f;
    [SerializeField] private float targetHeight = 1080f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        UpdateScale();
    }

    private void Update()
    {
        UpdateScale();
    }

    private void UpdateScale()
    {
        // Calculate the scaling factors
        float scaleFactorX = Screen.width / targetWidth;
        float scaleFactorY = Screen.height / targetHeight;

        // Determine the smaller scale factor to maintain aspect ratio
        float uniformScaleFactor = Mathf.Min(scaleFactorX, scaleFactorY)*2;

        // Apply the uniform scale factor
        transform.localScale = new Vector3(originalScale.x * uniformScaleFactor,
                                           originalScale.y * uniformScaleFactor,
                                           originalScale.z);
    }
}
