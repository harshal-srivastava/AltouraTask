using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for handling UI/UX flow for project selection screen
/// </summary>
public class ProjectSelectionUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI usernameText;

    /// <summary>
    /// Delegate event to be called when project 1 is to be loaded
    /// </summary>
    public delegate void Project1Initiated();
    public static Project1Initiated Project1InitiatedEvent;

    /// <summary>
    /// Delegate event to be called when project 2 is to be loaded
    /// </summary>
    public delegate void Project2Initiated();
    public static Project2Initiated Project2InitiatedEvent;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Function to initialize values when the project selection screen shows up
    /// </summary>
    private void Initialize()
    {
        usernameText.text = string.Format("User : {0}", LoginSignUpManager.userName);
    }

    /// <summary>
    /// Function to send out the application wide event call to initialize project 1
    /// All UI and functionalities will be initiated when user presses on project 1 button
    /// </summary>
    public void InitiateProject1()
    {
        Project1InitiatedEvent?.Invoke();
    }


    /// <summary>
    /// Function to send out the application wide event call to initialize project 2
    /// All UI and functionalities will be initiated when user presses on project 2 button
    /// </summary>
    public void InitiateProject2()
    {
        Project2InitiatedEvent?.Invoke();
    }

    /// <summary>
    /// Function to quit the application
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}
