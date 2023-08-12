using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyScript : MonoBehaviour
{
    public bool isDead;
    public float maxHealth = 100;
    private float health;
    GameObject player;
    Animator animator;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private float detectRange;

    float cooldownTimePassed = 0;
    float updateNewDestCoolDown = 0.3f;

    NavMeshAgent navAgent;


    void Start()
    {
        health = maxHealth;
        isDead = false;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerStateManager>().gameObject;
        print(player.name);
    }

    void Update()
    {
        if (isDead) return;
        if(player.GetComponent<PlayerStateManager>().IsDead() && !isDead)
        {
            animator.SetFloat("enemySpeed", 0f);
        }
        else if (!player.GetComponent<PlayerStateManager>().IsDead() && !isDead)
        {
            // add a logic to not be called all the time like a bool
            animator.SetFloat("enemySpeed", navAgent.velocity.magnitude / navAgent.speed); // to normalize the value


            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                animator.SetTrigger("attack");
                cooldownTimePassed = 0;
            }
            cooldownTimePassed += Time.deltaTime;

            if (updateNewDestCoolDown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= detectRange)
            {
                transform.LookAt(player.transform);
                updateNewDestCoolDown = 0.3f;
                navAgent.SetDestination(player.transform.position);
            }

            updateNewDestCoolDown -= Time.deltaTime;
            
        }

    }
    public void TakeDamage(float _damage)
    {

        if (!isDead)
        {
            Debug.Log(gameObject.name + " hit , health : " + health);
            animator.SetTrigger("takeDamage");
            health -= _damage;

            if (health <= 0)
            {
                InitiateDeath();
            }
        }
    }

    public void InitiateDeath()
    {
        if (!isDead)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.deadEnemies.Add(this.name);
            PlayerHealthXP.Instance.UpdateXP(maxHealth/PlayerHealthXP.Instance.XPFactor);
            isDead = true;
            animator.SetTrigger("dead");
            Destroy(this.gameObject, 5f);
        }
    }

    public void InititateDamage()
    {
        GetComponentInChildren<EnemyMeleeScript>().InitiateDamage();
    }

    public void EndInflictingDamage()
    {
        GetComponentInChildren<EnemyMeleeScript>().EndInflictingDamage();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
