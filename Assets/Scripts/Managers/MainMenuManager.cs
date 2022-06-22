using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set;}

    public List<StageUIData> Stages;

    [SerializeField]
    private GameObject _mainPanel, _stagePanel, _levelPanel, _activeSoundButton;

    [SerializeField]
    private AnimationClip _clickClip;

    [SerializeField]
    private Animator _quitAnimator, _playAnimator, _howToPlayAnimator;

    [SerializeField]
    private TMP_Text _levelsDiamondText, _stageDiamondsText, _currentStageText;

    [SerializeField]
    private Image _levelBGImage;

    public int Diamonds { get; private set; }

    public int CurrentStage
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.Data.CURRENT_STAGE);
        }

        set
        {
            PlayerPrefs.SetInt(Constants.Data.CURRENT_STAGE,value);
        }
    }

    private void Awake()
    {
        Instance = this;

        _mainPanel.SetActive(true);
        _stagePanel.SetActive(false);
        _levelPanel.SetActive(false);

        Diamonds = PlayerPrefs.HasKey(Constants.Data.DIAMONDS) ? PlayerPrefs.GetInt(Constants.Data.DIAMONDS) : 0;


        bool sound = (PlayerPrefs.HasKey(Constants.Data.SETTINGS_SOUND) ?
            PlayerPrefs.GetInt(Constants.Data.SETTINGS_SOUND) : 1) == 1;
        _activeSoundButton.SetActive(!sound);
    }

    private void Start()
    {
        AudioManager.Instance.AddButtonSound();
    }

    public void GameQuit()
    {
        StartCoroutine(IGameQuit());
    }


    public void ClickedPlay()
    {
        StartCoroutine(IClickedPlay());
    }

    public void ToggleSound()
    {
        bool sound = (PlayerPrefs.HasKey(Constants.Data.SETTINGS_SOUND) ?
           PlayerPrefs.GetInt(Constants.Data.SETTINGS_SOUND) : 1) == 1;
        sound = !sound;
        PlayerPrefs.SetInt(Constants.Data.SETTINGS_SOUND, sound ? 1 : 0);
        _activeSoundButton.SetActive(!sound);
        AudioManager.Instance.ToggleSound();
    }

    public void BackToMenu()
    {
        _stagePanel.SetActive(false);
        _mainPanel.SetActive(true);
    }

    public void ClickedHowToPlay()
    {
        StartCoroutine(IClickedHowToPlay());
    }

    public void ClickedStage()
    {
        _stagePanel.SetActive(false);
        _levelPanel.SetActive(true);
        _levelsDiamondText.text = "X " + Diamonds.ToString();
        _currentStageText.text = "world " + CurrentStage.ToString();
        _levelBGImage.sprite = Stages[CurrentStage - 1].CurrentImage;
    }

    public void BackToStage()
    {
        _levelPanel.SetActive(false);
        _stagePanel.SetActive(true);
    }

    private IEnumerator IClickedPlay()
    {
        float animTime = _clickClip.length;
        _playAnimator.Play(_clickClip.name);
        yield return new WaitForSeconds(animTime);

        _mainPanel.SetActive(false);
        _stagePanel.SetActive(true);

        _stageDiamondsText.text = "X " + Diamonds.ToString();
    }

    private IEnumerator IGameQuit()
    {
        float animTime = _clickClip.length;
        _quitAnimator.Play(_clickClip.name);
        yield return new WaitForSeconds(animTime);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator IClickedHowToPlay()
    {
        float animTime = _clickClip.length;
        _howToPlayAnimator.Play(_clickClip.name);
        yield return new WaitForSeconds(animTime);


        UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.Data.HOW_TO_PLAY);
    }
}
