using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Image selectedSkin;
    [SerializeField] private Text coinsText;
    [SerializeField] private SkinManager skinManager;
    public Button nextSkinButton;
    public Button previousSkinButton;
    public List<SkinShopItem> shopItems;
    [SerializeField] public GameObject shopItemPrefab;
    [SerializeField] public GameObject shopItemPrefab1;
    [SerializeField] public GameObject shopItemPrefab2;
    [SerializeField] public GameObject shopItemPrefab3;

    void Start()
    {
        nextSkinButton.onClick.AddListener(SelectNextSkin);
        previousSkinButton.onClick.AddListener(SelectPreviousSkin);

        // 원하는 SkinShopItem 게임오브젝트들을 넣어줍니다.
        shopItems = new List<SkinShopItem>();
        shopItems.Add(shopItemPrefab.GetComponent<SkinShopItem>());
        shopItems.Add(shopItemPrefab1.GetComponent<SkinShopItem>());
        shopItems.Add(shopItemPrefab2.GetComponent<SkinShopItem>());
        shopItems.Add(shopItemPrefab3.GetComponent<SkinShopItem>());
    }

    void Update()
    {
        coinsText.text = "Coins: " + PlayerPrefs.GetInt("Coins");
        selectedSkin.sprite = skinManager.GetSelectedSkin().sprite;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

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


}
