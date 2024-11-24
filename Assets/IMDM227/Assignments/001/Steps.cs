using IMDM227;
using UnityEngine;

public class Steps : MonoBehaviour
{
    public float lineWidth = .5f;
    public float edgeWidth = .7f;
    public Color32 color = Color.white;
    public string meshName = "Steps";
    public int numSteps = 5;
    void Start()
    {
        MeshLine4 meshLine = new MeshLine4(gameObject, meshName, Color.white, lineWidth, edgeWidth);

        for (int stepNum = 0; stepNum < numSteps; ++stepNum)
        {
            float xFrom = -Screen.width / 2f + stepNum * Screen.width / numSteps;
            float yFrom = -Screen.height / 2f + stepNum * Screen.height / numSteps;

            int nextStepNum = stepNum + 1;
            
            float xTo = -Screen.width / 2f + nextStepNum * Screen.width / numSteps;
            float yTo = -Screen.height / 2f + nextStepNum * Screen.height / numSteps;

            meshLine.Line(xFrom, yFrom, xTo, yFrom);
            meshLine.Line(xTo, yFrom, xTo, yTo);
        }

        meshLine.Display();
    }
}
