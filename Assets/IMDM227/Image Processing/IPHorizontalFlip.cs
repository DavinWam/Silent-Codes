using UnityEngine;

public class IPHorizontalFlip : MonoBehaviour
{
    Texture2D texture;

    void Start()
    {
        Camera.main.orthographicSize = Screen.height / 2f;
        texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
        transform.localScale = new Vector3(texture.width, texture.height, 1);

        byte[] oldPixels = texture.GetRawTextureData();
        byte[] newPixels = new byte[oldPixels.Length];

        for (int row = 0; row < texture.height; ++row)
        {
            int rowOffset = texture.width * row * 4;

            for (int col = 0; col < texture.width; ++col)
            {
                int newIndex = rowOffset + col * 4;
                int endIndex = rowOffset + (texture.width - col - 1) * 4;//index of the alpha channel of the pixel scrolling from right to left
                
                newPixels[newIndex] = (byte)(oldPixels[endIndex]);
                newPixels[newIndex + 1] = (byte)(oldPixels[endIndex + 1]);
                newPixels[newIndex + 2] = (byte)(oldPixels[endIndex + 2]);
            }
        }

        texture.LoadRawTextureData(newPixels);
        texture.Apply();
    }
}
