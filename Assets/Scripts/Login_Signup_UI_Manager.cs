using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum UserEntryMode
{
    Login,
    SignUp,
    None
}
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


    private void Awake()
    {
        AttachEventSpecificListeners();
    }

    private void Start()
    {
        ShowHomeScreen();
    }

    private void ShowHomeScreen()
    {
        userStatusScreen.SetActive(true);
        loginSignUpScreen.SetActive(false);
        ResetElements();
    }

    private void ResetElements()
    {
        currUserEntryMode = UserEntryMode.None;
        userNameInputField.text = "";
        passWordInputField.text = "";
        errorText.text = "";
    }

    public void ShowSignUporLoginScreen(int mode)
    {
        userStatusScreen.SetActive(false);
        currUserEntryMode = mode==0?UserEntryMode.Login:UserEntryMode.SignUp;
        SetupSignUpLoginScreenAccordingToEntryMode();
    }

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

    public void BackButtonPressed()
    {
        ShowHomeScreen();
    }


    private void ShowError(string error)
    {
        StartCoroutine(DisplayError(error));
    }

    private IEnumerator DisplayError(string error)
    {
        errorText.text = error;
        yield return new WaitForSeconds(2);
        errorText.text = "";
    }

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

    private void AttachEventSpecificListeners()
    {
        LoginSignUpManager.LoginFailedEvent += LoginFailed;
    }

    private void DetachEventSpecificListeners()
    {
        LoginSignUpManager.LoginFailedEvent -= LoginFailed;
    }


    #region Login related functions
    private void Login()
    {
        if (!AreCredentialsValid())
        {
            return;
        }
        loginSignUpManagerRef.VerifyPassword(userNameInputField.text, passWordInputField.text);
    }

    private void LoginSuccess()
    {

    }

    private void LoginFailed(string reason)
    {
        ShowError(reason);
    }
    #endregion

    #region Signup related functions
    private void SignUp()
    {
        if (!AreCredentialsValid(true))
        {
            return;
        }
        loginSignUpManagerRef.SignUpNewUser(userNameInputField.text, passWordInputField.text);
    }

    private void SignUpSuccess()
    {

    }
    #endregion

    private bool AreCredentialsValid(bool checkForUniqueUsername = false)
    {
        if (userNameInputField.text == "")
        {
            ShowError("Enter a valid username");
            return false;
        }
        if (passWordInputField.text == "")
        {
            ShowError("Enter a valid password");
            return false;
        }
        if (checkForUniqueUsername)
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
