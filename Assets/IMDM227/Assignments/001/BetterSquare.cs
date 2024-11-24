using IMDM227;
using UnityEngine;

public class BetterSquareSquare : MonoBehaviour
{
    public float lineWidth = .5f;
    public float edgeWidth = .7f;
    public Color32 color = Color.white;
    public string meshName = "Square";
    public float fractionalSize = .5f;
    void Start()
    {
        MeshLine4 meshLine = new MeshLine4(gameObject, meshName, Color.white, lineWidth, edgeWidth);

        float halfSize = fractionalSize * Screen.height / 2f;

        float left = 0 - halfSize;
        float right = 0 + halfSize;
        float bottom = 0 - halfSize;
        float top = 0 + halfSize;

        meshLine.Line(left, top, right, top);
        meshLine.Line(right, top, right, bottom);
        meshLine.Line(right, bottom, left, bottom);
        meshLine.Line(left, bottom, left, top);

        meshLine.Display();
    }
}
