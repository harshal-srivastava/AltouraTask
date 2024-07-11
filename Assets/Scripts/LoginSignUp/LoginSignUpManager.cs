using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for handling the user login or sign up functionalities
/// </summary>
public class LoginSignUpManager : MonoBehaviour
{
    public static string userName;
    private string userPassword;

    [SerializeField]
    UserLoadSaveManager saveLoadManagerRef;

    /// <summary>
    /// Delegate coupled with static event to be called when user logs in successfully
    /// </summary>
    /// <param name="username"></param>
    public delegate void UserLoginSuccessful(string username);
    public static UserLoginSuccessful LoginSuccessEvent;

    /// <summary>
    /// Delegate coupled with static event to be called if there is any error in user login
    /// </summary>
    /// <param name="reason"></param>
    public delegate void UserLoginFailed(string reason);
    public static UserLoginFailed LoginFailedEvent;

    /// <summary>
    /// Delegate coupled with static event to be called if user has signed up successfully
    /// </summary>
    /// <param name="username"></param>
    public delegate void UserSignUpSuccessful(string username);
    public static UserSignUpSuccessful SignUpSuccessEvent;


    private void Awake()
    {
        AttachSpecificEventListeners();
    }

    /// <summary>
    /// Function to check if the username entered for sign up does not exist in our current user records
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool UserExists(string username)
    {
        return saveLoadManagerRef.CheckUser(username);
    }

    /// <summary>
    /// Function to verify the password entered for the particular user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
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

    /// <summary>
    /// Function call to sign up new user
    /// Basically to add the username and password into the json file that stores all users
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public void SignUpNewUser(string username, string password)
    {
        UserData data = new UserData();
        data.userName = username;
        data.userPassword = password;
        saveLoadManagerRef.SaveUserToFile(data);
    }

    /// <summary>
    /// Function to call the Successful user sign up event
    /// </summary>
    /// <param name="newUser"></param>
    private void NewUserSignUpSuccessful(UserData newUser)
    {
        SignUpSuccessEvent?.Invoke(newUser.userName);
    }

    private void AttachSpecificEventListeners()
    {
        UserLoadSaveManager.NewUserInfoSavedSuccessEvent += NewUserSignUpSuccessful;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachSpecificEventListeners()
    {
        UserLoadSaveManager.NewUserInfoSavedSuccessEvent -= NewUserSignUpSuccessful;
    }

    private void OnDestroy()
    {
        DetachSpecificEventListeners();
    }
}
