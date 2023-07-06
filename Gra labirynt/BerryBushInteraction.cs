using UnityEngine;

public class BerryBushInteraction : MonoBehaviour
{
    //zmienna przechowujaca dystnas z jakiego mozemy podjac interakcje z przedmiotem
    public float itemDetectionDistance = 3.5f;

    //obiekt z glowna kamera
    private Camera mainCamera;

    //obiekt RaycastHit pomagajacy w namierzeniu obiektow
    private RaycastHit hitInfo;

    [SerializeField]
    private GameObject inventoryBar;

    [SerializeField]
    private GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        // Przypisanie komponentu kamery z obiektu podrz�dnego
        mainCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.useSomethingKey) && !PauseManager.instance.gamePaused)
        {
            InventorySlot[] chestInvSlots = inventoryBar.GetComponentsInChildren<InventorySlot>();
            // Utworzenie promienia od gracza do miejsca, gdzie "celuje" gracz
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Wykrycie kolizji mi�dzy promieniem, a innymi obiektami na scenie w odleg�o�ci okre�lonej przez zmienn�
            if (Physics.Raycast(ray, out hitInfo) && hitInfo.distance < itemDetectionDistance)
            {
                // Sprawdzenie, czy kolizja wyst�puje w obiektem z tagiem "BerryBush"
                if (hitInfo.collider.gameObject.CompareTag("BerryBush"))
                {
                    Krzak krzak = hitInfo.collider.gameObject.GetComponent<Krzak>();

                    if (krzak.giveAccessToTakeBerry())
                    {
                        GameObject berry = Instantiate<GameObject>(prefab);
                        berry.SetActive(false);
                        string nameOfItem = berry.GetComponent<NameOfItems>().getNameOfItem();

                        //dodanie przedmiotu do inventory gracza oraz przekazanie obiektu ze sceny aby dodac go do listy
                        bool result = GetComponent<InventoryManager>().AddItem(GetComponent<InventoryManager>().getItemsToPickUp(nameOfItem), berry);
                        if (result == true)
                        {
                            krzak.takeBerry();
                            Debug.Log("Dodano jagodę do EQ");
                        }
                        else
                        {
                            Debug.Log("Nie ma wolnego miejsca w ekwipunku lub jagody na krzaku");
                        }
                    }
                }
            }
        }
    }
}
