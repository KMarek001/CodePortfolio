using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static ChangeScene instance;

    private void Awake()
    {
        instance = this;
    }

    public void SwapScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        switch(scene.name)
        {
            case "Tutorial":
            {
                SceneManager.LoadScene("Level1");
                break;
            }
            case "Level1":
            {
                SceneManager.LoadScene("Level2");
                break;
            }
            case "Level2":
            {
                SceneManager.LoadScene("Level3");
                break;
            }
            default:
            {
                SceneManager.LoadScene("MainMenu");
                break;
            }     
        }
    }
}