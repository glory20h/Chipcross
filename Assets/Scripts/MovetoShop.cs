using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovetoShop : MonoBehaviour
{
    public void LoadShop()
    {
        SceneManager.LoadSceneAsync("ShopScene", LoadSceneMode.Additive);
    }

    public void MainBoard()
    {
        SceneManager.UnloadSceneAsync("ShopScene");
    }
}
