using System;
using UnityEngine;
public class SimpleCam : MonoBehaviour
{
    const int waitWidth = 16;

    public int width = 640;
    public int height = 360;

    Color32[] pixels;

    WebCamTexture cam;
    Texture2D tex;

    Action update = () => { };
    void OnMouseDown()
    {
        update = WaitingForCam;

        cam = new WebCamTexture(WebCamTexture.devices[0].name, width, height, 30);
        cam.Play();
        int w = cam.width;
        Debug.Log($"Width = {w}");
    }
    private void Update()
    {
        update();
    }
    void WaitingForCam()
    {
        if (cam.width > waitWidth)
        {
            width = cam.width;
            height = cam.height;
            pixels = new Color32[cam.width * cam.height];
            tex = new Texture2D(cam.width, cam.height, TextureFormat.RGBA32, false);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = tex;
            update = CamIsOn;
        }
        else
        {
            Debug.Log("Not Yet");
        }
    }
    void CamIsOn()
    {
        if (cam.didUpdateThisFrame)
        {
            cam.GetPixels32(pixels);
            //for (int i = 0; i < pixels.Length; ++i)
            //{
            //    pixels[i].r = (byte)(pixels[i].r * 4 % 256);
            //    pixels[i].g = (byte)(pixels[i].g * 4 % 256);
            //    pixels[i].b = (byte)(pixels[i].b * 4 % 256);
            //}
            tex.SetPixels32(pixels);
            tex.Apply();
        }
    }
}