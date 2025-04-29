using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public bool isOptionsEnabled;
    public GameObject optionsPanel;

    public Slider playerSensSlider;
    public Slider MasterSlider;
    public Slider SFXSlider;
    public Slider BGMSlider; 

    public int playerSensValue;
    public int MasterValue;
    public int SFXValue;
    public int BGMValue;

    public TextMeshProUGUI SensValueText;
    public TextMeshProUGUI MasterValueText;
    public TextMeshProUGUI SFXValueText;
    public TextMeshProUGUI BGMValueText;

    public TextMeshProUGUI currentMicrophone;

    void Start()
    {
        
    }

    void Update()
    {
        OptionsHandler();
    }

    public void StartButton()
    { 
        
    }

    public void OptionsButton()
    {
        isOptionsEnabled = !isOptionsEnabled;
    }

    public void OptionsHandler()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isOptionsEnabled = !isOptionsEnabled;
        }


        if (isOptionsEnabled)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            optionsPanel.SetActive(false);
        }
    }

    public void ExitButton()
    { 
    
    }

}
