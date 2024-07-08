using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginSignUpManager : MonoBehaviour
{
    public static string userName;
    private string userPassword;

    [SerializeField]
    UserLoadSaveManager saveLoadManagerRef;

    public delegate void UserLoginSuccessful(string username);
    public static UserLoginSuccessful LoginSuccessEvent;

    public delegate void UserLoginFailed(string reason);
    public static UserLoginFailed LoginFailedEvent;

    public delegate void UserSignUpSuccessful(string username);
    public static UserSignUpSuccessful SignUpSuccessEvent;


    private void Awake()
    {
        AttachSpecificEventListeners();
    }

    public bool UserExists(string username)
    {
        return saveLoadManagerRef.CheckUser(username);
    }

    public void VerifyPassword(string username, string password)
    {
        if (!UserExists(username))
        {
            LoginFailedEvent?.Invoke("Login failed, user doesn't exists");
            return;
        }
        UserData data = new UserData();
        data.userName = username;
        data.userPassword = password;
        if (saveLoadManagerRef.IsPasswordCorrect(data))
        {
            LoginSuccessEvent?.Invoke(username);
            userName = username;
        }
        else
        {
            LoginFailedEvent?.Invoke("Login failed, incorrect password");
        }
    }

    public void SignUpNewUser(string username, string password)
    {
        UserData data = new UserData();
        data.userName = username;
        data.userPassword = password;
        saveLoadManagerRef.SaveUserToFile(data);
    }

    void NewUserSignUpSuccessful(UserData newUser)
    {
        SignUpSuccessEvent?.Invoke(newUser.userName);
    }

    void AttachSpecificEventListeners()
    {
        UserLoadSaveManager.NewUserInfoSavedSuccessEvent += NewUserSignUpSuccessful;
    }

    void DetachSpecificEventListeners()
    {
        UserLoadSaveManager.NewUserInfoSavedSuccessEvent -= NewUserSignUpSuccessful;
    }

    private void OnDestroy()
    {
        DetachSpecificEventListeners();
    }
}
