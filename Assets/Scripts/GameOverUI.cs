using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameOverUI : MonoBehaviour
{
    public  static void RetryLastLevel()
    {
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            int lastLevelIndex = PlayerPrefs.GetInt("LastLevel");
            SceneManager.LoadScene(lastLevelIndex);
        }
        else
        {
            Debug.LogWarning("LastLevel bilgisi bulunamadı. Ana menüye dönülüyor.");
            SceneManager.LoadScene(1); // Yedek olarak ana menü
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
