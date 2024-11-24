using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetInteractable(bool contRedraw)
    {
        button.interactable = !contRedraw;
    }
}
