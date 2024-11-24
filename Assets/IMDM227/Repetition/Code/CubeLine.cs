using UnityEngine;

public class CubeLine : MonoBehaviour
{
    public GameObject blank;
    public int numClones;
    public Vector3 pos;
    public Vector3 deltaPos;

    void Start()
    {
        for (int i = 0; i < numClones; ++i)
        {
            Instantiate(blank, pos, Quaternion.identity);

            pos = pos + deltaPos;
        }
    }
}
