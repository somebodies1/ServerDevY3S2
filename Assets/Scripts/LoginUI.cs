using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField]
    GameObject userEmailInput, userNameInput, userPasswordInput, userPasswordMatchInput, displayNameInput, //Input fields
                                loginButtonUsername, registerButton, //Buttons
                                Msg,    //Text message
                                toLoginUIButton, toRegisterUIButton; //UI changing buttons


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

        SetGameObjectActiveIfExist(Msg, false);

        SetGameObjectActiveIfExist(toLoginUIButton, false);
        SetGameObjectActiveIfExist(toRegisterUIButton, false);

        //userEmail.gameObject.SetActive(false);
        //userName.gameObject.SetActive(false);
        //userPassword.gameObject.SetActive(false);
        //userPasswordMatch.gameObject.SetActive(false);
        //displayName.gameObject.SetActive(false);

        //loginButtonUsername.gameObject.SetActive(false);
        //registerButton.gameObject.SetActive(false);

        //Msg.gameObject.SetActive(false);

        //toLoginUIButton.SetActive(false);
        //toRegisterUIButton.SetActive(false);
    }

    public void SwitchToLogin()
    {
        DisableAllUI();

        SetGameObjectActiveIfExist(userNameInput);
        SetGameObjectActiveIfExist(userPasswordInput);

        SetGameObjectActiveIfExist(loginButtonUsername);

        SetGameObjectActiveIfExist(Msg);

        SetGameObjectActiveIfExist(toRegisterUIButton);

        //userName.gameObject.SetActive(true);
        //userPassword.gameObject.SetActive(true);

        //loginButtonUsername.gameObject.SetActive(true);

        //Msg.gameObject.SetActive(true);

        //toRegisterUIButton.SetActive(true);
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

        SetGameObjectActiveIfExist(Msg);

        SetGameObjectActiveIfExist(toLoginUIButton);

        //userEmail.gameObject.SetActive(true);
        //userName.gameObject.SetActive(true);
        //userPassword.gameObject.SetActive(true);
        //userPasswordMatch.gameObject.SetActive(true);
        //displayName.gameObject.SetActive(true);

        //registerButton.gameObject.SetActive(true);

        //Msg.gameObject.SetActive(true);

        //toLoginUIButton.SetActive(true);
    }
}
