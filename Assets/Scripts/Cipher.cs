using System.Text;
using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Cipher : MonoBehaviour
{
    public int codeLength = 10;
    char[] answer, encodedAnswer;
    public string answerString, encodedAnswerString;
    int shownPieces = 0;
    public TMP_Text successTextPro;
    public ButtonMinigame buttonMinigame;
    string textInput;
    public GameObject textInputGO;

    System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        textInputGO.SetActive(true);
        answer = new char[codeLength];
        encodedAnswer = new char[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            int charNum = rnd.Next(65, 91);
            answer[i] = (char)charNum;

            charNum += 8;

            if (charNum > 90)
            {
                charNum -= 26;
            }
            encodedAnswer[i] = (char)charNum;

            StringBuilder sb = new StringBuilder(codeLength);
            sb.Append(answer);
            answerString = sb.ToString();
            sb.Clear();
            sb.Append(encodedAnswer);
            encodedAnswerString = sb.ToString();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && textInput.ToUpper() == answerString)
        {
            try
            {
                SceneManager.LoadScene("Win");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load scene: {ex.Message}");
            }

        }
    }
    public void ShowPiece()
    {
        
        shownPieces++;
        if (shownPieces == codeLength)
        {
            successTextPro.text = "Success!\nThe last encoded character of the password is: " + encodedAnswer[shownPieces - 1];
            buttonMinigame.lastCharReached = true;
        }
        else
        {
            successTextPro.text = "Success!\nThe next encoded character of the password is: " + encodedAnswer[shownPieces-1];
        }
    }
    public void TextInput(string text)
    {
        textInput = text;
    }
}
