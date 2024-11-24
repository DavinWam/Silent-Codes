using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class AudioListen : MonoBehaviour
{
    public float sensitivity = 25;
    public GameObject bar;
    public RawImage barImage;
    public GameControl gameControl;
    public float rms;
    bool tooLoudFlag, hang = false;
    float hangCount;

    AudioSource audioSource;
    RectTransform t;
    Vector3 s;
    float sy;

    private void Start()
    {
        barImage.color = new Color32(225, 247, 244, 255);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        audioSource.loop = true;
        //audioSource.volume = .01f;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
        t = bar.GetComponent<RectTransform>();
        s = t.localScale;
        sy = s.y;

        tooLoudFlag = false;
    }

    private void Update()
    {
        if (!hang || (hangCount > .3f))
        {
            hang = false;
            hangCount = 0;

            float currentLoudness = rms * sensitivity;
            s.y = sy * currentLoudness;
            if (s.y > 1.2f)
            {
                s.y = 1.2f;
            }
            t.localScale = s;


            if (currentLoudness >= 1)
            {
                if (!tooLoudFlag)
                {
                    StartCoroutine(ShowTooLoudAlert());
                    hang = true;
                }
            }
        }
        else
        {
            hangCount += Time.deltaTime;
        }
    }

    IEnumerator ShowTooLoudAlert()
    {
        gameControl.OnPlayerTooLoud();
        tooLoudFlag = true;
        barImage.color = new Color32(169, 50, 50, 255);
        yield return new WaitForSeconds(1f);
        barImage.color = new Color32(225, 247, 244, 255);
        tooLoudFlag = false;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        rms = 0;

        for (int i = 0; i < data.Length; ++i)
        {
            rms += data[i] * data[i];
        }

        rms = Mathf.Sqrt(rms / data.Length);
    }
}
