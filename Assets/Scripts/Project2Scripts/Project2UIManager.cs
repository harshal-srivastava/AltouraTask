using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIState
{
    Text,
    TextAndImage,
    Teleported,

}
public class Project2UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI displayText;

    [SerializeField]
    private Image displayImage;

    [SerializeField]
    private UIState currUIState;

    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private GameObject backButton;

    [SerializeField]
    private Sprite displaySprite;

    [SerializeField]
    private string displaySpriteName;

    public delegate void ActivateTeleportation(bool reverse);
    public static ActivateTeleportation ActivateTeleportationEvent;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        displayText.text = "Click here";
        backButton.SetActive(false);
        currUIState = UIState.Text;
        displaySprite = null;
        displayImage.enabled = false;
    }

    public void NextButtonPressed()
    {
        
        switch (currUIState)
        {
            case UIState.Text:
                SetTextandDisplayImage();
                break;
            case UIState.TextAndImage:
                Teleport();
                break;
        }
        if (currUIState != UIState.Teleported)
        {
            currUIState++;
        }
    }

    private void Teleport()
    {
        nextButton.SetActive(false);
        backButton.SetActive(true);
        ActivateTeleportationEvent?.Invoke(false);
    }

    private void SetTextandDisplayImage()
    {
        displayText.text = "Click again to teleport";
        if (displaySprite == null)
        {
            displaySprite = ResourceLoaderUtil.instance.LoadSprite(displaySpriteName);
        }
        displayImage.sprite = displaySprite;
        displayImage.enabled = true;
        backButton.SetActive(true);
    }

    private void ResetDisplayTextAndImage()
    {
        displayText.text = "Click Here";
        displayImage.sprite = null;
        displayImage.enabled = false;
        backButton.SetActive(false);
    }

    public void BackButtonPressed()
    {
       
        switch (currUIState)
        {
            case UIState.Teleported:
                ActivateBackTeleportation();
                break;
            case UIState.TextAndImage:
                ResetDisplayTextAndImage();
                break;
            case UIState.Text:
                break;
        }
        if (currUIState != UIState.Text)
        {
            currUIState--;
        }
    }

    private void ActivateBackTeleportation()
    {
        nextButton.SetActive(true);
        ActivateTeleportationEvent?.Invoke(true);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
