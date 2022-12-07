using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Msg, coinTMP, inventoryTMP;

    private void Start()
    {
        UpdateCoinTMPAmount();
    }

    //Display in console and message box
    void UpdateMsg(string msg)
    {
        Debug.Log(msg);
        //Msg.text += msg +'\n';
    }

    void UpdateCoinTMP(string _text)
    {
        if (coinTMP)
            coinTMP.text = "Coins: " + _text;

        Debug.Log("CoinTMP: " + _text);
    }

    void UpdateInventoryTMP(string _text)
    {
        if (inventoryTMP)
            inventoryTMP.text = _text;

        Debug.Log("InvTMP: " + _text);
    }

    public void UpdateCoinTMPAmount()
    {
        int currencyAmt = 0;
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            r =>
            {
                currencyAmt = r.VirtualCurrency["CN"];
                UpdateCoinTMP(currencyAmt.ToString());
            }, OnError);
    }

    void OnError(PlayFabError e)
    {
        UpdateMsg(e.GenerateErrorReport());
    }

    public void LoadScene(string scn)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scn);
    }

    //public void GetVirtualCurrencies()
    //{
    //    PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
    //        r =>
    //        {
    //            int coins = r.VirtualCurrency["CN"];
    //        },OnError);
    //}

    public int GetVirtualCurrencies()
    {
        int currencyAmt = 0;
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            r =>
            {
                currencyAmt = r.VirtualCurrency["CN"];
                //UpdateCoinTMP(currencyAmt.ToString());
            }, OnError);

        return currencyAmt;
    }

    public void OnButtonSetVirtualCurrencies(int _amount)
    {
        SetVirtualCurrencies(_amount);
    }

    void SetVirtualCurrencies(int _amount)
    {
        var req = new AddUserVirtualCurrencyRequest
        {
            Amount = _amount,
            VirtualCurrency = "CN",
        };

        PlayFabClientAPI.AddUserVirtualCurrency(req, OnSetVirtualCurrency, OnError);
    }

    void OnSetVirtualCurrency(ModifyUserVirtualCurrencyResult r)
    {
        Debug.Log("Updated currency: " + r.Balance);
    }

    public void GetCatalog()
    {
        var catreq = new GetCatalogItemsRequest
        {
            CatalogVersion = "catalogname"
        };

        PlayFabClientAPI.GetCatalogItems(catreq,
            result =>
            {
                List<CatalogItem> items = result.Catalog;
                UpdateMsg("Catalog Items");

                foreach (CatalogItem i in items)
                {
                    //CN is virtual currency type
                    UpdateMsg(i.DisplayName + "," + i.VirtualCurrencyPrices["CN"]);
                }
            }, OnError);
    }

    public int GetItemCNPrice(string _itemID)
    {
        var catreq = new GetCatalogItemsRequest
        {
            CatalogVersion = "MainCatalog"
        };

        int price = 0;

        PlayFabClientAPI.GetCatalogItems(catreq,
            result =>
            {
                List<CatalogItem> items = result.Catalog;

                foreach (CatalogItem i in items)
                {
                    if (i.ItemId == _itemID)
                    {
                        price = (int)i.VirtualCurrencyPrices["CN"];
                    } 
                }
            }, OnError);

        return price;
    }

    public void GetPlayerInventory()
    {
        var UserInv = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(UserInv,
            result =>
            {
                List<ItemInstance> ii = result.Inventory;
                UpdateMsg("Player Inventory");
                foreach (ItemInstance i in ii)
                {
                    UpdateMsg(i.DisplayName + "," + i.ItemId + "," + i.ItemInstanceId);
                }
            }, OnError);
    }

    public void OnButtonUpdatePlayerInventory()
    {
        var UserInv = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(UserInv,
            result =>
            {
                List<ItemInstance> ii = result.Inventory;
                UpdateMsg("Player Inventory");
                string msg = "";
                foreach (ItemInstance i in ii)
                {
                    //UpdateMsg(i.DisplayName + "," + i.ItemId + "," + i.ItemInstanceId);
                    msg += i.DisplayName + "\n";
                }
                UpdateInventoryTMP(msg);
            }, OnError);
    }

    //public void Buy()
    //{
    //    var buyreq = new PurchaseItemRequest
    //    {
    //        //Hardcoded - need to change
    //        CatalogVersion = "catalogname",
    //        ItemId = "Item1", //Replace with real item name
    //        VirtualCurrency = "CN",
    //        Price = 2
    //    };

    //    PlayFabClientAPI.PurchaseItem(buyreq,
    //        result => { UpdateMsg("Bought!"); }, 
    //        OnError); 
    //}

    public void OnButtonBuy(string _itemID)
    {
        BuyItem(_itemID);
    }

    void BuyItem(string _itemID, string _currency = "CN")
    {
        var buyreq = new PurchaseItemRequest
        {
            CatalogVersion = "MainCatalog",
            ItemId = _itemID, //Replace with real item name
            VirtualCurrency = _currency,
            Price = ClientItemPrice(_itemID)
        };

        PlayFabClientAPI.PurchaseItem(buyreq, BuyItemResult, OnError);
    }

    void BuyItemResult(PurchaseItemResult r)
    {
        UpdateMsg("Bought!");

        UpdateCoinTMPAmount();
    }

    int ClientItemPrice(string _itemID)
    {
        switch(_itemID)
        {
            case "CheapItem":
                return 1;
            case "ExpensiveItem":
                return 5;
        }

        return 0;
    }
}
