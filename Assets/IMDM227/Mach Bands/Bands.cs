using UnityEngine;

public class Bands : MonoBehaviour
{
    public GameObject band;

    int numBands = 2;
    int gap = 10;

    float screenWidth;
    float screenHeight;

    int loColor = 0;
    int hiColor = 255;

    GameObject[] bands;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        Camera.main.orthographicSize = screenHeight / 2f;

        bands = new GameObject[0];

        BuildBands();
    }
    void BuildBands()
    {
        foreach (var band in bands)
        {
            Destroy(band);
        }

        bands = new GameObject[numBands];

        float bandWidth = screenWidth / numBands;

        for (int bandNum = 0; bandNum < numBands; ++bandNum)
        {
            GameObject newBand = Instantiate(band);

            bands[bandNum] = newBand;

            Vector3 scale = new Vector3(bandWidth - gap, screenHeight, 1);
            newBand.transform.localScale = scale;

            float xPosition = -screenWidth / 2; // Start at the left edge of the screen.
            xPosition += bandWidth / 2; // Move the center half a band's width to the right.
            xPosition += bandNum * bandWidth; // And move it again to the right of existing bands.

            Vector3 position = new Vector3(xPosition, 0, 0);
            newBand.transform.localPosition = position;

            int color = (int)(0.5f + loColor + (hiColor - loColor) * bandNum / (numBands - 1f));

            float intensity = color / 255f;

            newBand.GetComponent<Renderer>().material.color = new Color(intensity, intensity, intensity);
        }
    }

    public void SetNumBands(float numBands)
    {
        this.numBands = (int)numBands;

        BuildBands();
    }

    public void SetGap(float gap)
    {
        this.gap = (int)gap;

        BuildBands();
    }

    public void SetLoColor(float loColor)
    {
        this.loColor = (int)loColor;

        BuildBands();
    }

    public void SetHiColor(float hiColor)
    {
        this.hiColor = (int)hiColor;

        BuildBands();
    }
}
