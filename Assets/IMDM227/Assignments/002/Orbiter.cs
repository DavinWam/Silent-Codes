using UnityEngine;

public class Orbiter : MonoBehaviour
{
    public float frequency = 0.5f;
    public float radius = 3;
    public float startTime = 0;

    float theta;

    private void Start()
    {
        theta = frequency * startTime * 2 * Mathf.PI;

        while (theta > 2 * Mathf.PI)
        {
            theta = theta - 2 * Mathf.PI;
        }
    }
    void Update()
    {
        theta = theta + frequency * Time.deltaTime * 2 * Mathf.PI;

        while (theta > 2 * Mathf.PI)
        {
            theta = theta - 2 * Mathf.PI;
        }

        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

        Vector3 position;

        position.x = x;
        position.y = y;
        position.z = 0;

        transform.position = position;
    }
}
