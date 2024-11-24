using UnityEngine;

public class Orbiter2 : MonoBehaviour
{
    public float frequency = 0.5f;
    public float radius = 3;
    public float startTime = 0;

    float theta;
    private void Start()
    {
        theta = addToTheta(0, frequency, startTime);
    }
    void Update()
    {
        theta = addToTheta(theta, frequency, Time.deltaTime);

        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

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
