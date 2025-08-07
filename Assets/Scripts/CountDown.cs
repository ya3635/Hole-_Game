using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    [SerializeField] private Image _timer;
    [SerializeField] private Text _timeText;
    [SerializeField] private float _currentTime;
    [SerializeField] private float _duration;
    void Start()
    {
        _currentTime = _duration;
        _timeText.text = _currentTime.ToString();
        StartCoroutine(CountdownTime());
    }

    private IEnumerator CountdownTime () {
        while(_currentTime >= 0) {
            _timer.fillAmount = Mathf.InverseLerp(0, _duration, _currentTime);
            _timeText.text = _currentTime.ToString();
            yield return new WaitForSeconds(1f);
            _currentTime--;
        }
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);

        
        SceneManager.LoadScene(2);

    }   
    
    
}
