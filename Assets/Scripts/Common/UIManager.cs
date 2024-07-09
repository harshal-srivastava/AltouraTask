using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] applicationScreensArray;

    // 0 : login/signup screen
    // 1 : projects screen
    // 2 : Project 1 screen
    // 3 : project 2 screen

    private void ShowScreen(int index)
    {
        DisableAllScreens();
        if (applicationScreensArray[index] != null)
        {
            applicationScreensArray[index].SetActive(true);
        }
    }

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

    private void AttachEventSpecificListeners()
    {
        Login_Signup_UI_Manager.LoginCompleteEvent += GoToProjectScreen;
    }

    private void DetachEventSpecificListeners()
    {
        Login_Signup_UI_Manager.LoginCompleteEvent -= GoToProjectScreen;
    }

    private void GoToProjectScreen()
    {
        ShowScreen(1);
    }

    private void OnDestroy()
    {
        DetachEventSpecificListeners();
    }
}
