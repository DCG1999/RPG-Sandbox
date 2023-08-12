using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
    bool canDamage;
    bool hasHit;

    [SerializeField] float weaponLength;
    private float damage;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        damage = PlayerHealthXP.Instance.maxHealth/gameManager.playerStats.strength;
        canDamage = false;
        hasHit = false;
    }

    void Update()
    {
        if(canDamage && !hasHit)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, -transform.up, out hit,weaponLength,LayerMask.GetMask("Player")))
            {
                if(hit.transform.TryGetComponent(out PlayerStateManager player))
                {
                    player.TakeDamage(damage);
                    hasHit = true;
                }
                
            }
        }
    }

    public void InitiateDamage()
    {

        canDamage = true;
        hasHit = false;
    }

    public void EndInflictingDamage()
    {
        canDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }

}
