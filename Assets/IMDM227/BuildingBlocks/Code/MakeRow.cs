using Random = System.Random;
using UnityEngine;

public class MakeRow : MonoBehaviour
{
    public bool getNarrower = false;
    public bool isBreathing = false;
    public float maxBreathSeconds = 10; // Maximum seconds per breath
    public float minBreathSeconds = 2;  // Minimum seconds per breath

    public int wallWidth = 7;
    public int wallHeight = 4;
    public Transform redModel;
    public Transform ylwModel;

    private Random random = new Random();

    void Start()
    {
        BuildWall();
    }

    private void BuildWall()
    {
        // Calculate offsets to center the wall in the view
        float xOffset = wallWidth % 2 == 0 ? -wallWidth / 2 + 0.5f : -wallWidth / 2;
        float yOffset = wallHeight % 2 == 0 ? -wallHeight / 2 + 0.5f : -wallHeight / 2;

        for (int y = 0; y < wallHeight; y++)
        {
            // If 'getNarrower' is checked, the width decreases by 2 with each row going up.
            int currentWidth = getNarrower ? Mathf.Max(wallWidth - y, 0) : wallWidth;


            // Center the row by adjusting startXPos based on the currentWidth
            float startXPos = xOffset + (wallWidth - currentWidth) / 2f;


            for (int x = 0; x < currentWidth; x++)
            {
                // Alternate the color of the cubes
                Transform modelPrefab = (x + y) % 2 == 0 ? redModel : ylwModel;
                Vector3 position = new Vector3(startXPos + x, yOffset + y, 0); // Use yOffset to center vertically

                // Instantiate the cube at the calculated position
                Transform cube = Instantiate(modelPrefab, position, Quaternion.identity);

                // If 'isBreathing' is checked, set a random breathing rate
                if (isBreathing)
                {
                    Breather breather = cube.GetComponent<Breather>();
                    breather.rate = GetRandomBreathingRate();
                    
                }
                else
                {
                    // Ensure cubes are not breathing if 'isBreathing' is unchecked
                    cube.GetComponent<Breather>().enabled = false; // Disable the Breather component
                }
            }
        }
    }

    // Convert the breathing duration to rate and ensure a flat distribution
    private float GetRandomBreathingRate()
    {
        // Generating a random number between minBreathSeconds and maxBreathSeconds
        double breathDuration = minBreathSeconds + (random.NextDouble() * (maxBreathSeconds - minBreathSeconds));

        // Convert seconds per breath to breaths per second.
        // Since the distribution of breathDuration is flat/uniform (every value is as likely as any other),
        // the conversion to rate (which is the reciprocal of duration) also results in a flat/uniform distribution
        // for the rate between the minimum and maximum breathing rate.
        float rate = 1f / (float)breathDuration;

        return rate;
    }

}
