using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPScript : MonoBehaviour
{
    public static XPScript instance;

    private Slider slider;
    private PlayerController playerController;
    [SerializeField]
    private TextMeshProUGUI currentExpText;
    [SerializeField]
    private TextMeshProUGUI maxExpText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        slider.maxValue = playerController.GetMaxExperience();
        slider.value = slider.minValue;

        maxExpText.text = playerController.GetMaxExperience().ToString();
        currentExpText.text = "0";
    }

    public void UpdateXPBar()
    {
        slider.value = playerController.currentEXP;
        currentExpText.text = playerController.currentEXP.ToString();
        if(playerController.currentEXP >= playerController.GetMaxExperience())
        {
            levelText.text = playerController.level.ToString();
        }
    }
}
