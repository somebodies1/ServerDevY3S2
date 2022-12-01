using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField]
    GameObject userEmailInput, userNameInput, userPasswordInput, userPasswordMatchInput, displayNameInput, //Input fields
                                loginButtonUsername, registerButton, sendRecoveryEmailButton, guestLoginButton, //Buttons
                                userNameText, passwordText, confirmPasswordText, emailText, displayNameText, Msg,    //Text message
                                toLoginUIButton, toRegisterUIButton, toForgotPasswordUIButton, //UI changing buttons
                                loginBG, registerBG, forgotPasswordBG; //Backgrounds


    //[SerializeField] TMP_InputField userEmail, userName, userPassword, userPasswordMatch, displayName;
    //[SerializeField] Button loginButtonUsername, registerButton;
    //[SerializeField] TextMeshProUGUI Msg;

    private void Start()
    {
        SwitchToLogin();
    }

    void SetGameObjectActiveIfExist(GameObject _go, bool _active = true)
    {
        if (_go)
            _go.SetActive(_active);
    }

    public void DisableAllUI()
    {
        SetGameObjectActiveIfExist(userEmailInput, false);
        SetGameObjectActiveIfExist(userNameInput, false);
        SetGameObjectActiveIfExist(userPasswordInput, false);
        SetGameObjectActiveIfExist(userPasswordMatchInput, false);
        SetGameObjectActiveIfExist(displayNameInput, false);

        SetGameObjectActiveIfExist(loginButtonUsername, false);
        SetGameObjectActiveIfExist(registerButton, false);
        SetGameObjectActiveIfExist(sendRecoveryEmailButton, false);
        SetGameObjectActiveIfExist(guestLoginButton, false);

        SetGameObjectActiveIfExist(userNameText, false);
        SetGameObjectActiveIfExist(passwordText, false);
        SetGameObjectActiveIfExist(confirmPasswordText, false);
        SetGameObjectActiveIfExist(emailText, false);
        SetGameObjectActiveIfExist(displayNameText, false);
        SetGameObjectActiveIfExist(Msg, false);

        SetGameObjectActiveIfExist(toLoginUIButton, false);
        SetGameObjectActiveIfExist(toRegisterUIButton, false);
        SetGameObjectActiveIfExist(toForgotPasswordUIButton, false);

        SetGameObjectActiveIfExist(loginBG, false);
        SetGameObjectActiveIfExist(registerBG, false);
        SetGameObjectActiveIfExist(forgotPasswordBG, false);
    }

    public void SwitchToLogin()
    {
        DisableAllUI();

        SetGameObjectActiveIfExist(userNameInput);
        SetGameObjectActiveIfExist(userPasswordInput);

        SetGameObjectActiveIfExist(loginButtonUsername);
        SetGameObjectActiveIfExist(guestLoginButton);

        SetGameObjectActiveIfExist(userNameText);
        SetGameObjectActiveIfExist(passwordText);
        SetGameObjectActiveIfExist(Msg);

        SetGameObjectActiveIfExist(toRegisterUIButton);
        SetGameObjectActiveIfExist(toForgotPasswordUIButton);

        SetGameObjectActiveIfExist(loginBG);
    }

    public void SwitchToRegister()
    {
        DisableAllUI();

        SetGameObjectActiveIfExist(userEmailInput);
        SetGameObjectActiveIfExist(userNameInput);
        SetGameObjectActiveIfExist(userPasswordInput);
        SetGameObjectActiveIfExist(userPasswordMatchInput);
        SetGameObjectActiveIfExist(displayNameInput);

        SetGameObjectActiveIfExist(registerButton);

        SetGameObjectActiveIfExist(userNameText);
        SetGameObjectActiveIfExist(passwordText);
        SetGameObjectActiveIfExist(confirmPasswordText);
        SetGameObjectActiveIfExist(emailText);
        SetGameObjectActiveIfExist(displayNameText);
        SetGameObjectActiveIfExist(Msg);

        SetGameObjectActiveIfExist(toLoginUIButton);

        SetGameObjectActiveIfExist(registerBG);
    }

    public void SwitchToForgotPassword()
    {
        DisableAllUI();

        SetGameObjectActiveIfExist(userEmailInput);

        SetGameObjectActiveIfExist(sendRecoveryEmailButton);

        SetGameObjectActiveIfExist(emailText);
        SetGameObjectActiveIfExist(Msg);

        SetGameObjectActiveIfExist(toLoginUIButton);

        SetGameObjectActiveIfExist(forgotPasswordBG);
    }
}
