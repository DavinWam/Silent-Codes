using UnityEngine;

public class KeyTrack : MonoBehaviour
{
    public KeyCam keyCam;

    const float screenWidth = 16;
    const float screenHeight = 9;
    const float half = 0.5f;

    Vector3 localPosition;

    void Start()
    {
        keyCam.ReportPixels += ReportPixels;
        localPosition = transform.localPosition;
    }

    void ReportPixels(int width, int height, bool[] k)
    {
        if (width == 0) return;

        int keyCount = 0;
        int sumX = 0;
        int sumY = 0;

        for (int y = 0; y < height; ++y)
        {
            int offset = y * width;

            for (int x = 0; x < width; ++x)
            {
                if (k[offset + x])
                {
                    ++keyCount;
                    sumX += x;
                    sumY += y;
                }
            }
        }

        if (keyCount == 0) return;

        float avgX = (float)sumX / keyCount;
        float avgY = (float)sumY / keyCount;

        float myX = screenWidth * (avgX / (width - 1) - half);
        float myY = screenHeight * (avgY / (height - 1) - half);

        localPosition.x = myX;
        localPosition.y = myY;

        transform.localPosition = localPosition;
    }
}
