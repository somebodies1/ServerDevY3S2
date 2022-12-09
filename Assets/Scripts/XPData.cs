using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XP
{
    public int xpAmt;
    public XP(int _xp)
    {
        xpAmt = _xp;
    }
}

public class XPData : MonoBehaviour
{
    public LevelData levelDataScript;

    [SerializeField] TMP_Text xpTMP;
    public static XP clientXP = new XP(0);

    private void Start()
    {
        //get clientxp at start
        LoadJSON();
    }

    public XP ReturnClass()
    {
        return clientXP;
    }

    public void AddToXP(int _xpAmt)
    {
        clientXP.xpAmt += _xpAmt;
        SendJSON();

        levelDataScript.AddXPToClientLevel(clientXP.xpAmt);
        SetUI();
    }

    public void SetXP(XP _xp)
    {
        clientXP = _xp;
    }

    public void SetUI()
    {
        UpdateXPTMP();
    }

    public void UpdateXPTMP()
    {
        if (xpTMP)
        {
            xpTMP.text = "XP: ";
            xpTMP.text += clientXP.xpAmt.ToString();
        }
    }

    public void SendJSON()
    {
        string xpAsJson = JsonUtility.ToJson(clientXP);
        Debug.Log("JSON data prepared: " + xpAsJson);

        var req = new UpdateUserDataRequest
        {
            //Package as dictionary item
            Data = new Dictionary<string, string>
            {
                {"XP", xpAsJson}
            }
        };

        PlayFabClientAPI.UpdateUserData(req, result => Debug.Log("Data sent success!"), OnError);
    }

    public void LoadJSON()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnJSONDataReceived, OnError);
    }

    void OnJSONDataReceived(GetUserDataResult r)
    {
        Debug.Log("Received JSON data");

        if (r.Data != null && r.Data.ContainsKey("XP"))
        {
            Debug.Log(r.Data["XP"].Value);

            XP xp = JsonUtility.FromJson<XP>(r.Data["XP"].Value);

            SetXP(xp);
            SetUI();
        }
        else
        {
            clientXP = new XP(0);
        }

        SetUI();
    }

    void OnError(PlayFabError e)
    {
        Debug.Log("Error" + e.GenerateErrorReport());
    }
}
