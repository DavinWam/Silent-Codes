using UnityEngine;

public class TexCam : MonoBehaviour
{
    public Material texMat;

    WebCamTexture cam;

    void Start()
    {
        cam = new WebCamTexture(WebCamTexture.devices[0].name, 640, 360, 30);
        int w = cam.width;
        Debug.Log($"{w}");
        texMat.mainTexture = cam;
        cam.Play();
        w = cam.width;
        Debug.Log($"{w}");
    }
}
