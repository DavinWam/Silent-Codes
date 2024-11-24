using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameControl : MonoBehaviour
{
    /* all variables are arbitrary; subject to change */

    /* we can do a little bar on the bottom w the minigame icon (puzzle piece?)
    so that when we click it and it is hidden, we can make it active. when 
    it is visible (active), then we can hide it. just easier coding 

    whip up smth for main menu (start, instructions, credits button?)
    or just do start button for simplicity lol 

    and then smth for game over 
    */

    public GameObject minigameWindow, wordWindow;
    public RectTransform textInput;
    public RawImage bossShadowColor;
    public TMP_Text timeText, bossIncomingText, wordText;
    public TextMeshProUGUI wordTextGUI;
    public GameState gameState;
    Coroutine flashingText;
    WordScript wordScript = new WordScript();

    private bool fadedIn = false, initialized = false; // is the boss currently checking on player?
    private string bossState = "Away";

    private float minCheckTime = 10f; // minimum time between checks
    private float maxCheckTime = 25f; // maximum time between checks
    public float nextCheckTime;
    
    private int bossCheckDuration; // duration that boss is checking
    private int bossCheckDurationMin = 30;
    private int bossCheckDurationMax = 100;
    public int charsTypedAroundBoss = 0;
    private int totalCharsTyped = 0;
    private int linesFit;

    private float timer = 180f;

    private float warningDuration = 2f; // warning time before boss check
    private float bossCheckStartTime; // time when boss check started (to keep track)


    void Start()
    {
        minigameWindow.SetActive(true);
        wordWindow.SetActive(false);
        bossIncomingText.color = new Color32(137, 190, 231, 255);
        bossShadowColor.color = new Color32(0, 0, 0, 0);
        bossIncomingText.text = "BOSS INCOMING";
        nextCheckTime = Time.time + GetNextCheckTime();
        timeText.text = "05:00";

        linesFit = (int)textInput.rect.height / 12;
        initialized = true;
    }
    private void OnRectTransformDimensionsChange()
    {
        if (initialized)
        {
            linesFit = (int)textInput.rect.height / 12;
            if (wordTextGUI.textInfo.lineCount > linesFit)
            {
                textInput.anchoredPosition = Vector3.up * ((wordTextGUI.textInfo.lineCount - linesFit) * 12);
            }
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameState.GameOver();
        }

        int seconds = (int)(timer % 60);
        if (seconds < 10)
        {
            timeText.text = "0" + ((int)timer / 60) + ":0" + seconds;
        }
        else
        {
            timeText.text = "0" + ((int)timer / 60) + ":" + seconds;
        }

        /* if boss check is finished */
        if (bossState == "Here" && Time.time >= (bossCheckStartTime + warningDuration) && charsTypedAroundBoss >= bossCheckDuration)
        {
            StartCoroutine(BossFadeOut());
            bossState = "Away";
            fadedIn = false;
            bossIncomingText.text = "BOSS INCOMING";
            nextCheckTime = Time.time + GetNextCheckTime();
        }

        /* if in middle of checking */
        else if (bossState == "Approaching" && Time.time >=
            (bossCheckStartTime + warningDuration))
        {
            bossState = "Here";
            bossIncomingText.text = "BE QUIET";
            StopCoroutine(flashingText);
            bossIncomingText.color = new Color32(137, 190, 231, 255);
            StartCoroutine(BossWindowCheck());
            //bossIncomingText.color = new Color32(255, 255, 255, 0); 
            //bossShadow.SetActive(true);
        }

        else if (Time.time >= nextCheckTime + warningDuration - .5f && !fadedIn)
        {
            fadedIn = true;
            StartCoroutine(BossFadeIn());
        }

        else if (Time.time >= nextCheckTime && bossState == "Away")
        {
            flashingText = StartCoroutine(BossComingFlashing());
            //bossIncomingText.SetActive(true);
            bossCheckStartTime = Time.time;
            bossState = "Approaching";
        }

        if (Input.anyKeyDown && wordWindow.activeSelf && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            try
            {
                if (totalCharsTyped == 0)
                {
                    wordText.text = "";
                }
                wordText.text += wordScript.wordText[totalCharsTyped];
                if (wordTextGUI.textInfo.lineCount > linesFit)
                {
                    textInput.anchoredPosition = Vector3.up * ((wordTextGUI.textInfo.lineCount - linesFit) * 12);
                }

                if (bossState == "Here")
                {
                    charsTypedAroundBoss++;
                }
                totalCharsTyped++;
            }
            catch (Exception E)
            {
                UnityEngine.Debug.Log(E.Message);
                gameState.GameOver();
            }
        }

        if (Input.GetKey("left") && Input.GetKey("left ctrl") && Input.GetKey("x"))
        {
            nextCheckTime = 10000;
        }
        if (Input.GetKey("right") && Input.GetKey("left ctrl") && Input.GetKey("m"))
        {
            if (AudioListener.volume == 0)
            {
                AudioListener.volume = 1;
            }
            else
            {
                AudioListener.volume = 0;
            }
        }
    }
    IEnumerator BossComingFlashing()
    {
        while (true)
        {
            bossIncomingText.color = new Color32(169, 50, 50, 255);
            yield return new WaitForSeconds(.3f);
            bossIncomingText.color = new Color32(137, 190, 231, 255);
            yield return new WaitForSeconds(.3f);
        }
    }
    IEnumerator BossFadeOut()
    {
        float shadowAlpha = 100;
        while (shadowAlpha != 0)
        {
            shadowAlpha -= Time.deltaTime * 100;
            if (shadowAlpha < 0)
            {
                shadowAlpha = 0;
            }
            bossShadowColor.color = new Color32(0, 0, 0, (byte)shadowAlpha);
            yield return 0;
        }
    }
    IEnumerator BossFadeIn()
    {
        float shadowAlpha = 0;
        while (shadowAlpha != 100)
        {
            shadowAlpha += Time.deltaTime * 100;
            if (shadowAlpha > 100)
            {
                shadowAlpha = 100;
            }
            bossShadowColor.color = new Color32(0, 0, 0, (byte)shadowAlpha);
            yield return 0;
        }

    }
    IEnumerator BossWindowCheck()
    {
        while (bossState == "Here")
        {
            if (minigameWindow.activeSelf)
            {
                gameState.LoseLife();
                yield return new WaitForSeconds(3f);
            }
            yield return 0;
        }
    }

    float GetNextCheckTime()
    {
        charsTypedAroundBoss = 0;
        bossCheckDuration = UnityEngine.Random.Range(bossCheckDurationMin, bossCheckDurationMax);

        return UnityEngine.Random.Range(minCheckTime, maxCheckTime);
    }

    /* called if too loud via Update() */
    public void OnPlayerTooLoud()
    {
        // if player is too loud and boss is checking, lose a life
        if (bossState == "Here")
        {
            gameState.LoseLife();
        }
        else
        {
            if (Time.time + 2 >= nextCheckTime)
            {
                return;
            }
            nextCheckTime -= UnityEngine.Random.Range(4f, 7f);
            if (Time.time + 2 >= nextCheckTime)
            {
                nextCheckTime = Time.time + 2;
            }
        }
    }
}
