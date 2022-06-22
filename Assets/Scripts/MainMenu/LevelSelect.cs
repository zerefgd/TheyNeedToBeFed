using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private int _currentLevel;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private GameObject _activeObject, _perfectObject;

    [SerializeField]
    private TMP_Text _levelText;

    [SerializeField]
    private List<Image> _diamonds;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private AnimationClip _startClip, _clickClip;

    private int totalDiamonds, acquiredDiamonds;

    private void OnEnable()
    {
        _levelText.text = _currentLevel.ToString();

        int currentStage = MainMenuManager.Instance.CurrentStage;
        LevelUIData data = MainMenuManager.Instance.Stages[currentStage - 1].Levels[_currentLevel - 1];
        totalDiamonds = data.TotalDiamonds;
        string levelName = Constants.Data.LEVEL_NAME + "_" + currentStage.ToString() + "_" + _currentLevel.ToString();
        acquiredDiamonds = PlayerPrefs.HasKey(levelName) ? PlayerPrefs.GetInt(levelName) : 0;

        string levelPerfect = levelName + "_" + Constants.Data.PERFECT;
        bool isPerfect = (PlayerPrefs.HasKey(levelPerfect) ? PlayerPrefs.GetInt(levelPerfect) : 0) == 1;
        _perfectObject.SetActive(isPerfect);

        for (int i = 0; i < _diamonds.Count; i++)
        {
            _diamonds[i].gameObject.SetActive(false);
        }

        _activeObject.SetActive(false);
        if(_currentLevel == 8)
        {
            _activeObject.SetActive(MainMenuManager.Instance.Diamonds < 99);
        }

        _button.onClick.AddListener(ClickedLevel);

        StartCoroutine(StartAnimation());
    }

    private void ClickedLevel()
    {
        if (_activeObject.activeInHierarchy) return;
        StartCoroutine(IClickedLevel());
    }

    private IEnumerator StartAnimation()
    {
        _animator.Play(_startClip.name);
        yield return new WaitForSeconds(_startClip.length);

        for (int i = 0; i < totalDiamonds; i++)
        {
            _diamonds[i].gameObject.SetActive(true);
            _diamonds[i].color = i < acquiredDiamonds ? Color.white : Color.black;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator IClickedLevel()
    {
        _animator.Play(_clickClip.name);
        yield return new WaitForSeconds(_clickClip.length);

        PlayerPrefs.SetInt(Constants.Data.CURRENT_LEVEL, _currentLevel);
        int currentStage = MainMenuManager.Instance.CurrentStage;
        string levelName = Constants.Data.LEVEL_NAME + "_" + currentStage.ToString() + "_" + _currentLevel.ToString();
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

}
