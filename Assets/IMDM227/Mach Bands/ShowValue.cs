using UnityEngine;
using TMPro;

public class ShowValue : MonoBehaviour
{
    TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void SetValue(float value)
    {
        int iValue = (int)(value + 0.5f);

        tmp.text = $"{iValue}";
    }
}
