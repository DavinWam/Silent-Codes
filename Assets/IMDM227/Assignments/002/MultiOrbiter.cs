using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MultiOrbiter : MonoBehaviour
{
    public float xFrequency = 0.5f;
    public float yFrequency = 1.0f;
    public float radius = 3;
    public float startTime = 0;

    float xTheta;
    float yTheta;

    private void Start()
    {
        xTheta = addToTheta(0, xFrequency, startTime);
        yTheta = addToTheta(0, yFrequency, startTime);
    }
    void Update()
    {
        xTheta = addToTheta(xTheta, xFrequency, Time.deltaTime);
        yTheta = addToTheta(yTheta, yFrequency, Time.deltaTime);

        float x = radius * Mathf.Cos(xTheta);
        float y = radius * Mathf.Sin(yTheta);

        Vector3 position;

        position.x = x;
        position.y = y;
        position.z = 0;

        transform.position = position;
    }

    float addToTheta(float theta, float frequency, float time)
    {
        theta = theta + frequency * time * 2 * Mathf.PI;

        while (theta > 2 * Mathf.PI)
        {
            theta = theta - 2 * Mathf.PI;
        }

        return theta;
    }
}
