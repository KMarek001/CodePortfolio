using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int[] maxHealth = {20,30,40,50};
    public int currentHealth;

    private int[] maxEXP = {50,100,200,400,800,1000};
    public int currentEXP;
    public int level = 0;

    public Animator playerAnimator;

    [SerializeField]
    private AudioClip damageSound;

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth[level];
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetMaxHealth()
    {
        return maxHealth[level];
    }

    public int GetMaxExperience()
    {
        return maxEXP[level];
    }

    public void LevelUp(int exp)
    {
        currentEXP += exp;
        if(currentEXP >= maxEXP[level])
        {
            level++;
            currentEXP = 0;
        }
        XPScript.instance.UpdateXPBar();
    }

    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        playerAnimator.Play("GetHit");
        HPScript.instance.UpdateHPBar();
        audioSource.PlayOneShot(damageSound, 0.35f);
        if (currentHealth <= 0)
        {
            transform.position = new Vector3(0, 1, 0);
            currentHealth = maxHealth[level];
        }
            
    }
}
