using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinManager", menuName = "Skin Manager")]
public class SkinManager : ScriptableObject
{
    [SerializeField] public Skin[] skins;
    private const string Prefix = "Skin_";
    private const string SelectedSkin = "SelectedSkin";

    public void SelectSkin(int skinIndex)
    {
        PlayerPrefs.SetInt(SelectedSkin, skinIndex);
        PlayerPrefs.Save();
    }

    public Skin GetSelectedSkin()
    {
        int skinIndex = PlayerPrefs.GetInt(SelectedSkin, 0);
        if (skinIndex >= 0 && skinIndex < skins.Length)
        {
            return skins[skinIndex];
        }
        else
        {
            return null;
        }
    }

    public void Unlock(int skinIndex)
    {
        PlayerPrefs.SetInt(Prefix + skinIndex, 1);
        PlayerPrefs.Save();
    }

    public bool IsUnlocked(int skinIndex)
    {
        return PlayerPrefs.GetInt(Prefix + skinIndex, 0) == 1;
    }

    public List<Skin> GetUnlockedSkins()
    {
        List<Skin> unlockedSkins = new List<Skin>();
        for (int i = 0; i < skins.Length; i++)
        {
            if (IsUnlocked(i))
            {
                unlockedSkins.Add(skins[i]);
            }
        }
        return unlockedSkins;
    }

    public List<Skin> GetNextPage(int pageSize, int pageIndex)
    {
        List<Skin> unlockedSkins = GetUnlockedSkins();
        List<Skin> pageSkins = new List<Skin>();
        int startIndex = pageSize * pageIndex;
        int endIndex = startIndex + pageSize;
        for (int i = startIndex; i < endIndex && i < unlockedSkins.Count; i++)
        {
            pageSkins.Add(unlockedSkins[i]);
        }
        return pageSkins;
    }
}
