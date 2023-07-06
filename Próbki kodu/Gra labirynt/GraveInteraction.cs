using UnityEngine;

public class GraveInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject graveInventory;

    private GameObject currentGrave;

    //flaga informuj�ca, czy ekwipunek grobu jest aktywny
    private bool isGraveInvActive = false;

    //zmienna przechowujaca dystnas z jakiego mozemy podjac interakcje z przedmiotem
    public float itemDetectionDistance = 3.5f;

    //obiekt z glowna kamera
    private Camera mainCamera;

    //obiekt RaycastHit pomagajacy w namierzeniu obiektow
    private RaycastHit hitInfo;

    [SerializeField]
    private GameObject canvas;

    GameObject message;

    InventorySlot[] graveInvSlots;

    public void onYesClicked()
    {
        PauseManager.instance.SetPauseState(false);

        currentGrave.GetComponent<GraveEquipment>().clearEquipment();
        message.SetActive(false);
        PauseManager.instance.gamePaused = false;
        Destroy(currentGrave);
    }

    public void onNoClicked()
    {
        isGraveInvActive = !isGraveInvActive;
        graveInventory.SetActive(true);

        int index = 0;
        foreach (InventorySlot slot in graveInvSlots)
        {
            if (currentGrave.GetComponent<GraveEquipment>().getGraveItems()[index] != null)
            {
                currentGrave.GetComponent<GraveEquipment>().getGraveItems()[index].transform.SetParent(slot.transform);
            }
            index++;
        }

        message.SetActive(false);
    }

    void Start()
    {
        if (graveInventory != null)
        {
            graveInventory.SetActive(false);
        }
        // Przypisanie komponentu kamery z obiektu podrz�dnego
        mainCamera = GetComponentInChildren<Camera>();

        message = canvas.transform.Find("DeathItemMessage").gameObject;
        graveInvSlots = graveInventory.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.useSomethingKey) && isGraveInvActive == false)
        {
            // Utworzenie promienia od gracza do miejsca, gdzie "celuje" gracz
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Wykrycie kolizji mi�dzy promieniem, a innymi obiektami na scenie w odleg�o�ci okre�lonej przez zmienn�
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.distance < itemDetectionDistance)
            {
                // Sprawdzenie, czy kolizja wyst�puje w obiektem z tagiem "Chest"
                if (hitInfo.collider.gameObject.CompareTag("Grave"))
                {
                    currentGrave = hitInfo.collider.gameObject;

                    graveInventory.SetActive(true);
                    isGraveInvActive = true;
                    int index = 0;

                    foreach (InventorySlot slot in graveInvSlots)
                    {
                        if (currentGrave.GetComponent<GraveEquipment>().getGraveItems()[index] != null)
                        {
                            currentGrave.GetComponent<GraveEquipment>().getGraveItems()[index].transform.SetParent(slot.transform);
                        }
                        index++;
                    }
                }
            }
        }
        else if (message.activeSelf)
        {
            PauseManager.instance.gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (!PauseManager.instance.IsPauseScreenShowing())
            {
                if (Input.GetKeyDown(InputManager.instance.cancelKey))
                {
                    PauseManager.instance.gamePaused = false;
                }
            }
        }
        else if (isGraveInvActive)
        {
            PauseManager.instance.gamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            int index = 0;

            foreach (InventorySlot slot in graveInvSlots)
            {
                if (slot.GetComponentInChildren<InventoryItem>() != null)
                {
                    currentGrave.GetComponent<GraveEquipment>().addItem(slot.GetComponentInChildren<InventoryItem>(), index);
                }

                index++;
            }

            if (Input.GetKeyDown(InputManager.instance.cancelKey))
            {
                bool isEmpty = true;
                index = 0;
                foreach (InventorySlot slot in graveInvSlots)
                {
                    if (slot.GetComponentInChildren<InventoryItem>() != null)
                    {
                        isEmpty = false;
                        slot.GetComponentInChildren<InventoryItem>().transform.SetParent(null);
                    }
                    index++;
                }

                isGraveInvActive = !isGraveInvActive;
                graveInventory.SetActive(false);

                if (isEmpty)
                {
                    currentGrave.GetComponent<GraveEquipment>().clearEquipment();
                    PauseManager.instance.SetPauseState(false);
                    Destroy(currentGrave);
                }
                else
                {
                    message.SetActive(true);
                    PauseManager.instance.SetPauseState(true);
                    PauseManager.instance.pauseScreen.ShowPauseScreen(false);
                }
            }
        }
    }
}
