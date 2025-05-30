using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuHandler : MonoBehaviour
{
    private int layerNumber = 13;
    private int layerMask;

    private int selectedLayerNumber = 14;
    private int selectedLayerMask = 14;

    public bool selectedOption;

    public GameObject CinemachineMainCam;
    public GameObject CinemachineBookCam;
    public GameObject CinemachineTVCam;

    public Slider playerSensSlider;
    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider BGMSlider; 

    public int playerSensValue;
    public int MasterValue;
    public int SFXValue;
    public int BGMValue;
    
    public AudioMixerGroup masterGroup;
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;

    public TextMeshProUGUI SensValueText;
    public TextMeshProUGUI MasterValueText;
    public TextMeshProUGUI SFXValueText;
    public TextMeshProUGUI BGMValueText;

    public TextMeshProUGUI currentMicrophone;

    public TriggerBox currentSelectedTrigger;

    public BookRotation bookRotation;

    public GameObject playerPrefs;

    public float transitionSpeed;


    public bool graphicsActive;
    public bool soundActive;
    public bool controlsActive;
    
    public CanvasGroup graphicsSettings;
    public CanvasGroup soundSettings;
    public CanvasGroup controlSettings;
    
    public GameObject hostMenu;
    public GameObject clientMenu;
    public GameObject hostAndClientMenu;
    
    void Start()
    {
        layerMask = 1 << layerNumber;
        selectedLayerMask = 1 << selectedLayerNumber;
    }

    void Update()
    {
       // OptionsHandler();
        RaycastManager();
        
        playerPrefs.GetComponent<PlayerPrefs>().mouseSensitivity = playerSensSlider.value;

        playerPrefs.GetComponent<PlayerPrefs>().masterVolume = masterSlider.value;
        playerPrefs.GetComponent<PlayerPrefs>().sfxVolume = SFXSlider.value;
        playerPrefs.GetComponent<PlayerPrefs>().bgmVolume = BGMSlider.value;


        masterGroup.audioMixer.SetFloat("MasterParam", Mathf.Log10(masterSlider.value) * 20);
        sfxGroup.audioMixer.SetFloat("SFXParam", Mathf.Log10(SFXSlider.value) * 20);
        bgmGroup.audioMixer.SetFloat("BGMParam", Mathf.Log10(BGMSlider.value) * 20);

        SensValueText.text = playerSensSlider.value.ToString("F2");
        MasterValueText.text = (masterSlider.value * 100).ToString("F0");
        SFXValueText.text = (SFXSlider.value * 100).ToString("F0");
        BGMValueText.text = (BGMSlider.value * 100).ToString("F0");

        if (graphicsActive)
        {
            graphicsSettings.alpha += Time.deltaTime * transitionSpeed;
            soundSettings.alpha -= Time.deltaTime * transitionSpeed;
            controlSettings.alpha -= Time.deltaTime * transitionSpeed;
        }
        if (soundActive)
        {
            graphicsSettings.alpha -= Time.deltaTime * transitionSpeed;
            soundSettings.alpha += Time.deltaTime * transitionSpeed;
            controlSettings.alpha -= Time.deltaTime * transitionSpeed;
        }
        if (controlsActive)
        {
            graphicsSettings.alpha -= Time.deltaTime * transitionSpeed;
            soundSettings.alpha -= Time.deltaTime * transitionSpeed;
            controlSettings.alpha += Time.deltaTime * transitionSpeed;
        }
    }

    public void GraphicsSettingsButton()
    {
        graphicsActive = true;
        soundActive = false;
        controlsActive = false;

        graphicsSettings.interactable = true;
        soundSettings.interactable = false;
        controlSettings.interactable = false;

        graphicsSettings.blocksRaycasts = true;
        soundSettings.blocksRaycasts = false;
        controlSettings.blocksRaycasts = false;
    }
    public void AudioSettingsButton()
    {
        graphicsActive = false;
        soundActive = true;
        controlsActive = false;

        graphicsSettings.interactable = false;
        soundSettings.interactable = true;
        controlSettings.interactable = false;
        
        graphicsSettings.blocksRaycasts = false;
        soundSettings.blocksRaycasts = true;
        controlSettings.blocksRaycasts = false;
    }
    public void ControlsSettingsButton()
    {
        graphicsActive = false;
        soundActive = false;
        controlsActive = true;

        graphicsSettings.interactable = false;
        soundSettings.interactable = false;
        controlSettings.interactable = true;
        
        graphicsSettings.blocksRaycasts = false;
        soundSettings.blocksRaycasts = false;
        controlSettings.blocksRaycasts = true;
        
    }
    public void ShowHostMenu()
    {
        hostMenu.SetActive(true);

        clientMenu.SetActive(false);
        
        hostAndClientMenu.SetActive(false);
    }
    public void ShowClientMenu()
    {
        clientMenu.SetActive(true);
        
        hostMenu.SetActive(false);
        
        hostAndClientMenu.SetActive(false);
    }
    public void Return()
    {
        hostAndClientMenu.SetActive(true);

        clientMenu.SetActive(false);
        
        hostMenu.SetActive(false);

        Debug.Log("alo");
    }
    public void ExitButton()
    { 
    
    }
    public void RaycastManager()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (selectedOption == false)
        {
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (selectedOption == false)
                {
                    if (hit.transform.GetComponent<TriggerBox>() && hit.transform.CompareTag("Menu/Book"))
                    {
                        hit.transform.GetComponent<TriggerBox>().isHovered = true;
                        hit.transform.GetComponent<TriggerBox>().hoverTime = 0.1f;

                        if (Input.GetMouseButtonDown(0))
                        {
                            hit.transform.GetComponent<TriggerBox>().cinemachineCamMain.SetActive(false);
                            hit.transform.GetComponent<TriggerBox>().cinemachineCamBook.SetActive(true);
                            hit.transform.GetComponent<TriggerBox>().hasBeenClicked = true;
                            selectedOption = true;
                            currentSelectedTrigger = hit.transform.GetComponent<TriggerBox>();

                            bookRotation.OpenBook();
                        }
                    }
                    if (hit.transform.GetComponent<TriggerBox>() && hit.transform.CompareTag("Menu/TV"))
                    {
                        hit.transform.GetComponent<TriggerBox>().isHovered = true;
                        hit.transform.GetComponent<TriggerBox>().hoverTime = 0.1f;

                        if (Input.GetMouseButtonDown(0))
                        {
                            hit.transform.GetComponent<TriggerBox>().cinemachineCamMain.SetActive(false);
                            hit.transform.GetComponent<TriggerBox>().cinemachineCamBook.SetActive(true);
                            hit.transform.GetComponent<TriggerBox>().hasBeenClicked = true;
                            selectedOption = true;
                            currentSelectedTrigger = hit.transform.GetComponent<TriggerBox>();
                        }
                    }
                }
                
                if()
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit, 1000, selectedLayerMask))
            {
                if (hit.transform.GetComponent<ReturnBox>() && hit.transform.CompareTag("Menu/Book"))
                {
                    print("a");
                    if (Input.GetMouseButtonDown(0))
                    {
                        CinemachineBookCam.SetActive(false);
                        CinemachineTVCam.SetActive(false);
                        CinemachineMainCam.SetActive(true);
                        selectedOption = false;
                        currentSelectedTrigger.hasBeenClicked = false;
                        currentSelectedTrigger = null;
                        
                        bookRotation.OpenBook();
                    }
                }
                if (hit.transform.GetComponent<ReturnBox>() && hit.transform.CompareTag("Menu/TV"))
                {
                    print("a");
                    if (Input.GetMouseButtonDown(0))
                    {
                        CinemachineBookCam.SetActive(false);
                        CinemachineTVCam.SetActive(false);
                        CinemachineMainCam.SetActive(true);
                        selectedOption = false;
                        currentSelectedTrigger.hasBeenClicked = false;
                        currentSelectedTrigger = null;
                    }
                }
            }
        }
        Debug.DrawRay( ray.origin, ray.direction * 1000, Color.red);
    }
}
