using UnityEngine;

public class DeathSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject gravePrefab;

    private HealthSystem playerHealth;

    private bool isFirstDeath = true;

    private bool playerIsDeath = false;

    private PauseUI pauseManager;

    void Start()
    {
        playerHealth = GetComponent<HealthSystem>();
        pauseManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseUI>();
    }

    public void RespawnPlayer()
    {
        //Ustawienie miejsca odrodzenia gracza
        GameObject gameObject = GameObject.FindGameObjectWithTag("Fountain");
        if (gameObject != null)
        {
            FontainControler fontain = gameObject.GetComponent<FontainControler>();
            if (fontain != null)
            {
                transform.position = fontain.posToTeleport.position;
            }
        }

        //Redukcja zdrowia gracza do 50%
        playerHealth.currentHealth = GetComponent<HealthSystem>().maxHealth / 2;
        //Ustawienie sanity do 100%
        SanitySystem sanity = GetComponent<SanitySystem>();
        sanity.sanityLevel = 100;
        sanity.UpdateSanityIndicator();
        if (isFirstDeath)
        {
            PlayerInfoPageUnlocker playerInfoPageUnlocker = GetComponent<PlayerInfoPageUnlocker>();
            playerInfoPageUnlocker.UnlockDeathInfoPage();
            isFirstDeath = false;
        }

        pauseManager.deathScreen.SetActive(false);
        PauseManager.instance.gamePaused = false;
        playerIsDeath = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        if(playerHealth.currentHealth <= 0 && !PauseManager.instance.gamePaused && !playerIsDeath)
        {
            playerIsDeath = true;
            pauseManager.deathScreen.SetActive(true);

            Vector3 deathPlace = transform.position;
            //Powstanie grobu na miejscu smierci
            GameObject grave = Instantiate(gravePrefab);
            deathPlace.y = deathPlace.y - 2 + (grave.transform.localScale.y/2)/ grave.transform.localScale.y;
            grave.transform.SetLocalPositionAndRotation(deathPlace, new Quaternion(0,0,0,0));

            grave.GetComponent<GraveEquipment>().getGraveItems();

            bool isInvEmpty = true;

            int index = 0;
            foreach (InventorySlot slot in GetComponent<InventoryManager>().inventorySlots)
            {
                InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();
                if(slotItem != null)
                {
                    grave.GetComponent<GraveEquipment>().addItem(slotItem, index);
                    slot.GetComponentInChildren<InventoryItem>().transform.SetParent(null);
                    isInvEmpty = false;
                }
                index++;
            }
            if(isInvEmpty)
                Destroy(grave);
        }
    }
}
