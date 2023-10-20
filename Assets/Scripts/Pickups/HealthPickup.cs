using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public int healthPoints = 15;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealthController.instance.Heal(healthPoints);

            SFXManager.instance.PlaySFX(11);

            Destroy(gameObject);
        }
    }
}
