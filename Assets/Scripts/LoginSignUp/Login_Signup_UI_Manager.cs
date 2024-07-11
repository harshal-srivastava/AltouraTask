using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Enums used to determine the current user state in the application
/// Used while checking if the user is logging in or signing up
/// </summary>
public enum UserEntryMode
{
    Login,
    SignUp,
    None
}

/// <summary>
/// Class responsible for handling the UI/UX flow for the login/sign up process in the application
/// </summary>
public class Login_Signup_UI_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject userStatusScreen;

    [SerializeField]
    private GameObject loginSignUpScreen;

    public UserEntryMode currUserEntryMode;

    [SerializeField]
    private TextMeshProUGUI loginOrSignUpButtonText;

    [SerializeField]
    private GameObject goToLoginScreenPopUp;

    [SerializeField]
    private TMP_InputField userNameInputField;

    [SerializeField]
    private TMP_InputField passWordInputField;

    [SerializeField]
    private TextMeshProUGUI errorText;

    [SerializeField]
    private LoginSignUpManager loginSignUpManagerRef;

    [SerializeField]
    private GameObject loginSuccessPopUp;

    /// <summary>
    /// Delegate to send out an application wide event when user login is completed
    /// </summary>
    public delegate void LoginComplete();
    public static LoginComplete LoginCompleteEvent;


    private void Awake()
    {
        AttachEventSpecificListeners();
    }

    private void Start()
    {
        ShowHomeScreen();
    }

    /// <summary>
    /// Function to show homescreen at the start of the application
    /// </summary>
    private void ShowHomeScreen()
    {
        userStatusScreen.SetActive(true);
        loginSignUpScreen.SetActive(false);
        ResetElements();
    }

    /// <summary>
    /// Function to reset the visual elements to their default state
    /// </summary>
    private void ResetElements()
    {
        currUserEntryMode = UserEntryMode.None;
        userNameInputField.text = "";
        passWordInputField.text = "";
        errorText.text = "";
    }

    /// <summary>
    /// Function to show the sign up or login screen
    /// One screen gameobject is used to show sign up or login based on what user selects
    /// </summary>
    /// <param name="mode"></param>
    public void ShowSignUporLoginScreen(int mode)
    {
        ResetElements();
        userStatusScreen.SetActive(false);
        currUserEntryMode = mode==0?UserEntryMode.Login:UserEntryMode.SignUp;
        SetupSignUpLoginScreenAccordingToEntryMode();
    }

    /// <summary>
    /// Function to setup the sign up or login screen elements based on the user's choice
    /// </summary>
    private void SetupSignUpLoginScreenAccordingToEntryMode()
    {
        switch (currUserEntryMode)
        {
            case UserEntryMode.Login:
                loginOrSignUpButtonText.text = "Login";
                break;
            case UserEntryMode.SignUp:
                loginOrSignUpButtonText.text = "Sign Up";
                break;
        }
        loginSignUpScreen.SetActive(true);
    }

    /// <summary>
    /// Function to go back to the previous screen
    /// </summary>
    public void BackButtonPressed()
    {
        ShowHomeScreen();
    }

    /// <summary>
    /// Function to show or display any error while signing up or logging in
    /// </summary>
    /// <param name="error"></param>
    private void ShowError(string error)
    {
        StartCoroutine(DisplayError(error));
    }

    /// <summary>
    /// Coroutine to show the error along with error message
    /// And disable it after 2 seconds
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    private IEnumerator DisplayError(string error)
    {
        errorText.text = error;
        yield return new WaitForSeconds(2);
        errorText.text = "";
    }

    /// <summary>
    /// Attached to Signup/Login button
    /// Called when user presses the Sign up/ Login button
    /// </summary>
    public void LoginOrSignUp()
    {
        switch (currUserEntryMode)
        {
            case UserEntryMode.Login:
                Login();
                break;
            case UserEntryMode.SignUp:
                SignUp();
                break;
        }

    }

    /// <summary>
    /// Function to attach game event listeners
    /// Helps in decoupling the referencing to multiple classes
    /// </summary>
    private void AttachEventSpecificListeners()
    {
        LoginSignUpManager.LoginFailedEvent += LoginFailed;
        UserLoadSaveManager.NewUserInfoSavedSuccessEvent += SignUpSuccess;
        LoginSignUpManager.LoginSuccessEvent += LoginSuccess;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachEventSpecificListeners()
    {
        LoginSignUpManager.LoginFailedEvent -= LoginFailed;
        UserLoadSaveManager.NewUserInfoSavedSuccessEvent -= SignUpSuccess;
        LoginSignUpManager.LoginSuccessEvent -= LoginSuccess;
    }


    #region Login related functions
    /// <summary>
    /// Function to log the user in
    /// </summary>
    private void Login()
    {
        //condition added to check whether the credentials added by user are valid, if not, no point in making login call
        if (!AreCredentialsValid())
        {
            return;
        }
        loginSignUpManagerRef.VerifyPassword(userNameInputField.text, passWordInputField.text);
    }

    /// <summary>
    /// Listener to the LoginSignUpManager.LoginSuccessEvent
    /// </summary>
    /// <param name="username"></param>
    private void LoginSuccess(string username)
    {
        loginSuccessPopUp.SetActive(true);
    }

    /// <summary>
    /// Listener for the LoginSignUpManager.LoginFailedEvent
    /// </summary>
    /// <param name="reason"></param>
    private void LoginFailed(string reason)
    {
        ShowError(reason);
    }

    /// <summary>
    /// Function called when user presses next button after successfully logging in
    /// </summary>
    public void LoginSuccessPopUpNextButtonPressed()
    {
        LoginCompleteEvent?.Invoke();
    }
    #endregion

    #region Signup related functions

    /// <summary>
    /// Function call to sign up a new user
    /// </summary>
    private void SignUp()
    {
        //condition added to check whether the credentials added by user are valid, if not, no point in making login call
        if (!AreCredentialsValid(true))
        {
            return;
        }
        loginSignUpManagerRef.SignUpNewUser(userNameInputField.text, passWordInputField.text);
    }

    /// <summary>
    /// Listener to the UserLoadSaveManager.NewUserInfoSavedSuccessEvent
    /// </summary>
    /// <param name="newUser"></param>
    private void SignUpSuccess(UserData newUser)
    {
        goToLoginScreenPopUp.SetActive(true);
    }
    #endregion

    /// <summary>
    /// Function to check whether the credentials added by user are appropriate or not
    /// </summary>
    /// <param name="checkForUniqueUsername"></param>
    /// <returns></returns>
    private bool AreCredentialsValid(bool checkForUniqueUsername = false)
    {
        
        if (userNameInputField.text == "") //username field should not be blank
        {
            ShowError("Enter a valid username");
            return false;
        }
        if (passWordInputField.text == "") //password field should not be blank
        {
            ShowError("Enter a valid password");
            return false;
        }
        if (checkForUniqueUsername) // check to ensure user puts in a distinct username while signing up
        {
            if (loginSignUpManagerRef.UserExists(userNameInputField.text))
            {
                ShowError("Username not available, select different username");
                return false;
            }
        }
        return true;
    }

    private void OnDestroy()
    {
        DetachEventSpecificListeners();
    }


}
