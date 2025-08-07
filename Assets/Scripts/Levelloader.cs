using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Levelloader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    
    [SerializeField] private int sceneToLoad = 2; 
    
    void Start()
    {
        LoadLevel(sceneToLoad);
    }

    private void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        
        
        float visualProgress = 0f;
        
        

        while (!operation.isDone)
        {

            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            visualProgress += Time.deltaTime / 2f;
            visualProgress = Mathf.MoveTowards(visualProgress, targetProgress, Time.deltaTime);

            slider.value = visualProgress;

            
            if (operation.progress >= 0.9f && visualProgress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    
}
