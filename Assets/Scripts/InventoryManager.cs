using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Msg;
    
    //Display in console and message box
    void UpdateMsg(string msg)
    {
        Debug.Log(msg);
        Msg.text += msg +'\n';
    }

    void OnError(PlayFabError e)
    {
        UpdateMsg(e.GenerateErrorReport());
    }

    public void LoadScene(string scn)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scn);
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            r =>
            {
                int coins = r.VirtualCurrency["CN"];
                UpdateMsg("Coins:" + coins);

            },OnError);
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

    public void Buy()
    {
        var buyreq = new PurchaseItemRequest
        {
            //Hardcoded - need to change
            CatalogVersion = "catalogname",
            ItemId = "Item1", //Replace with real item name
            VirtualCurrency = "CN",
            Price = 2
        };

        PlayFabClientAPI.PurchaseItem(buyreq,
            result => { UpdateMsg("Bought!"); }, 
            OnError); 
    }
}
