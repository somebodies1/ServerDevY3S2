using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject inventoryPanel;

    public void OnButtonActiveInventoryPanel(bool _bool)
    {
        inventoryPanel.SetActive(_bool);
    }

    public void OnButtonActiveShopPanel(bool _bool)
    {
        shopPanel.SetActive(_bool);
    }
}
