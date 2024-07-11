using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectSelectionUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI usernameText;

    public delegate void Project1Initiated();
    public static Project1Initiated Project1InitiatedEvent;

    public delegate void Project2Initiated();
    public static Project2Initiated Project2InitiatedEvent;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        usernameText.text = string.Format("User : {0}", LoginSignUpManager.userName);
    }

    public void InitiateProject1()
    {
        Project1InitiatedEvent?.Invoke();
    }

    public void InitiateProject2()
    {
        Project2InitiatedEvent?.Invoke();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
