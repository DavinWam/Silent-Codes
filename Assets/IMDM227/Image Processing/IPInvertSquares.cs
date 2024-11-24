using UnityEngine;

public class IPInvertSquares : MonoBehaviour
{
    Texture2D texture;
    byte[] pixels;
    void Start()
    {
        Camera.main.orthographicSize = Screen.height / 2f;
        texture = GetComponent<Renderer>().material.mainTexture as Texture2D;
        transform.localScale = new Vector3(texture.width, texture.height, 1);

        pixels = texture.GetRawTextureData();

        DrawCheckerboard(4);

        texture.LoadRawTextureData(pixels);
        texture.Apply();
    }
    //Uses drawSquare to grid an image of size divisor
    //starting from the bottom left
    public void DrawCheckerboard(int divisor)
    {
        for (int y = 0; y < divisor; y++)
        {
            //starts at 0 if y is even and 1 if it is odd
            for (int x = (y % 2); x < divisor; x += 2)
            {
                drawSquare(x, y, divisor);
            }
        }
    }
    //treats image as a square grid of length divisor, (1,1) is the bottom left corner, (divisor,divisor) is the top right
    //Replaces the pixels in a block of width/divisor by height of divisor
    public void drawSquare(int startX, int startY, int divisor)
    {
        // Calculate the actual starting pixel positions based on the coordinates
        int actualStartX = startX * (texture.width / divisor);
        int actualStartY = startY * (texture.height / divisor);

        for (int row = actualStartY; row < actualStartY + texture.height / divisor; ++row)
        {
            int rowOffset = (texture.width) * row * 4;

            for (int col = actualStartX; col < actualStartX + texture.width / divisor; ++col)
            {
                int pixelIndex = rowOffset + col * 4;
                
                pixels[pixelIndex] = (byte)(byte.MaxValue - pixels[pixelIndex]);
                pixels[pixelIndex + 1] = (byte)(byte.MaxValue - pixels[pixelIndex + 1]);
                pixels[pixelIndex + 2] = (byte)(byte.MaxValue - pixels[pixelIndex + 2]);
            }
        }
    }

}
