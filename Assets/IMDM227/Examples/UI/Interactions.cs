using UnityEngine;

public class Interactions : MonoBehaviour
{
    public float sliderValue = 123.45f;
    public int buttonPressCount = 0;
    public bool toggleState = false;

    public void PressThisMess()
    {
        buttonPressCount = buttonPressCount + 1;
    }

    public void SlideIntoThis(float someValue)
    {
        sliderValue = someValue;
    }

    public void WhichIsIt(bool onOrOff)
    {
        toggleState = onOrOff;
    }
}
