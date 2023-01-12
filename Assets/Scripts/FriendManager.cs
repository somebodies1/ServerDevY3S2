using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI friendListText, leaderboardText;
    [SerializeField] TMP_InputField addFriendInput, removeFriendInput;

    public void UpdateFriendsPanel()
    {
        GetFriends();
        OnGetFriendLB();
    }

    void RemoveFriendByName(string _friendName)
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        },
        result =>
        {
            _friends = result.Friends;
            RemoveFriendByNameCheck(_friends, _friendName);
        },
        DisplayPlayFabError
        );
    }

    void RemoveFriendByNameCheck(List<FriendInfo> _friendInfoList, string _friendName)
    {
        for (int i = 0; i < _friendInfoList.Count; ++i)
        {
            if (_friendInfoList[i].TitleDisplayName == _friendName)
            {
                RemoveFriend(_friendInfoList[i].FriendPlayFabId);
            }
        }
    }

    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        friendListText.text = "Friends: \n";

        friendsCache.ForEach(f =>
        {
            Debug.Log(f.FriendPlayFabId + "," + f.TitleDisplayName);
            friendListText.text += f.TitleDisplayName + "\n";
            if (f.Profile != null)
            {
                Debug.Log(f.FriendPlayFabId + "/" + f.Profile.DisplayName);
            }
        });
    }

    void DisplayPlayFabError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    void DisplayError(string error)
    {
        Debug.LogError(error);
    }

    List<FriendInfo> _friends = null;

    public void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, 
        result =>
        {
            _friends = result.Friends;
            DisplayFriends(_friends);
        }, 
        DisplayPlayFabError
        );
    }

    enum FriendIDType { PlayFabID, Username, Email, DisplayName};

    void AddFriend(FriendIDType idType, string friendID)
    {
        var request = new AddFriendRequest();
        switch(idType)
        {
            case FriendIDType.PlayFabID:
                request.FriendPlayFabId = friendID;
                break;
            case FriendIDType.Username:
                request.FriendUsername = friendID;
                break;
            case FriendIDType.Email:
                request.FriendEmail = friendID;
                break;
            case FriendIDType.DisplayName:
                request.FriendTitleDisplayName = friendID;
                break;
        }

        //Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, AddFriendResult, DisplayPlayFabError);
    }

    void AddFriendResult(AddFriendResult r)
    {
        UpdateFriendsPanel();
        Debug.Log("Friend added succesfully!");
    }

    //Add friend base on display name
    public void OnAddFriend()
    {
        AddFriend(FriendIDType.DisplayName, addFriendInput.text);
    }

    //Remove friend only takes PlayFabID
    //You can get it from FriendInfo object under FriendPlayFabId
    void RemoveFriend(FriendInfo friendInfo)
    {
        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
        {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        },
        result =>
        {
            _friends.Remove(friendInfo);
        }, 
        DisplayPlayFabError);
    }

    public void OnUnFriend()
    {
        //RemoveFriend(removeFriendInput.text);
        RemoveFriendByName(removeFriendInput.text);
    }

    void RemoveFriend(string pfid)
    {
        var req = new RemoveFriendRequest
        {
            FriendPlayFabId = pfid
        };

        PlayFabClientAPI.RemoveFriend(req, RemoveFriendResult, DisplayPlayFabError);
    }

    void RemoveFriendResult(RemoveFriendResult r)
    {
        UpdateFriendsPanel();
        Debug.Log("Unfriend!");
    }

    public void OnGetFriendLB()
    {
        PlayFabClientAPI.GetFriendLeaderboard(
            new GetFriendLeaderboardRequest { StatisticName = "Highscore", MaxResultsCount = 10 },
            r =>
            {
                leaderboardText.text = "Friends Leaderboard: \n";
                foreach (var item in r.Leaderboard)
                {
                    //string onerow = item.Position + "/" + item.PlayFabId + "/" + item.DisplayName + "/" + item.StatValue + "\n";
                    //Debug.Log(onerow);
                    //leaderboardText.text += onerow;
                    string onerow = (item.Position + 1) + " - " + item.DisplayName + " - " + item.StatValue + "\n";
                    Debug.Log(onerow);
                    leaderboardText.text += onerow;
                }
            },
            DisplayPlayFabError);
    }

    //Scene changer
    public void GoToScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    //public void OnButtonGetLeaderboard()
    //{
    //    var lbreq = new GetLeaderboardRequest
    //    {
    //        StatisticName = "Highscore", //Playfab leaderboard name
    //        StartPosition = 0,
    //        MaxResultsCount = 10
    //    };

    //    PlayFabClientAPI.GetLeaderboard(lbreq, OnLeaderboardGet, OnError);
    //}

    //void OnLeaderboardGet(GetLeaderboardResult r)
    //{
    //    string LeaderboardStr = "Leaderboard\n";

    //    foreach (var item in r.Leaderboard)
    //    {
    //        string onerow = (item.Position + 1) + " - " + item.DisplayName + " - " + item.StatValue + "\n";
    //        LeaderboardStr += onerow; //Combine all into one string
    //    }

    //    leaderboardText.text = LeaderboardStr;
    //}
}
