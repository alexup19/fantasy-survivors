using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    public float damage;
    public float hitWaitTime = 1f;
    public float health = 5f;
    public float knockBackTime = .5f;
    public int expToGive = 1;

    public int coinValue = 1;
    public float coinDropRate = .5f;

    private float knockBackCounter;
    private Transform target;
    private float hitCounter;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerHealthController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.gameObject.activeSelf == true)
        {

            if (knockBackCounter > 0f)
            {
                knockBackCounter -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2f;
                }

                if (knockBackCounter <= 0f)
                {
                    moveSpeed = Mathf.Abs(moveSpeed * .5f);
                }
            }

            theRB.velocity = (target.position - transform.position).normalized * moveSpeed;

            if (hitCounter > 0)
            {
                hitCounter -= Time.deltaTime;
            }
        } else
        {
            theRB.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);

            hitCounter = hitWaitTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;

        if (health <= 0)
        {
            Destroy(gameObject);

            ExperienceLevelController.instance.SpawnExp(transform.position, expToGive);

            float randomValue = Random.value;

            if (randomValue < coinDropRate)
            {
                Vector3 offsetPosition = new(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                CoinController.instance.DropCoin(transform.position + offsetPosition, coinValue);
            }

            SFXManager.instance.PlaySFXPitched(0);
        } else
        {
            SFXManager.instance.PlaySFXPitched(1);
        }

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
    }

    public void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        TakeDamage(damageToTake);

        if (shouldKnockBack == true)
        {
            knockBackCounter = knockBackTime;
        }
    }
}