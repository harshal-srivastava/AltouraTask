using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Enum representation for the user and the UI state
/// </summary>
public enum UIState
{
    Text,
    TextAndImage,
    Teleported,

}

/// <summary>
/// Class responsible for the UI/UX flow for the part 2 of the assignment
/// </summary>
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

    /// <summary>
    /// Delegate coupled with static event when the user is getting teleported
    /// </summary>
    /// <param name="reverse"></param>
    public delegate void ActivateTeleportation(bool reverse);
    public static ActivateTeleportation ActivateTeleportationEvent;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Initialize the level state to the default value
    /// </summary>
    private void Initialize()
    {
        displayText.text = "Click here";
        backButton.SetActive(false);
        currUIState = UIState.Text;
        displaySprite = null;
        displayImage.enabled = false;
    }

    /// <summary>
    /// Function called when the user presses the next button on the world space UI
    /// </summary>
    public void NextButtonPressed()
    {
        //based on the current state, execute the application flow
        switch (currUIState)
        {
            case UIState.Text:
                SetTextandDisplayImage();
                break;
            case UIState.TextAndImage:
                Teleport();
                break;
        }
        //update the current UI state, since it is the enum, hence added the restricting condition
        if (currUIState != UIState.Teleported)
        {
            currUIState++;
        }
    }

    /// <summary>
    /// Function to start the teleportation mechanism
    /// Called when user presses the next button
    /// </summary>
    private void Teleport()
    {
        nextButton.SetActive(false);
        backButton.SetActive(true);
        ActivateTeleportationEvent?.Invoke(false);
    }

    /// <summary>
    /// Function called when user presses the next button for the first time
    /// </summary>
    private void SetTextandDisplayImage()
    {
        displayText.text = "Click again to teleport";
        //using the ResourceLoaderUtil class to load the sprite required rather than referencing here
        if (displaySprite == null)
        {
            displaySprite = ResourceLoaderUtil.instance.LoadSprite(displaySpriteName);
        }
        displayImage.sprite = displaySprite;
        displayImage.enabled = true;
        backButton.SetActive(true);
    }

    /// <summary>
    /// Reset the visual UI elements to default state
    /// </summary>
    private void ResetDisplayTextAndImage()
    {
        displayText.text = "Click Here";
        displayImage.sprite = null;
        displayImage.enabled = false;
        backButton.SetActive(false);
    }

    /// <summary>
    /// Function called when user presses the back button in the world space UI
    /// </summary>
    public void BackButtonPressed()
    {
        //update the application and UI flow based on the application state
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
        //decrement the current UI state, since it is enum, hence added the restricting condition
        if (currUIState != UIState.Text)
        {
            currUIState--;
        }
    }

    /// <summary>
    /// Function to start the mechanism to teleport the user back to their last location
    /// </summary>
    private void ActivateBackTeleportation()
    {
        nextButton.SetActive(true);
        ActivateTeleportationEvent?.Invoke(true);
    }

    /// <summary>
    /// Function to Quit the application
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}
