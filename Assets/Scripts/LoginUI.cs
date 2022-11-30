using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] TMP_InputField userEmail, userName, userPassword, userPasswordMatch, displayName;
    [SerializeField] Button loginButtonUsername, registerButton;
    [SerializeField] TextMeshProUGUI Msg;

    public GameObject toLoginUIButton, toRegisterUIButton;

    private void Start()
    {
        SwitchToLogin();
    }

    public void DisableAllUI()
    {
        userEmail.gameObject.SetActive(false);
        userName.gameObject.SetActive(false);
        userPassword.gameObject.SetActive(false);
        userPasswordMatch.gameObject.SetActive(false);
        displayName.gameObject.SetActive(false);

        loginButtonUsername.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(false);

        Msg.gameObject.SetActive(false);

        toLoginUIButton.SetActive(false);
        toRegisterUIButton.SetActive(false);
    }

    public void SwitchToLogin()
    {
        DisableAllUI();

        userName.gameObject.SetActive(true);
        userPassword.gameObject.SetActive(true);

        loginButtonUsername.gameObject.SetActive(true);

        Msg.gameObject.SetActive(true);

        toRegisterUIButton.SetActive(true);
    }

    public void SwitchToRegister()
    {
        DisableAllUI();

        userEmail.gameObject.SetActive(true);
        userName.gameObject.SetActive(true);
        userPassword.gameObject.SetActive(true);
        userPasswordMatch.gameObject.SetActive(true);
        displayName.gameObject.SetActive(true);

        registerButton.gameObject.SetActive(true);

        Msg.gameObject.SetActive(true);

        toLoginUIButton.SetActive(true);
    }
}
