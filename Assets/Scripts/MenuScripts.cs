
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScripts : MonoBehaviour
{
    public void PlayButton()
    {
      SceneManager.LoadScene(3);  
    }

    public void QuitButton()
    {
       Application.Quit(); 
    }
}
