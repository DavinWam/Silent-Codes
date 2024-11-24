using IMDM227;
using UnityEngine;

public class Spiral : MonoBehaviour
{
    public float width = .5f;
    public float edge = .7f;

    public float fractionalRadius = .8f;

    public Color32 color32 = Color.white;

    public int numSegments = 100;
    public int numCoils = 10;
    public float dRadius = 10;

    public string Name = "Spiral";

    void Start()
    {
        MeshLine4 meshLine = new MeshLine4(gameObject, name, color32, width, edge);

        float radius = fractionalRadius * Screen.height / 2f;

        dRadius = dRadius / numSegments;

        for (int coilNum = 0; coilNum < numCoils; ++coilNum)
        {
            float x0 = radius;
            float y0 = 0;

            for (int segmentNum = 1; segmentNum < numSegments; ++segmentNum)
            {
                float theta = segmentNum * 2f * Mathf.PI / numSegments;

                radius = radius - dRadius;

                float x1 = radius * Mathf.Cos(theta);
                float y1 = radius * Mathf.Sin(theta);

                meshLine.Line(x0, y0, x1, y1);

                x0 = x1;
                y0 = y1;
            }

            meshLine.Line(x0, y0, radius, 0);
        }

        meshLine.Display();
    }
}
