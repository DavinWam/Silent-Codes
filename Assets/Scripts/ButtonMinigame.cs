using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonMinigame : MonoBehaviour
{
    public GameObject glowButton, gridParent, failText, successText, newPuzzleButton;
    public TMP_Text nextButton, puzzlesLeft;
    public EventSystem eventSystem;
    public Cipher cipher;
    public GameObject[] grid;
    public AudioSource normalButton, winSound;
    string[] gridAnswer;
    System.Random rnd = new System.Random();
    int correctTilesPressed, targetNumTiles, puzzlesSolved;
    public bool lastCharReached = false, showSuccessText = false, initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        nextButton.text = "START";
        puzzlesLeft.text = "1 of 5";
        newPuzzleButton.SetActive(true);
        gridParent.SetActive(false);
        successText.SetActive(false);
        failText.SetActive(false);

        float gridWidth = gridParent.GetComponent<RectTransform>().rect.width;
        float gridHeight = gridParent.GetComponent<RectTransform>().rect.height;

        grid = new GameObject[16];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int index = i * 4 + j;
                grid[index] = Instantiate(glowButton, gridParent.transform);
                RectTransform rt = grid[index].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(gridWidth / 4, gridHeight / 4);
                rt.anchoredPosition = new Vector3((-3 * gridWidth/8) + (j * gridWidth / 4), (3 * gridHeight / 8) - (i * gridHeight / 4), 0);
                grid[index].transform.GetChild(0).gameObject.SetActive(false);

                grid[index].GetComponent<Button>().onClick.AddListener(ButtonPress);
            }
        }
        initialized = true;
    }
    private void OnRectTransformDimensionsChange()
    {
        if (initialized)
        {
            float gridWidth = gridParent.GetComponent<RectTransform>().rect.width;
            float gridHeight = gridParent.GetComponent<RectTransform>().rect.height;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int index = i * 4 + j;
                    RectTransform rt = grid[index].GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(gridWidth / 4, gridHeight / 4);
                    rt.anchoredPosition = new Vector3((-3 * gridWidth / 8) + (j * gridWidth / 4), (3 * gridHeight / 8) - (i * gridHeight / 4), 0);
                }
            }
        }
    }

    // Update is called once per frame
    public void CreateAnswer()
    {
        showSuccessText = false;
        puzzlesLeft.text = (puzzlesSolved + 1) + " of 5";
        foreach (GameObject gO in grid)
        {
            gO.GetComponent<Button>().interactable = false;
            gO.transform.GetChild(0).gameObject.SetActive(false);
        }
        newPuzzleButton.SetActive(false);
        nextButton.text = "NEXT";
        gridParent.SetActive(true);
        successText.SetActive(false);
        failText.SetActive(false);

        //arrayShuffler code taken from https://stackoverflow.com/questions/14473321/generating-random-unique-values-c-sharp
        int[] shuffledIndexes = Enumerable.Range(0, 16).ToArray(); //create array 0-9


        // Shuffle the array
        for (int i = 0; i < shuffledIndexes.Length; ++i)
        {
            int randomIndex = rnd.Next(shuffledIndexes.Length);
            int temp = shuffledIndexes[randomIndex];
            shuffledIndexes[randomIndex] = shuffledIndexes[i];
            shuffledIndexes[i] = temp;
        }

        targetNumTiles = rnd.Next(6, 14);
        correctTilesPressed = 0;
        gridAnswer = new string[grid.Length];
        for (int i = 0; i < gridAnswer.Length; i++)
        {
            gridAnswer[i] = "empty";
        }
        // Now your array is randomized and you can simply print them in order
        for (int i = 0; i < targetNumTiles; i++)
        {
            grid[shuffledIndexes[i]].transform.GetChild(0).gameObject.SetActive(true);
            gridAnswer[shuffledIndexes[i]] = "lit";
        }

        StartCoroutine(WaitToTurnOff());
    }

    IEnumerator WaitToTurnOff()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject gO in grid)
        {
            gO.transform.GetChild(0).gameObject.SetActive(false);
            gO.GetComponent<Button>().interactable = true;
        }
    }
    public void ButtonPress()
    {
        normalButton.Play();
        GameObject buttonPressed = eventSystem.currentSelectedGameObject;
        for (int i = 0; i < grid.Length; i++)
        {
            if (buttonPressed != grid[i])
            {
                continue;
            }
            if (gridAnswer[i] == "lit")
            {
                buttonPressed.transform.GetChild(0).gameObject.SetActive(true);
                gridAnswer[i] = "clicked";
                correctTilesPressed++;
            }
            else if (gridAnswer[i] == "empty")
            {
                gridParent.SetActive(false);
                failText.SetActive(true);
                StartCoroutine(WaitForNewPuzzle(3));
            }
            if (correctTilesPressed == targetNumTiles)
            {
                winSound.Play();
                puzzlesSolved++;
                gridParent.SetActive(false);
                successText.SetActive(true);
                showSuccessText = true;
                cipher.ShowPiece();
                StartCoroutine(WaitForNewPuzzle(1));
            }
            break;
        }
    }
    IEnumerator WaitForNewPuzzle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (!lastCharReached)
        {
            newPuzzleButton.SetActive(true);
        }
    }
    public void HideEverything()
    {
        newPuzzleButton.SetActive(false);
        gridParent.SetActive(false);
        successText.SetActive(false);
        failText.SetActive(false);
    }
    public void CloseMinigame()
    {
        HideEverything();
    }
    public void OpenMinigame()
    {
        if (puzzlesSolved >= 5)
        {
            newPuzzleButton.SetActive(false);
        }
        else
        {
            newPuzzleButton.SetActive(true);
        }
        gridParent.SetActive(false);
        successText.SetActive(showSuccessText);
        failText.SetActive(false);
    }
}
