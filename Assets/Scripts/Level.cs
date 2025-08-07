
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Level : MonoBehaviour

{
    #region Singleton class : Level
    public static Level Instance;

    void Awake()
    {
        if (Instance == null)
         Instance = this;
    }
    

    #endregion
    public Transform objectsParent; 
    public int objectsInScene;
    public int totalObjects;
    
    [Space]
    [Header("Level Objects & Obstacles")]
    [SerializeField] Material obstacleMaterial;
    [SerializeField] Material objectMaterial;
    [SerializeField] Material groundMaterial;
    [SerializeField] Image progressFillImage;

    [SerializeField] SpriteRenderer bgFadeSprite;
    
    [Space]
    [Header("Level Colors")]
    [Header("Ground")]
    [SerializeField] Color groundColor;
    
    [Header("Objects & Obstacles")]
    [SerializeField] Color objectColor;
    [SerializeField] Color obstaclesColor; 
    
    [Header("UI (progress")]
    [SerializeField] Color progressFillColor;
    
    [Header("Background")]
    [SerializeField] Color cameraColor;
    [SerializeField] Color fadeColor;
    
     void Start()
     { 
         SetRandomColors();
         UpdateLevelColors();
         CountObjects();

    }
    void CountObjects()
    {
        totalObjects = objectsParent.childCount;
        objectsInScene = totalObjects;
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

    void UpdateLevelColors()
    {
        if (!ReferenceEquals(groundMaterial, null))
        {
            groundMaterial.color = groundColor;
        }
        if (!ReferenceEquals(obstacleMaterial, null))
        {
            obstacleMaterial.color = obstaclesColor;
        }
        if (!ReferenceEquals(objectMaterial, null))
        {
            objectMaterial.color = objectColor;
        }
        if (!ReferenceEquals(progressFillImage, null))
        {
            progressFillImage.color = progressFillColor;
        }
        if (!ReferenceEquals(bgFadeSprite, null))
        {
            bgFadeSprite.color = fadeColor;
        }
        if (Camera.main != null)
        {
            Camera.main.backgroundColor = cameraColor;
        }
    }
    void OnValidate()
    {
        UpdateLevelColors();
    }
    public void SetRandomColors()
    {
        float baseHue = Random.Range(0f, 1f);
        float objectHue = baseHue;
        float obstacleHue = (baseHue + Random.Range(0.08f, 0.25f)) % 1f; 
        
        objectColor = Color.HSVToRGB(objectHue, 0.7f, 0.9f);
        obstaclesColor = Color.HSVToRGB(obstacleHue, 0.75f, 0.6f);
        
        groundColor = Color.HSVToRGB(baseHue, 0.3f, 0.95f); 
        cameraColor = Color.HSVToRGB(baseHue, 0.2f, 0.9f);
        fadeColor = Color.HSVToRGB(baseHue, 0.2f, 0.6f);

        
        progressFillColor = objectColor;
    }
    
}


