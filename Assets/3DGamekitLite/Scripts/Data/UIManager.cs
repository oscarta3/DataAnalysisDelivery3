using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    void Start()
    {
        // Set the text of the button
        buttonText.text = "Click me!";

        // Set the color of the button
        ColorBlock cb = button.colors;
        cb.normalColor = Color.red;
        cb.highlightedColor = Color.green;
        button.colors = cb;
    }
}
