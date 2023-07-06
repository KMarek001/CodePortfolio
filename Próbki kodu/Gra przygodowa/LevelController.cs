using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public int levelNumber = 0;
    public int[] numberOfEnemies = {1,3,4,5};
    public int enemyCounter = 0;

    private GameObject portal;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        portal = GameObject.Find("CenterPoint").transform.GetChild(0).gameObject;
    }

    void Update()
    {
        
    }

    public void CheckNumberOfEnemy()
    {
        Scene scene = SceneManager.GetActiveScene();
        int enemiesAmount = 0;

        if (scene.name == "Tutorial")
            enemiesAmount = 1;
        else if (scene.name == "Level1")
            enemiesAmount = 3;
        else if (scene.name == "Level2")
            enemiesAmount = 4;
        else if (scene.name == "Level3")
            enemiesAmount = 5;

        if (enemyCounter == enemiesAmount)
        {
            levelNumber++;
            enemyCounter = 0;
            portal.SetActive(true);
        }
    }
}
