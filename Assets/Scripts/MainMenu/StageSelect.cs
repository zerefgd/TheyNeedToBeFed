using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    private int _currentStage;

    [SerializeField]
    private GameObject _inActiveObject;

    [SerializeField]
    private Image _bgImage;

    [SerializeField]
    private TMP_Text _countText, _diamondsText;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private AnimationClip _clickClip;

    private bool isActive;

    private void OnEnable()
    {
        StageUIData data = MainMenuManager.Instance.Stages[_currentStage - 1];

        int currentDiamonds = MainMenuManager.Instance.Diamonds;
        isActive = currentDiamonds >= data.DiamondsRequired;

        if(isActive)
        {
            _bgImage.sprite = data.CurrentImage;
            _countText.text = "world " + _currentStage.ToString();
            _inActiveObject.SetActive(false);
        }
        else
        {
            _diamondsText.text = "X " + data.DiamondsRequired.ToString();
            _inActiveObject.SetActive(true);
        }

        _button.onClick.AddListener(ClickedButton);
    }

    private void ClickedButton()
    {
        if (!isActive) return;
        StartCoroutine(IClicked());
    }

    private IEnumerator IClicked()
    {
        _animator.Play(_clickClip.name);
        yield return new WaitForSeconds(_clickClip.length);

        MainMenuManager.Instance.CurrentStage = _currentStage;
        MainMenuManager.Instance.ClickedStage();
    }
}
