using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public GameObject[] fullHearts, emptyHearts; // heart icons!
    public AudioSource loseLife;
    private int numLives = 3;


    /* 
    For array of heart icons! Import heart icon asset, then create prefab for it
    by dragging it into scene (creating GameObject) and dragging it back to Assets.
    Then arrange duplicates of prefab, and assign heart icons to array:
    - select GameObject w script attached
    - find Lives field (corresponding to array)
    - set size of array to number of heart icons (which should be 3)
    - drag each heart icon from hierarchy into element of array
    */
    private void Start()
    {
        foreach (GameObject gO in fullHearts)
        {
            gO.SetActive(true);
        }
        foreach (GameObject gO in emptyHearts)
        {
            gO.SetActive(false);
        }
    }

    public void LoseLife()
    {
        loseLife.Play();
        // just coded this so that it will show what will happen when 
        // heart image is grayed out 
        //lives[numLives - 1].GetComponent<Image>().color = Color.gray;
        numLives--;

        StartCoroutine(CoLoseLife());
    }
    IEnumerator CoLoseLife()
    {
        fullHearts[2 - numLives].GetComponent<RawImage>().color = new Color32(169, 50, 50, 255);
        yield return new WaitForSeconds(.3f);
        fullHearts[2 - numLives].SetActive(false);
        emptyHearts[2 - numLives].SetActive(true);



        if (numLives == 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        try
        {
            SceneManager.LoadScene("Lose");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }

}