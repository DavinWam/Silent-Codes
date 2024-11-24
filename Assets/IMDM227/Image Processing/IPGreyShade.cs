using Unity.VisualScripting;
using UnityEngine;

public class IPGreyShade : MonoBehaviour
{
    Texture2D texture;

    void Start()
    {
        Camera.main.orthographicSize = Screen.height / 2f;
        texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
        transform.localScale = new Vector3(texture.width, texture.height, 1);

        byte[] pixels = texture.GetRawTextureData();

        for (int row = 0; row < texture.height; ++row)
        {
            int rowOffset = texture.width * row * 4;

            for (int col = 0; col < texture.width; ++col)
            {
                int pixelIndex = rowOffset + col * 4;
                byte average = (byte)((pixels[pixelIndex] + pixels[pixelIndex+1] + pixels[pixelIndex+2])/3f);
                
                pixels[pixelIndex] = average;
                pixels[pixelIndex + 1] = average;
                pixels[pixelIndex + 2] = average;
            }
        }

        texture.LoadRawTextureData(pixels);
        texture.Apply();
    }
}
