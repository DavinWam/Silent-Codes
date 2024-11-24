using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public void Button()
    {
        // No need to check if it's in the editor, just load the scene directly
        try
        {
            SceneManager.LoadScene("MainGame");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }
}
