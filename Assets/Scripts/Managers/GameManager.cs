using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player;

    [SerializeField]
    private List<GameObject> _diamonds, _endDiamonds;

    [SerializeField]
    private LevelUIData _levelData;

    [SerializeField]
    private Transform _camera;

    [SerializeField]
    private float _cameraSpeed;

    [SerializeField]
    private GameObject _endPanel, _perfectObject;

    [SerializeField]
    private TMP_Text _retryText;

    private int currentDiamonds;
    private int totalDiamonds;


    private void Awake()
    {
        Instance = this;

        currentDiamonds = 0;
        totalDiamonds = _levelData.TotalDiamonds;

        _endPanel.SetActive(false);

        for (int i = 0; i < _diamonds.Count; i++)
        {
            _diamonds[i].SetActive(false);
        }

        for (int i = 0; i < totalDiamonds; i++)
        {
            _diamonds[i].SetActive(true);
            _diamonds[i].GetComponent<Image>().color = Color.black;
        }


    }

    public void UpdateDiamond()
    {
        _diamonds[currentDiamonds].GetComponent<Image>().color = Color.white;
        currentDiamonds++;        
    }

    public void EndGame()
    {
        _endPanel.SetActive(true);

        int currentStage = PlayerPrefs.GetInt(Constants.Data.CURRENT_STAGE);
        int currentLevel = PlayerPrefs.GetInt(Constants.Data.CURRENT_LEVEL);

        string levelName = Constants.Data.LEVEL_NAME + "_" + currentStage.ToString() + "_" + currentLevel.ToString();
        int currentScore = PlayerPrefs.HasKey(levelName) ? PlayerPrefs.GetInt(levelName) : 0;

        string retryKey = levelName + "_" + Constants.Data.RETRY;
        int numOfRetries = PlayerPrefs.HasKey(retryKey) ? PlayerPrefs.GetInt(retryKey) : 0;
        _retryText.text = "retries : " + numOfRetries.ToString();


        string perfectKey = levelName + "_" + Constants.Data.PERFECT;
        if(numOfRetries == 0 && currentDiamonds == totalDiamonds)
        {
            PlayerPrefs.SetInt(perfectKey, 1);
        }
        _perfectObject.SetActive((PlayerPrefs.HasKey(Constants.Data.PERFECT) 
            ? PlayerPrefs.GetInt(perfectKey) : 0) == 1);

        for (int i = 0; i < _endDiamonds.Count; i++)
        {
            _endDiamonds[i].SetActive(false);
        }

        for (int i = 0; i < totalDiamonds; i++)
        {
            _endDiamonds[i].SetActive(true);
            _endDiamonds[i].GetComponent<Image>().color = Color.black;
        }

        for (int i = 0; i < currentDiamonds; i++)
        {
            _endDiamonds[i].GetComponent<Image>().color = Color.white;
        }

        if(currentDiamonds > currentScore)
        {
            PlayerPrefs.SetInt(levelName, currentDiamonds);
        }

        //Diamonds
        int diamondsIncrease = currentDiamonds - currentScore;
        if(diamondsIncrease > 0)
        {
            int diamonds = PlayerPrefs.GetInt(Constants.Data.DIAMONDS) + diamondsIncrease;
            PlayerPrefs.SetInt(Constants.Data.DIAMONDS, diamonds);
        }
    }

    public void PlayNextLevel()
    {
        int currentStage = PlayerPrefs.GetInt(Constants.Data.CURRENT_STAGE);
        int currentLevel = PlayerPrefs.GetInt(Constants.Data.CURRENT_LEVEL);

        if(currentLevel == 7)
        {
            GoToMainMenu();
            return;
        }

        currentLevel++;
        PlayerPrefs.SetInt(Constants.Data.CURRENT_LEVEL, currentLevel);
        string levelName = Constants.Data.LEVEL_NAME + "_" + currentStage.ToString() + "_" + currentLevel.ToString();

        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.Data.MAIN_MENU);
    }

    public void RestartGame()
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        string retryKey = levelName + "_" + Constants.Data.RETRY;
        int numOfRetries = PlayerPrefs.HasKey(retryKey) ? PlayerPrefs.GetInt(retryKey) : 0;
        numOfRetries++;
        PlayerPrefs.SetInt(retryKey, numOfRetries);

        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

    public void MoveCamera(Vector3 targetPos)
    {
        StartCoroutine(IMoveCamera(targetPos));
    }

    private IEnumerator IMoveCamera(Vector3 targetPos)
    {
        Vector3 target = targetPos;
        target.z = _camera.position.z;

        while(Vector3.Distance(target,_camera.position) > 0.05)
        {
            _camera.position = Vector3.MoveTowards(_camera.position, target, _cameraSpeed * Time.deltaTime);
            yield return null;
        }

        _camera.position = target;

    }
}
