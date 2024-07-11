using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for overall UI/UX flow of the application
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Handling showing of screens from a gameobject array
    /// Allows easier access and showing of screens avoiding multiple function calls
    /// </summary>
    [SerializeField]
    private GameObject[] applicationScreensArray;

    [SerializeField]
    private Camera UICamera;

    // 0 : login/signup screen
    // 1 : projects screen
    // 2 : Project 1 screen
    // 3 : project 2 screen

    /// <summary>
    /// Function to enable the relevant UI screen based on index
    /// </summary>
    /// <param name="index"></param>
    private void ShowScreen(int index)
    {
        DisableAllScreens();
        if (applicationScreensArray[index] != null)
        {
            applicationScreensArray[index].SetActive(true);
        }
    }

    /// <summary>
    /// Function to disable all screens
    /// </summary>
    private void DisableAllScreens()
    {
        for (int i=0;i<applicationScreensArray.Length;i++)
        {
            applicationScreensArray[i].SetActive(false);
        }
    }

    private void Awake()
    {
        AttachEventSpecificListeners();
    }


    /// <summary>
    /// Function to attach game event listeners
    /// Helps in decoupling the referencing to multiple classes
    /// </summary>
    private void AttachEventSpecificListeners()
    {
        Login_Signup_UI_Manager.LoginCompleteEvent += GoToProjectScreen;
        ProjectSelectionUIManager.Project1InitiatedEvent += GoToProject1;
        ProjectSelectionUIManager.Project2InitiatedEvent += GoToProject2;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachEventSpecificListeners()
    {
        Login_Signup_UI_Manager.LoginCompleteEvent -= GoToProjectScreen;
        ProjectSelectionUIManager.Project1InitiatedEvent -= GoToProject1;
        ProjectSelectionUIManager.Project2InitiatedEvent -= GoToProject2;
    }

    /// <summary>
    /// Listener function for the Login_Signup_UI_Manager.LoginCompleteEvent callback
    /// </summary>
    private void GoToProjectScreen()
    {
        ShowScreen(1);
    }

    /// <summary>
    /// Listener function for the ProjectSelectionUIManager.Project1InitiatedEvent callback
    /// </summary>
    private void GoToProject1()
    {
        ShowScreen(2);
    }

    /// <summary>
    /// Listener function for the ProjectSelectionUIManager.Project2InitiatedEvent callback
    /// </summary>
    private void GoToProject2()
    {
        UICamera.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        ShowScreen(3);
    }

    private void OnDestroy()
    {
        DetachEventSpecificListeners();
    }
}
