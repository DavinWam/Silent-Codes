using UnityEngine;

public class IPHalfInvert : MonoBehaviour
{
    Texture2D texture;

    void Start()
    {
        Camera.main.orthographicSize = Screen.height / 2f;
        texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
        transform.localScale = new Vector3(texture.width, texture.height, 1);

        byte[] pixels = texture.GetRawTextureData();

        for (int row = 0; row < texture.height/2 - 1; ++row)
        {
            int rowOffset = texture.width * row * 4;

            for (int col = 0; col < texture.width; ++col)
            {
                int pixelIndex = rowOffset + col * 4;

                pixels[pixelIndex] = (byte)(byte.MaxValue - pixels[pixelIndex]);
                pixels[pixelIndex + 1] = (byte)(byte.MaxValue - pixels[pixelIndex + 1]);
                pixels[pixelIndex + 2] = (byte)(byte.MaxValue - pixels[pixelIndex + 2]);
            }
        }

        texture.LoadRawTextureData(pixels);
        texture.Apply();
    }
}
