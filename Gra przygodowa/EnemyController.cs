using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private int level = 1;
    private int[] damage = {1,2,5,10};
    private int[] maxHealth = {10,15,25,50};
    private int[] experience = {5,10,20,50};
    public int currentHealth;

    [Header("Attack")]
    private Animator enemyAnimator;
    public float attackCooldown = 50f;
    private float nextAttack;
    public float attackRange = 1.7f;

    private GameObject player;
    private Transform playerTransform;
 
    [Header("Navigation")]
    public Transform[] navPoints;
    private NavMeshAgent navAgent;
    public int destPoint = 0;
    public float playerDistance;
    public float awareAI = 10f;
    public float AIMoveSpeed;
    private float tmpSpeed;

    [Header("Sound")]
    [SerializeField]
    private AudioClip damageSound;

    private AudioSource audioSource;

    public ParticleSystem particleSystem;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerTransform = player.GetComponent<Transform>();

        navAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        enemyAnimator.SetBool("isWalking", true);
        currentHealth = maxHealth[level - 1];
        navAgent.destination = navPoints[0].position;
        tmpSpeed = AIMoveSpeed;
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(playerTransform.position, transform.position);

        if(playerDistance < awareAI)
        {
            navAgent.destination = playerTransform.position;
            LookAtPlayer();
            transform.Translate(Vector3.forward * AIMoveSpeed * Time.deltaTime);
            enemyAnimator.SetBool("isWalking", true);

            if (playerDistance < attackRange)
            {
                Stop();
                enemyAnimator.SetBool("isWalking", false);
                if (Time.time > nextAttack)
                {
                    nextAttack = Time.time + attackCooldown;
                    Attack();
                }
            }
            else
                Resume();
        }
        else if (transform.position == navAgent.destination)
            GoToNextPoint();
    }

    void Stop()
    {
        AIMoveSpeed = 0;
        navAgent.isStopped = true;
    }

    void Resume()
    {
        AIMoveSpeed = tmpSpeed;
        navAgent.isStopped = false;
    }

    void LookAtPlayer()
    {
        transform.LookAt(playerTransform);
    }

    void GoToNextPoint()
    {
        if(navPoints.Length == 0)
            return;
        navAgent.destination = navPoints[destPoint].position;
        destPoint = (destPoint + 1) % navPoints.Length;
        enemyAnimator.SetBool("isWalking", true);
    }

    void Attack()
    {
        enemyAnimator.Play("Attack");
    }

    void Hit()
    {
        player.GetComponent<PlayerController>().GetDamage(damage[level - 1]);
    }

    public void TakeDamage(int damage)
    {
        enemyAnimator.SetTrigger("Hit");
        currentHealth -= damage;
        audioSource.PlayOneShot(damageSound, 0.15f);

        if (currentHealth <= 0)
            Die();
    }
    
    void Die()
    {
        enemyAnimator.SetTrigger("Die");
        player.GetComponent<PlayerController>().LevelUp(experience[level - 1]);
        LevelController.instance.enemyCounter++;
        LevelController.instance.CheckNumberOfEnemy();
    }

    void disapearEnemy()
    {
        gameObject.SetActive(false);
    }
}
