using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ChestEquipment;

public class ChestInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryChest;

    private GameObject currentChest;

    //flaga informuj�ca, czy ekwipunek skrzyni jest aktywny
    private bool isChestInvActive = false;

    //zmienna przechowujaca dystnas z jakiego mozemy podjac interakcje z przedmiotem
    public float itemDetectionDistance = 3.5f;

    //obiekt z glowna kamera
    private Camera mainCamera;

    //obiekt RaycastHit pomagajacy w namierzeniu obiektow
    private RaycastHit hitInfo;

    [SerializeField]
    private GameObject canvas;

    GameObject codeBar;
    GameObject hintText;

    private int[] code = new int[4] {0,0,0,0};

    private int[] currentChestCode = new int[4];

    private InventorySlot[] chestInvSlots;

    private AudioSource audioObject;
    
    void Start()
    {
        if (inventoryChest != null)
        {
            inventoryChest.SetActive(false);
        }
        // Przypisanie komponentu kamery z obiektu podrz�dnego
        mainCamera = GetComponentInChildren<Camera>();
        codeBar = canvas.transform.Find("ChestCode").gameObject;
        hintText = codeBar.transform.Find("HintText").gameObject;
        chestInvSlots = inventoryChest.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.useSomethingKey) && isChestInvActive == false)
        {
            // Utworzenie promienia od gracza do miejsca, gdzie "celuje" gracz
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Wykrycie kolizji mi�dzy promieniem, a innymi obiektami na scenie w odleg�o�ci okre�lonej przez zmienn�
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.distance < itemDetectionDistance)
            {
                // Sprawdzenie, czy kolizja wyst�puje w obiektem z tagiem "Chest"
                if (hitInfo.collider.gameObject.CompareTag("Chest"))
                {
                    PauseManager.instance.gamePaused = true;
                    currentChest = hitInfo.collider.gameObject;

                    audioObject = currentChest.GetComponent<ChestEquipment>().GetComponent<AudioSource>();

                    if (currentChest.GetComponent<ChestEquipment>().isUnlocked)
                    {
                        openChest();
                    }
                    else
                    {
                        ChestEquipment.ChestType chestType = currentChest.GetComponent<ChestEquipment>().getChestType();
                        bool hasAccess = false;
                        if (chestType == ChestEquipment.ChestType.Key)
                        {
                            String keyName = currentChest.GetComponent<ChestEquipment>().key.GetComponent<NameOfItems>().getNameOfItem();
                            Item selectedItem = GetComponent<InventoryManager>().GetSelectedItem(false);
                            if (selectedItem != null && selectedItem.itemName == keyName)
                            {
                                hasAccess = true;
                                GetComponent<InventoryManager>().GetSelectedItem(true);
                                audioObject.PlayOneShot(currentChest.GetComponent<ChestEquipment>().correctKeySound, 0.7f * PauseManager.instance.soundVolume);
                            }
                            else
                            {
                                audioObject.PlayOneShot(currentChest.GetComponent<ChestEquipment>().invalidKeySound, 0.7f * PauseManager.instance.soundVolume);
                                Debug.Log("Nieprawidlowy klucz");
                                PauseManager.instance.gamePaused = false;
                            }
                        }
                        else if (chestType == ChestType.Code)
                        {
                            if (currentChest.GetComponent<ChestEquipment>().getChestType() == ChestType.Code)
                            {
                                Text[] codeTexts = codeBar.GetComponentsInChildren<Text>();

                                foreach (Text text in codeTexts)
                                    text.text = "0";
                            }

                            Array.Copy(currentChest.GetComponent<ChestEquipment>().code, currentChestCode, 4);
                            codeBar.SetActive(true);
                            hintText.GetComponent<TextMeshProUGUI>().text  = currentChest.GetComponent<ChestEquipment>().hint;
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                        }
                        else if (chestType == ChestType.None)
                        {
                            hasAccess = true;
                        }

                        if (hasAccess)
                        {
                            if (chestType == ChestType.Key)
                                Invoke("openChest", 5);
                            else
                                openChest();

                            currentChest.GetComponent<ChestEquipment>().isUnlocked = true;
                        }
                    }
                }
            }
        }
        else if (isChestInvActive)
        {
            int index = 0;
            bool isChestEmpty = true;

            foreach (InventorySlot slot in chestInvSlots)
            {
                if (slot.GetComponentInChildren<InventoryItem>() != null)
                {
                    currentChest.GetComponent<ChestEquipment>().addItem(slot.GetComponentInChildren<InventoryItem>(), index);
                    isChestEmpty = false;
                }
                else
                {
                    currentChest.gameObject.GetComponent<ChestEquipment>().addItem(null, index);
                }
                index++;
            }

            if (Input.GetKeyDown(InputManager.instance.cancelKey))
            {
                closeChest(index, isChestEmpty);
            }
        }
        else if (codeBar.activeSelf)
        {
            if (Input.GetKeyDown(InputManager.instance.cancelKey))
            {
                codeBar.SetActive(false);
            }
        }
    }

    void openChest()
    {
        if(!currentChest.GetComponent<ChestEquipment>().isChestOpened)
        {
            currentChest.GetComponent<Animator>().Play("ChestOpening");
            audioObject.PlayOneShot(currentChest.GetComponent<ChestEquipment>().openingSound, 0.7f * PauseManager.instance.soundVolume);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inventoryChest.SetActive(true);
        isChestInvActive = true;
        currentChest.GetComponent<ChestEquipment>().isChestOpened = true;
        int index = 0;

        foreach (InventorySlot slot in chestInvSlots)
        {
            if (currentChest.GetComponent<ChestEquipment>().getChestItems()[index] != null)
            {
                currentChest.GetComponent<ChestEquipment>().getChestItems()[index].transform.SetParent(slot.transform);
            }
            index++;
        }
    }

    void closeChest(int index, bool isChestEmpty)
    {
        index = 0;
        foreach (InventorySlot slot in chestInvSlots)
        {
            if (currentChest.GetComponent<ChestEquipment>().getChestItems()[index] != null)
            {
                slot.GetComponentInChildren<InventoryItem>().transform.SetParent(null);
            }
            index++;
        }

        isChestInvActive = !isChestInvActive;
        inventoryChest.SetActive(false);
        if (audioObject.isPlaying)
            audioObject.Stop();

        if (!isChestEmpty)
        {
            currentChest.GetComponent<Animator>().Play("ChestClosing");
            audioObject.PlayOneShot(currentChest.GetComponent<ChestEquipment>().closingSound, 0.7f * PauseManager.instance.soundVolume);
            currentChest.GetComponent<ChestEquipment>().isChestOpened = false;
        }
    }

    public void changeCode(Button button)
    {
        int number = int.Parse(button.transform.GetChild(0).GetComponent<Text>().text);
        if (number < 9)
            number++;
        else number = 0;
        button.transform.GetChild(0).GetComponent<Text>().text = number.ToString();

        string name = button.name;
        switch (name)
        {
            case "Button1":
                code[0] = number;
                break;
            case "Button2":
                code[1] = number;
                break;
            case "Button3":
                code[2] = number;
                break;
            case "Button4":
                code[3] = number;
                break;
            default:
                return;
        }
        if (checkCode())
        {
            openChest();
            currentChest.GetComponent<ChestEquipment>().isUnlocked = true;
            codeBar.SetActive(false);
        }
        else
            Debug.Log("Nieprawidlowy kod");
    }

    private bool checkCode()
    {
        for (int i = 0; i < code.Length; i++)
        {
            if (code[i] != currentChestCode[i])
                return false;
        }
        return true;
    }
}
