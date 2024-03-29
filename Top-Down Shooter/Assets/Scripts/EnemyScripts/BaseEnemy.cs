using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private static int instances = 0;
    public static GameObject player;
    protected float health = 100f;
    private Rigidbody2D rb;
    const float knockbackDelay = 0.15f;

    protected BaseEnemy()
    {
    }

    // Start is called before the first frame update
    protected void Start()
    {
        instances++;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        //face player
        transform.rotation = Utils.GetRelativeRotation(transform.position, player.transform.position);
    }


    /// <summary>
    /// Should be called when the enemy is hit.
    /// </summary>
    /// <param name="senderTransform"> - position of the hitter </param>
    /// <param name="knockbackStrength"> - strength of the knockback dealt </param>
    /// <param name="hitValue"> - damage dealt by the hit</param>
    public void TakeHit(Transform senderTransform, float knockbackStrength, float hitValue)
    {
        health -= hitValue;

        if (health <= 0)
        {
            Die();
            return;
        }

        TakeKnockback(senderTransform, knockbackStrength);
    }


    private void TakeKnockback(Transform senderTransform, float strength)
    {
        StopAllCoroutines();
        Vector2 direction = (transform.position - senderTransform.position).normalized;
        direction *= strength;
        rb.AddForce(direction, ForceMode2D.Impulse);
        StartCoroutine(KnockbackReset(direction)); 
    }

    private IEnumerator KnockbackReset(Vector2 force)
    {
        yield return new WaitForSeconds(knockbackDelay);
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Should be called on death.
    /// </summary>
    protected void Die()
    {
        Destroy(gameObject);
        instances--;
    }

    public static int GetNumberOfInstances()
    {
        return instances;
    }
}
