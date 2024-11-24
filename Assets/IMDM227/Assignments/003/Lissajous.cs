using IMDM227;
using UnityEngine;

public class Lissajous : MonoBehaviour
{
    public float width = .5f;
    public float edge = .7f;

    public float fractionalRadius = .8f;

    public Color32 color32 = Color.white;

    public int numSegments = 100;
    public int numCoils = 10;

    public string Name = "Lissajous";

    public float xFrequency;
    public float yFrequency;
    public float phase;

    MeshLine4 meshLine;

    private void Start()
    {
        meshLine = new MeshLine4(gameObject, name, color32, width, edge);
    }
    void Update()
    {
        DrawMesh();
    }
    public void DrawMesh()
    {
        float radius = fractionalRadius * Screen.height / 2f;

        float xTheta = 0;
        float yTheta = 0;

        float x0 = radius * Mathf.Sin(xTheta + phase);
        float y0 = radius * Mathf.Sin(yTheta);

        for (int coilNum = 0; coilNum < numCoils; ++coilNum)
        {
            for (int segmentNum = 0; segmentNum < numSegments; ++segmentNum)
            {
                xTheta += xFrequency * 2f * Mathf.PI / numSegments;
                yTheta += yFrequency * 2f * Mathf.PI / numSegments;
                
                float x1 = radius * Mathf.Sin(xTheta + phase);
                float y1 = radius * Mathf.Sin(yTheta);

                meshLine.Line(x0, y0, x1, y1);

                x0 = x1;
                y0 = y1;
            }
        }

        meshLine.Display();
    }
}
