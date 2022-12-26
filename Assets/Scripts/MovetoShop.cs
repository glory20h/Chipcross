using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovetoShop : MonoBehaviour
{
    public void LoadShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void MainBoard()
    {
        SceneManager.LoadScene("MainBoard");
    }
}
