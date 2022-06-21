using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioSource _musicSource, _effectSource;

    [SerializeField]
    private AudioClip _clickSound;

    private bool isSoundMuted;
    private bool IsSoundMuted
    {
        get
        {
            isSoundMuted = (PlayerPrefs.HasKey(Constants.Data.SETTINGS_SOUND)
                ? PlayerPrefs.GetInt(Constants.Data.SETTINGS_SOUND) : 1) == 0;
            return isSoundMuted;
        }
        set
        {
            isSoundMuted = value;
            PlayerPrefs.SetInt(Constants.Data.SETTINGS_SOUND, isSoundMuted ? 0 : 1);
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PlayerPrefs.SetInt(Constants.Data.SETTINGS_SOUND, isSoundMuted ? 0 : 1);
        _effectSource.mute = isSoundMuted;
        _musicSource.mute = isSoundMuted;
        _musicSource.Play();
    }

    public void AddButtonSound()
    {
        var buttons = FindObjectsOfType<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => {
                PlaySound(_clickSound);
            });
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void ToggleSound()
    {
        _musicSource.mute = IsSoundMuted;
        _effectSource.mute = IsSoundMuted;
    }
}
