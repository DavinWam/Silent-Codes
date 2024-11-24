using UnityEngine;

public class Breather : MonoBehaviour
{
    public float amount = .5f;
    public float rate = 1;

    public float theta = 0;

    const float twoPi = 2 * Mathf.PI;
    const float half = 0.5f;

    void Start()
    {
        rate = rate * twoPi;
    }

    void Update()
    {
        theta = theta + rate * Time.deltaTime;

        while (theta > twoPi)
        {
            theta = theta - twoPi;
        }

        transform.localScale = Vector3.one * (1 - half * (amount * Mathf.Sin(theta) + 1));
    }
}
