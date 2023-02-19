using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelectionUI : MonoBehaviour
{
    public SkinManager skinManager;
    public Image skinPreviewImage;
    public Button nextButton;
    public Button previousButton;

    private List<Skin> skins = new List<Skin>();
    private int currentIndex = 0;

    private void Start()
    {
        // 스킨 리스트를 가져온다
        skins = skinManager.GetUnlockedSkins();
        Debug.Log(skins);
        // UI를 업데이트한다
        UpdateUI();
    }


    public void Next()
    {
        currentIndex += 4;
        if (currentIndex >= skins.Count)
        {
            currentIndex = 0;
        }
        UpdateUI();
    }

    public void Previous()
    {
        currentIndex -= 4;
        if (currentIndex < 0)
        {
            currentIndex = (skins.Count / 4) * 4;
            if (currentIndex == skins.Count)
            {
                currentIndex -= 4;
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < 4; i++)
        {
            int index = (currentIndex + i) % skins.Count;
            // 해당 스킨이 언락되어 있는 경우에만 활성화하도록 합니다.
            if (i < transform.childCount)
            {
                transform.GetChild(i).gameObject.SetActive(skinManager.IsUnlocked(index));
                transform.GetChild(i).GetComponent<Image>().sprite = skins[index].sprite;
            }
        }

        skinPreviewImage.sprite = skins[currentIndex].sprite;

        // 다음 버튼과 이전 버튼이 활성화되어 있는지 확인합니다.
        nextButton.interactable = currentIndex < skins.Count - 4;
        previousButton.interactable = currentIndex >= 4;
    }

}
