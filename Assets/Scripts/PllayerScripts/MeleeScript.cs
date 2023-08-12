using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    bool canDamage;

    [SerializeField] float weaponLength;
    public float damageFactor = 5;

    List<GameObject> enemiesHit;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemiesHit = new List<GameObject>();
        canDamage = false;     
    }

    void Update()
    {
        if(canDamage)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, weaponLength, LayerMask.GetMask("Enemies")))
            {
                if (hit.transform.TryGetComponent(out EnemyScript enemy) && !enemiesHit.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(gameManager.playerStats.strength * damageFactor);

                    enemiesHit.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void InitiateDamage()
    {
        canDamage = true;
        enemiesHit.Clear();
    }

    public void EndInflictingDamage()
    {
        canDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * weaponLength);
    }
}
