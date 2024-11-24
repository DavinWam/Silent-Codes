using System;
using UnityEngine;
public class SimpleCamZ : MonoBehaviour
{
    const int waitWidth = 16;

    public int width = 640;
    public int height = 360;

    Color32[] pixels;


    WebCamTexture cam;
    Texture2D tex;

    Action update;
    //public GameObject butterfly;

    //GameObject clone;
    public Transform butterfly;
    Vector3 pos;
    int xAverage = 0;
    int yAverage = 0;
    int numX = 0;
    int numY = 0;
    int index = 0;


    void Start()
    {
        update = WaitingForCam;
        // clone = Instantiate(butterfly, pos, Quaternion.identity);
        cam = new WebCamTexture(WebCamTexture.devices[0].name, width, height, 30);
        cam.Play();
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
    }
    void CamIsOn()
    {
        if (cam.didUpdateThisFrame)
        {
            cam.GetPixels32(pixels);
            MirrorPixels(width, height, pixels);
            tex.SetPixels32(pixels);
            tex.Apply();



            numX = 0;
            xAverage = 0;
            numY = 0;
            yAverage = 0;
            index = 0;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    //Debug.Log(pixels[index].r);
                    if (pixels[index].r > 140 && pixels[index].r < 180 &&
                        pixels[index].g > 160 && pixels[index].g < 240 &&
                        pixels[index].b > 110 && pixels[index].b < 210)
                    {
                        //Debug.Log("true");
                        //Debug.Log(x);
                        //Debug.Log(y);
                        xAverage = xAverage + x;
                        yAverage = yAverage + y;
                        numX++;
                        numY++;
                    }

                    index = index + 1;
                }
            }


            if (numX != 0)
            {
                xAverage = xAverage / numX;
            }

            if (numY != 0)
            {
                yAverage = yAverage / numY;
            }

            /*pos.x = xAverage;
            pos.y = yAverage;
            pos.z = butterfly.localPosition.z;*/

            //Debug.log(pos);                     
            //pos = new Vector3(xAverage, yAverage, butterfly.localPosition.z);
            //Debug.Log(pos);
            //clone.localPosition = pos;



            butterfly.position = new Vector3((int)(0.01 * xAverage), (int)(0.01 * yAverage), butterfly.position.z);
            //butterfly.position = new Vector3(0,0,0);
            Debug.Log(butterfly.position);
        }
    }

    void MirrorPixels(int width, int height, Color32[] pixels)
    {
        for (int y = 0; y < height; ++y)
        {
            int offsetLft = y * width;
            int offsetRgt = offsetLft + width - 1;

            for (int x = 0; x < width / 2; ++x)
            {
                Color32 temp = pixels[offsetLft + x];
                pixels[offsetLft + x] = pixels[offsetRgt - x];
                pixels[offsetRgt - x] = temp;
            }
        }
    }
}