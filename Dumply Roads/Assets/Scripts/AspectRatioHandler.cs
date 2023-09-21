using UnityEngine;

public class AspectRatioHandler : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f; // Set your target aspect ratio here
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateAspectRatio();
    }


    private void UpdateAspectRatio()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        float wantedAspectRatio = targetAspectRatio;

        float scaleHeight = currentAspectRatio / wantedAspectRatio;

        Rect cameraRect = new Rect(0, 0, 1, 1);

        if (scaleHeight < 1.0f)
        {
            cameraRect.height = scaleHeight;
            cameraRect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            cameraRect.width = scaleWidth;
            cameraRect.x = (1.0f - scaleWidth) / 2.0f;
        }

        mainCamera.rect = cameraRect;
    }
}
