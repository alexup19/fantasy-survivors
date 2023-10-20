using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;
    public float lifeTime;
    public float floatSpeed = 1f;

    private float lifeCounter;

    // Update is called once per frame
    void Update()
    {
        if (lifeCounter > 0)
        {
            lifeCounter -= Time.deltaTime;

            if (lifeCounter <= 0)
            {
                //Destroy(gameObject);

                DamageNumberController.instance.PlaceInPool(this);
            }
        }

        transform.position += floatSpeed * Time.deltaTime * Vector3.up;
    }

    public void Setup(int damageDisplay)
    {
        lifeCounter = lifeTime;

        damageText.text = damageDisplay.ToString();
    }
}
