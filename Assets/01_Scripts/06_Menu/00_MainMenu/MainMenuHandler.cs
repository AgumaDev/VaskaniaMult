using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Serialization;
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

    public Animator bookAnim;

    public GameObject playerPrefs;


    public bool graphicsActive;
    public bool soundActive;
    public bool controlsActive;
    
    public CanvasGroup graphicsSettings;
    public CanvasGroup soundSettings;
    public CanvasGroup controlSettings;
    
    

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
            graphicsSettings.alpha += Time.deltaTime;
            soundSettings.alpha -= Time.deltaTime;
            controlSettings.alpha -= Time.deltaTime;
        }
        if (soundActive)
        {
            graphicsSettings.alpha -= Time.deltaTime;
            soundSettings.alpha += Time.deltaTime;
            controlSettings.alpha -= Time.deltaTime;
        }
        if (controlsActive)
        {
            graphicsSettings.alpha -= Time.deltaTime;
            soundSettings.alpha -= Time.deltaTime;
            controlSettings.alpha += Time.deltaTime;
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
                    if (hit.transform.GetComponent<TriggerBox>())
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
                            
                            bookAnim.SetBool("PressBool", true);
                        }
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit, 1000, selectedLayerMask))
            {
                if (hit.transform.GetComponent<ReturnBox>())
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
                        
                        bookAnim.SetBool("PressBool", false);
                    }
                }
            }
        }
    
        
        Debug.DrawRay( ray.origin, ray.direction * 1000, Color.red);
    }

}
