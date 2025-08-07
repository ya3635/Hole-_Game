
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    #region Singleton class : UIManager
    
    void Awake()
    {
        if (ReferenceEquals(Instance, null)) 
            Instance = this;
    }
    #endregion
    
    [Header("Level Progress UI")]
    [SerializeField] int sceneOffset;
    [SerializeField] TMP_Text nextLevelText;
    [SerializeField]  TMP_Text currentLevelText;
    [FormerlySerializedAs("progressFIllımage")]
    [FormerlySerializedAs("progressFillımage")] 
    [SerializeField] private Image progressFillImage;
    
    [Space]
    [SerializeField] TMP_Text levelCompleteText;

    [Space] [SerializeField] private Image fadePanel;
    
    
    void Start()
    {
        FadeAtStart();
        progressFillImage.fillAmount = 0f;
        SetLevelProgressText();
    }

    void SetLevelProgressText()
    {
        int level = SceneManager.GetActiveScene().buildIndex + sceneOffset;    
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
        
    }

    
      public void UpdateLevelProgress()
    {
        
        float val = 1f - ((float)Level.Instance.objectsInScene / Level.Instance.totalObjects);
        
        progressFillImage.DOFillAmount(val, 0.4f);
    }

    public void ShowLevelCompleteUI()
    {
        levelCompleteText.DOFade(1f, 1f).From(0f);
    }

    private void FadeAtStart()
    {
        fadePanel.DOFade(0f, 1.3f).From(1f);
    }
      
}
