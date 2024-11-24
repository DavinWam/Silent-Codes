using UnityEngine;

public class IPRotateCW90 : MonoBehaviour
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
                int pixelIndex = (texture.width * col * 4) + (texture.width - 1 - row ) * 4;

                newPixels[newIndex] = (oldPixels[pixelIndex]);
                newPixels[newIndex + 1] = (oldPixels[pixelIndex + 1]);
                newPixels[newIndex + 2] = (oldPixels[pixelIndex + 2]);

         
            }
  
        }

        texture.LoadRawTextureData(newPixels);
        texture.Apply();
    }
}
