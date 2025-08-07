
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour
{
    private bool isInHoleArea ;
    
    
    [Header("Effects")]
    public GameObject SplashPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (!Game.İsGameOver && other.CompareTag("Water"))
        {
            if (!isInHoleArea) return;

            if (SplashPrefab == null)
            {
                Debug.LogWarning("SplashPrefab is missing or destroyed.");
                return;
            }

            if (other == null)
            {
                Debug.LogWarning("Other collider is null or destroyed.");
                return;
            }

            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Instantiate(SplashPrefab, contactPoint, Quaternion.identity);
            return;;
        }

        if (other.CompareTag("HoleArea"))
        {
            isInHoleArea = true;
            return;
        }

        if (Game.İsGameOver) return;

        string otherTag = other.tag;

        if (IsScorableObject(otherTag))
        {
            HandleScorableObject(other.gameObject);
            return;
        }

        if (otherTag.Equals("Obstacle"))
        {
            Game.İsGameOver = true;
            Camera.main.transform
                .DOShakePosition(1f, .2f, 20)
                .OnComplete(() =>
                {
                });SceneManager.LoadScene(2);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HoleArea"))
        {
            isInHoleArea = false;
        }
    }
    
    void NextLevel()
    {
        Level.Instance.LoadNextLevel();
    }

    
    void HandleScorableObject(GameObject obj)
    {
        if (obj.tag == "Processed") return; // Zaten işlendiyse çık

        int scoreToAdd = GetScoreByTag(obj.tag); // Skoru önce al!
        if (scoreToAdd <= 0) return;

        obj.tag = "Processed"; // Skor aldıktan sonra "Processed" yap!

        Level.Instance.objectsInScene--;
        UIManager.Instance.UpdateLevelProgress();
        ScoreManager.Instance.AddScore(scoreToAdd);
        Destroy(obj);

        if (Level.Instance.objectsInScene == 0)
        {
            UIManager.Instance.ShowLevelCompleteUI();
            Invoke(nameof(NextLevel), 1.5f);
        }
    }
    
    int GetScoreByTag(string tag)
    {
        switch (tag)
        {
            case "Object": return 1;
            case "Object_5": return 5;
            case "Object_10": return 10;
            default: return 0;
        }
    }
    
    bool IsScorableObject(string tag)
    {
        return tag.StartsWith("Object") && tag != "Processed"; 
    }
}

     
   

