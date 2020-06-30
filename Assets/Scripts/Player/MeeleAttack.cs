using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    public float timeBtwAttackValue;
    private float timeBtwAttackKeeper;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public float damage;
    public Animator animator;

    private void Update()
    {
        if (timeBtwAttackKeeper <= 0 && Input.GetKey(KeyCode.Y))
        {
            timeBtwAttackKeeper = timeBtwAttackValue;
            animator.SetBool("isAttacking", true);
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].GetComponent<EnemyController>() != null)
                    enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
        else
        {
            timeBtwAttackKeeper -= Time.deltaTime;
            animator.SetBool("isAttacking", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
