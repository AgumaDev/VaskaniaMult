using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameMenuHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public RectTransform PlayerMenu;
    public Ease curveAnimation;

    public GameObject playerPrefs;

    public CanvasGroup menu;
    public CanvasGroup options;

    public Slider playerSensSlider;
    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider BGMSlider;

    public float playerSensValue;
    public float MasterValue;
    public float SFXValue;
    public float BGMValue;

    public bool isMenuEnabled;
    public bool isOptionsEnabled;
    void Start()
    {
        PlayerMenu.DOLocalMove(new Vector3(-850, 0, 0), 1, true).SetEase(curveAnimation);

        playerPrefs = GameObject.FindGameObjectWithTag("PlayerPrefs");

        MasterValue = playerPrefs.GetComponent<PlayerPrefs>().masterVolume;
        SFXValue = playerPrefs.GetComponent<PlayerPrefs>().sfxVolume;
        BGMValue = playerPrefs.GetComponent<PlayerPrefs>().bgmVolume;
        playerSensValue = playerPrefs.GetComponent<PlayerPrefs>().mouseSensitivity;

        masterSlider.value = playerPrefs.GetComponent<PlayerPrefs>().masterVolume;
        SFXSlider.value = playerPrefs.GetComponent<PlayerPrefs>().sfxVolume;
        BGMSlider.value = playerPrefs.GetComponent<PlayerPrefs>().bgmVolume;
        playerSensSlider.value = playerPrefs.GetComponent<PlayerPrefs>().mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuEnabled = !isMenuEnabled;
            if (isMenuEnabled)
            {
                PlayerMenu.DOLocalMove(new Vector3(0, 0, 0), 0.5f, true).SetEase(curveAnimation);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                PlayerMenu.DOLocalMove(new Vector3(-850, 0, 0), 0.5f, true).SetEase(curveAnimation);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        MasterValue = masterSlider.value;
        SFXValue = SFXSlider.value;
        BGMValue = BGMSlider.value;
        playerSensValue = playerSensSlider.value;

        playerPrefs.GetComponent<PlayerPrefs>().masterVolume = masterSlider.value;
        playerPrefs.GetComponent<PlayerPrefs>().bgmVolume = BGMSlider.value;
        playerPrefs.GetComponent<PlayerPrefs>().sfxVolume = SFXSlider.value;
        playerPrefs.GetComponent<PlayerPrefs>().mouseSensitivity = playerSensSlider.value;
    }

    public void ContinueButton()
    {
        isMenuEnabled = !isMenuEnabled;
        isOptionsEnabled = false;
    }

    public void OptionsButton()
    { 
        isOptionsEnabled = !isOptionsEnabled;
        if (isOptionsEnabled)
        {
            menu.DOFade(0, 1).SetEase(curveAnimation);
            options.DOFade(1, 1).SetEase(curveAnimation);
        }
        else 
        {
            menu.DOFade(1, 1).SetEase(curveAnimation);
            options.DOFade(0, 1).SetEase(curveAnimation);
        }
    }
}
