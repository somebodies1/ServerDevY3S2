using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level
{
    public int levelInt;

    public Level()
    {
        levelInt = 1;
    }

    public Level(int _lvl)
    {
        levelInt = _lvl;
    }
}

public class LevelData : MonoBehaviour
{
    public XPData xpDataScript;

    [SerializeField] TMP_Text lvlTMP;
    public static Level clientLevel = new Level(1);

    private void Start()
    {
        //get clientLevel at start
        LoadJSON();
    }

    public Level ReturnClass()
    {
        return clientLevel;
    }

    public void AddXPToClientLevel(int _xp)
    {
        int newLvl = ConvertXPToLevel(_xp) + new Level().levelInt;
        SetLevel(new Level(newLvl));
        SendJSON();
        SetUI();
    }

    public void AddToLevel(int _lvlInt)
    {
        clientLevel.levelInt += _lvlInt;
        SendJSON();
        SetUI();
    }

    public void SetLevel(Level _lvl)
    {
        clientLevel = _lvl;
    }

    public void SetUI()
    {
        UpdateLevelTMP();
    }

    public void UpdateLevelTMP()
    {
        if (lvlTMP)
        {
            lvlTMP.text = "Level: ";
            lvlTMP.text += clientLevel.levelInt.ToString();
        }
    }

    public int ConvertXPToLevel(int _xp)
    {
        int lvlAmt = _xp / 100;

        Debug.Log("Converted Level" + lvlAmt);

        return lvlAmt;
    }

    public void SendJSON()
    {

        string lvlAsJson = JsonUtility.ToJson(clientLevel);
        Debug.Log("JSON data prepared: " + lvlAsJson);

        var req = new UpdateUserDataRequest
        {
            //Package as dictionary item
            Data = new Dictionary<string, string>
            {
                {"Level", lvlAsJson}
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

        if (r.Data != null && r.Data.ContainsKey("Level"))
        {
            Debug.Log(r.Data["Level"].Value);

            Level lvl = JsonUtility.FromJson<Level>(r.Data["Level"].Value);

            SetLevel(lvl);
            SetUI();
        }
        else
        {
            clientLevel = new Level(ConvertXPToLevel(xpDataScript.ReturnClass().xpAmt) + new Level().levelInt);
        }

        Debug.Log("ClientLevel: " + clientLevel.levelInt);
        SetUI();
    }

    void OnError(PlayFabError e)
    {
        Debug.Log("Error" + e.GenerateErrorReport());
    }
}
