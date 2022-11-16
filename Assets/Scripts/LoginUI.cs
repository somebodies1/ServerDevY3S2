using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    

    [SerializeField] TMP_InputField userEmail, userName, userPassword, displayName;
    [SerializeField] Button loginButtonUsername, registerButton, updateDisplayNameButton;
    [SerializeField] TextMeshProUGUI Msg;

    public void DisableAllUI()
    {

    }

    public void SwitchToLogin()
    {
        DisableAllUI();

        userName.gameObject.SetActive(true);
        userPassword.gameObject.SetActive(true);

        loginButtonUsername.gameObject.SetActive(true);
    }

    public void SwitchToRegister()
    {

    }
}
