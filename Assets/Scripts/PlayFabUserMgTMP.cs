using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabUserMgTMP : MonoBehaviour
{
    public static string entityKeyID;
    public static string entityKeyType;

    [SerializeField] TMP_InputField userEmail, userPassword, userPasswordMatch, userName, currentScore, displayName, XP, level;
    [SerializeField] TextMeshProUGUI Msg;

    private void Start()
    {
        Debug.Log("EntKey: " + entityKeyID);
    }

    //to display in console and messagebox
    void UpdateMsg(string msg)
    {
        Debug.Log(msg);
        Msg.text = msg;
    }

    //Report any errors
    void OnError(PlayFabError e)
    {
        UpdateMsg("Error" + e.GenerateErrorReport());
    }

    void OnRegSuccess(RegisterPlayFabUserResult r)
    {   
        UpdateMsg("Registration success!");

        //Create player display name
        var req = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName.text
        };

        //Update profile
        PlayFabClientAPI.UpdateUserTitleDisplayName(req, OnDisplayNameUpdate, OnError);

        GoToScene("LoggedInScene");
    }

    public void OnButtonDisplayNameUpdate()
    {
        //Create player display name
        var req = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName.text
        };

        //Update profile
        PlayFabClientAPI.UpdateUserTitleDisplayName(req, OnDisplayNameUpdate, OnError);
    }

    public void OnButtonRegUser()
    {
        if (userPassword.text != userPasswordMatch.text)
        {
            UpdateMsg("Password does not match!");
            return;
        }

        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = userEmail.text,
            Password = userPassword.text,
            Username = userName.text
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegSuccess, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult r)
    {
        UpdateMsg("Display name updated!" + r.DisplayName);
    }

    void OnLoginSuccess(LoginResult r)
    {
        entityKeyID = r.EntityToken.Entity.Id;
        entityKeyType = r.EntityToken.Entity.Type;

        UpdateMsg("Login success!" + r.PlayFabId + r.InfoResultPayload.PlayerProfile.DisplayName);
        GoToScene("LoggedInScene");
    }

    //Login using email and password
    public void OnButtonLoginEmail()
    {
        var loginRequest = new LoginWithEmailAddressRequest
        {
            Email = userEmail.text,
            Password = userPassword.text,

            //To get player profile, to get display name
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, OnLoginSuccess, OnError);
    }

    //Login using username and password
    public void OnButtonLoginUserName()
    {
        var loginRequest = new LoginWithPlayFabRequest
        {
            Username = userName.text,
            Password = userPassword.text,

            //To get player profile, to get display name
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnError);
    }

    public void OnButtonLogout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        UpdateMsg("Logged out!");
        GoToScene("LoginScene");
    }

    public void OnButtonGetLeaderboard()
    {
        var lbreq = new GetLeaderboardRequest
        {
            StatisticName = "Highscore", //Playfab leaderboard name
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(lbreq, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult r)
    {
        string LeaderboardStr = "Leaderboard\n";

        foreach(var item in r.Leaderboard)
        {
            string onerow = item.Position + "/" + item.PlayFabId + "/" + item.DisplayName + "/" + item.StatValue + "\n";
            LeaderboardStr += onerow; //Combine all into one string
        }

        UpdateMsg(LeaderboardStr);
    }

    public void OnButtonSendLeaderboard()
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Highscore",
                    Value = int.Parse(currentScore.text)
                }
            }
        };

        UpdateMsg("Submitting score:" + currentScore.text);
        PlayFabClientAPI.UpdatePlayerStatistics(req, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult r)
    {
        UpdateMsg("Successful leaderboard sent:" + r.ToString());
    }

    //MOTD
    public void ClientGetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if (result.Data == null || !result.Data.ContainsKey("MOTD")) 
                    UpdateMsg("No MOTD");
                else
                    UpdateMsg("MOTD: " + result.Data["MOTD"]);
            },
            error =>
            {
                UpdateMsg("Got error getting titleData:");
                UpdateMsg(error.GenerateErrorReport());
            });
    }

    //Set player data
    public void OnButtonSetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>
            {
                {"XP",XP.text.ToString() },
                {"Level",level.text.ToString() }
            }
        },
        result => UpdateMsg("Successfully updated user data!"),
        error =>
        {
            UpdateMsg("Error setting user data.");
            UpdateMsg(error.GenerateErrorReport());
        });
    }

    //Get player data
    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                UpdateMsg("Got user data.");

                if (result.Data == null || !result.Data.ContainsKey("XP"))
                    Debug.Log("No XP");
                else
                    UpdateMsg("XP: " + result.Data["XP"].Value);

                if (result.Data == null || !result.Data.ContainsKey("Level"))
                    Debug.Log("No Level");
                else
                    UpdateMsg("Level: " + result.Data["Level"].Value);
            },
            (error) =>
            {
                UpdateMsg("Got error retrieving user data.");
                UpdateMsg(error.GenerateErrorReport());
            });
    }

    //Scene changer
    public void GoToScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void OnButtonResetPassword()
    {
        var req = new SendAccountRecoveryEmailRequest
        {
            Email = userEmail.text,
            TitleId = "D7C8A"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(req, OnResetPassword, OnError);
    }

    void OnResetPassword(SendAccountRecoveryEmailResult r)
    {
        UpdateMsg("Password recovery email sent!");
    }

    public void OnButtonGuestLogin()
    {
        var req = new LoginWithCustomIDRequest
        {
            TitleId = "D7C8A",
            CreateAccount = true,
            CustomId = "Guest1",
        };

        PlayFabClientAPI.LoginWithCustomID(req, OnGuestLogin, OnError);
    }

    void OnGuestLogin(LoginResult r)
    {
        UpdateMsg("Logged in as Guest");
        GoToScene("LoggedInScene");
    }

    public string GetEntityKeyID()
    {
        return entityKeyID;
    }

    public string GetEntityKeyType()
    {
        return entityKeyType;
    }
}
