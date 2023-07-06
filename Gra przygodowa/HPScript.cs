using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPScript : MonoBehaviour
{
    public static HPScript instance;

    private Slider slider;
    private PlayerController playerController;
    [SerializeField]
    private TextMeshProUGUI currentHealthText;
    [SerializeField]
    private TextMeshProUGUI maxHealthText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        slider.maxValue = playerController.GetMaxHealth();
        slider.value = slider.maxValue;

        maxHealthText.text = playerController.GetMaxHealth().ToString();
        currentHealthText.text = playerController.currentHealth.ToString();
    }

    public void UpdateHPBar()
    {
        slider.value = playerController.currentHealth;
        currentHealthText.text = playerController.currentHealth.ToString();
    }
}
