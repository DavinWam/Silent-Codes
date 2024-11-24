using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MakeGradient : MonoBehaviour
{
    public int lo = 0;
    public int hi = 255;
    public float noiseLevel = 0;
    public bool postNoise = false;

    System.Random rand = new System.Random();

    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;

        BuildGradient();
    }

    void BuildGradient()
    {
        int h = Screen.height;
        int w = Screen.width;

        Camera.main.orthographicSize = h / 2f;

        transform.localScale = new Vector3(w, h, 1);

        Texture2D texture = new Texture2D(w, h, TextureFormat.RGBA32, false);

        byte[] pixels = new byte[w * h * 4];

        float loLevel = lo;
        float hiLevel = hi;

        for (int row = 0; row < h; ++row)
        {
            int rowOffset = w * row * 4;

            for (int col = 0; col < w; ++col)
            {
                int colOffset = col * 4;

                float noise = (float)((rand.NextDouble() - .5) * noiseLevel * 2);
                float signal = loLevel + (hiLevel - loLevel) * (col / (w - 1f));

                if (postNoise)
                {
                    signal = (int)(signal + 0.5f);
                }

                byte signalPlusNoise = (byte)(noise + signal + 0.5f);

                pixels[rowOffset + colOffset + 0] = signalPlusNoise;
                pixels[rowOffset + colOffset + 1] = signalPlusNoise;
                pixels[rowOffset + colOffset + 2] = signalPlusNoise;
                pixels[rowOffset + colOffset + 3] = 0xFF;
            }
        }

        texture.LoadRawTextureData(pixels);
        texture.Apply();
        material.mainTexture = texture;
    }

    public void SetLo(float lo)
    {
        this.lo = (int)lo;
    }
    public void SetHi(float hi)
    {
        this.hi = (int)hi;
    }
    public void SetNoise(float noise)
    {
        noiseLevel = noise;
    }
    public void DoBuildGradient()
    {
        BuildGradient();
    }
    public void SetPostNoise(bool postNoise)
    {
        this.postNoise = postNoise;
    }
}
