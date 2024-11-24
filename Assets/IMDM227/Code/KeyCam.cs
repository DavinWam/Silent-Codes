using System;
using UnityEngine;
public class KeyCam : MonoBehaviour
{
    public Color maxColor;
    public Color minColor;

    public Keys keys;

    const int waitingWidth = 16;
    Color32 white32 = new Color32(255, 255, 255, 255);
    Color32 black32 = new Color32(0, 0, 0, 0);

    public bool showKey = false;

    public int width = 640;
    public int height = 360;

    public event Action<int, int, bool[]> ReportPixels;

    Color32[] pixels;
    bool[] keyPixels;

    WebCamTexture cam;
    Texture2D tex;

    Action update;
    void Start()
    {
        update = WaitingForCam;
        Application.targetFrameRate = 100;

        cam = new WebCamTexture(WebCamTexture.devices[0].name, width, height, 30);
        cam.Play();
        keys.maxRed = (byte)(maxColor.r * 255 + .5f);
        keys.maxGrn = (byte)(maxColor.g * 255 + .5f);
        keys.maxBlu = (byte)(maxColor.b * 255 + .5f);
    }
    private void Update()
    {
        update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            showKey = !showKey;
        }
    }
    void WaitingForCam()
    {
        if (cam.width > waitingWidth)
        {
            width = cam.width;
            height = cam.height;
            pixels = new Color32[cam.width * cam.height];
            keyPixels = new bool[pixels.Length];
            cam.GetPixels32(pixels);

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
            keys.maxRed = (byte)(maxColor.r * 255 + .5f);
            keys.maxGrn = (byte)(maxColor.g * 255 + .5f);
            keys.maxBlu = (byte)(maxColor.b * 255 + .5f);

            keys.minRed = (byte)(minColor.r * 255 + .5f);
            keys.minGrn = (byte)(minColor.g * 255 + .5f);
            keys.minBlu = (byte)(minColor.b * 255 + .5f);

            cam.GetPixels32(pixels);
            MirrorPixels(width, height, pixels);

            KeyPixels(pixels, keyPixels, keys);

            ReportPixels(width, height, keyPixels);

            if (showKey)
            {
                CopyKey(keyPixels, pixels);
            }

            tex.SetPixels32(pixels);
            tex.Apply();
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

    void KeyPixels(Color32[] p, bool[] b, Keys keys)
    {
        int bIndex = -1;

        foreach (Color32 c in p)
        {
            if (c.r >= keys.minRed &&
                c.r <= keys.maxRed &&
                c.g >= keys.minGrn &&
                c.g <= keys.maxGrn &&
                c.b >= keys.minBlu &&
                c.b <= keys.maxBlu)
            {
                b[++bIndex] = true;
            }
            else
            {
                b[++bIndex] = false;
            }
        }
    }

    void CopyKey(bool[] b, Color32[] p)
    {
        int cIndex = -1;

        foreach(bool k in b)
        {
            p[++cIndex] = k ? white32 : black32;
        }
    }
}

[Serializable]
public class Keys
{
    public int minRed;
    public int maxRed;
    public int minGrn;
    public int maxGrn;
    public int minBlu;
    public int maxBlu;
}