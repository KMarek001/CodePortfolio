using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public Animator playerAnimator;

    public Transform[] attackPoints;
    public float attackRange = 0.09656227f;
    public LayerMask enemyLayer;

    private int attackDamage = 2;

    public float attackCooldown = 0.5f;
    private float nextAttack;

    [SerializeField]
    private AudioClip swingSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackCooldown;
            Attack(0);
        }
        if (Input.GetMouseButtonDown(1) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackCooldown;
            Attack(1);
        }
        
    }

    void Attack(int attackType)
    {
        audioSource.PlayOneShot(swingSound, 0.2f);
        if(attackType == 0)
        {
            playerAnimator.Play("Attack1");
            attackDamage = 2;
        }
            
        else if (attackType == 1)
        {
            playerAnimator.Play("Attack2");
            attackDamage = 4;
        }

        Collider[] hitEnemies = Physics.OverlapCapsule(attackPoints[0].position, attackPoints[1].position, attackRange, enemyLayer);

        foreach(Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
            enemy.GetComponent<EnemyController>().particleSystem.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoints == null)
            return;
    }
}
