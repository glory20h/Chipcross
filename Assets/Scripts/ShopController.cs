using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Skin
{
    public int cost;
    public Sprite sprite;
    public string name;
}

public class ShopController : MonoBehaviour
{
    [SerializeField] private Image selectedSkin;
    [SerializeField] private Text coinsText;
    [SerializeField] private SkinManager skinManager;
    [SerializeField] private Button nextSkinButton;
    [SerializeField] private Button previousSkinButton;
    [SerializeField] private Transform shopItems;

    Button curBtn;

    void Start()
    {
        coinsText.text = "Coins: " + PlayerPrefs.GetInt("Coins");
        selectedSkin.sprite = skinManager.GetSelectedSkin().sprite;

        /*
        nextSkinButton.onClick.AddListener(SelectNextSkin);
        previousSkinButton.onClick.AddListener(SelectPreviousSkin);
        */

        foreach (Transform item in shopItems)
        {
            SetupItem(item);
        }
    }

    void SetupItem(Transform item)
    {
        int skinIndex = item.GetSiblingIndex();
        Button skinImg = item.GetComponent<Button>();
        Button buyButton = item.GetChild(0).GetComponent<Button>();
        Text costText = buyButton.transform.GetChild(0).GetComponent<Text>();

        Skin skin = skinManager.skins[skinIndex];
        item.GetComponent<Image>().sprite = skin.sprite;

        if (skinManager.IsUnlocked(skinIndex))
        {
            costText.text = "0";
        }
        else
        {
            costText.text = skin.cost.ToString();
        }
        
        if (skinIndex == PlayerPrefs.GetInt("SelectedSkin", 0))
        {
            buyButton.interactable = false;
            curBtn = buyButton;
        }
        else
        {
            buyButton.interactable = true;
        }

        skinImg.onClick.AddListener(() => OnSkinPressed(skinIndex));
        buyButton.onClick.AddListener(() => OnBuyButtonPressed(skinIndex, buyButton, costText));
    }

    public void OnSkinPressed(int skinIndex)
    {
        Skin skin = skinManager.skins[skinIndex];

        // Display skin info
        selectedSkin.sprite = skin.sprite;
    }

    public void OnBuyButtonPressed(int skinIndex, Button buyButton, Text costText)
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        Skin skin = skinManager.skins[skinIndex];

        // If skin is already unlocked
        if (skinManager.IsUnlocked(skinIndex))
        {
            skinManager.SelectSkin(skinIndex);
            buyButton.interactable = false;
            curBtn.interactable = true;
            curBtn = buyButton;
            return;
        }

        // Unlock the skin
        if (coins >= skin.cost)
        {
            coins = coins - skin.cost;
            coinsText.text = "Coins: " + coins;
            PlayerPrefs.SetInt("Coins", coins);
            costText.text = "0";

            skinManager.Unlock(skinIndex);
            skinManager.SelectSkin(skinIndex);
            buyButton.interactable = false;
            curBtn.interactable = true;
            curBtn = buyButton;
        }
        else
        {
            Debug.Log("Not enough coins :(");
        }
    }

    /*
    public void SelectNextSkin()
    {
        foreach (var item in shopItems)
        {
            item.Next();
        }
    }

    public void SelectPreviousSkin()
    {
        foreach (var item in shopItems)
        {
            item.Previous();
        }
    }
    */
}
