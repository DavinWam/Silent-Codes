using IMDM227;
using UnityEngine;
public class Square : MonoBehaviour
{
    void Start()
    {
        MeshLine4 meshLine = new MeshLine4(gameObject, "square", Color.white, .5f, .7f);
        meshLine.Line(-100, 100, 100, 100);
        meshLine.Line(100, 100, 100, -100);
        meshLine.Line(100, -100, -100, -100);
        meshLine.Line(-100, -100, -100, 100);
        meshLine.Display();
    }
}
