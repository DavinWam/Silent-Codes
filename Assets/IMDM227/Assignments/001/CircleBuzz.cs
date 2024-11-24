using IMDM227;
using UnityEngine;

public class CircleBuzz : MonoBehaviour
{
    public float width = .5f;
    public float edge = .7f;

    public float fractionalRadius = .8f;

    public Color32 color32 = Color.white;

    public int numSegments = 100;

    public string Name = "Circle";

    MeshLine4 meshLine;

    void Start()
    {
        meshLine = new MeshLine4(gameObject, name, color32, width, edge);
    }

    private void Update()
    {
        float radius = fractionalRadius * Screen.height / 2f;

        float x0 = radius;
        float y0 = 0;

        System.Random r = new System.Random();

        for (int segmentNum = 1; segmentNum < numSegments; ++segmentNum)
        {
            float theta = segmentNum * 2f * Mathf.PI / numSegments;

            float x1 = radius * Mathf.Cos(theta);
            float y1 = radius * Mathf.Sin(theta);

            meshLine.Color32 = new Color32((byte)r.Next(0x100), (byte)r.Next(0x100), (byte)r.Next(0x100), 0xFF);
            meshLine.Line(x0 + r.Next(-20, 20), y0 + r.Next(-20, 20), x1 + r.Next(-20, 20), y1 + r.Next(-20, 20));

            x0 = x1;
            y0 = y1;
        }

        meshLine.Line(x0, y0, radius, 0);

        meshLine.Display();
    }
}
