using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : Pickup
{
    public int expValue;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceLevelController.instance.GetExp(expValue);

            Destroy(gameObject);
        }
    }
}
