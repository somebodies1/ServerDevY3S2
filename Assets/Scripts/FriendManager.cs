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

    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        friendListText.text = "";

        friendsCache.ForEach(f =>
        {
            Debug.Log(f.FriendPlayFabId + "," + f.TitleDisplayName);
            friendListText.text += f.TitleDisplayName + "\n";
            if (f.Profile == null)
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
        PlayFabClientAPI.AddFriend(request, result =>
        {
            Debug.Log("Friend added succesfully!");
        }, DisplayPlayFabError);
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
        RemoveFriend(removeFriendInput.text);
    }

    void RemoveFriend(string pfid)
    {
        var req = new RemoveFriendRequest
        {
            FriendPlayFabId = pfid
        };

        PlayFabClientAPI.RemoveFriend(req, 
            result => 
            { Debug.Log("Unfriend!"); }, 
            DisplayPlayFabError);
    }

    public void OnGetFriendLB()
    {
        PlayFabClientAPI.GetFriendLeaderboard(
            new GetFriendLeaderboardRequest { StatisticName = "Highscore", MaxResultsCount = 10 },
            r =>
            {
                leaderboardText.text = "Friends LB \n";
                foreach (var item in r.Leaderboard)
                {
                    string onerow = item.Position + "/" + item.PlayFabId + "/" + item.DisplayName + "/" + item.StatValue + "\n";
                    Debug.Log(onerow);
                    leaderboardText.text += onerow;
                }
            },
            DisplayPlayFabError);
    }
}
