using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIState
{
    None,
    Text,
    TextAndImage,
    Teleported,

}
public class Project2UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textElement;

    [SerializeField]
    private Image displayImage;

    [SerializeField]
    private UIState currUIState;

    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private GameObject backButton;


    public void NextButtonPressed()
    {
        if (currUIState != UIState.Teleported)
        {
            currUIState++;
        }
    }

    public void BackButtonPressed()
    {
        if (currUIState != UIState.None)
        {
            currUIState--;
        }
    }
}
