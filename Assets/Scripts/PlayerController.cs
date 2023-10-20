using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    public float moveSpeed;

    public Animator anim;
    public float pickupRange = 1.5f;
    public float luck = 0.25f;
    public SpriteRenderer sprite;

    //public Weapon activeWeapon;

    public List<Weapon> unassignedWeapons, assignedWeapons;

    public Tilemap tilemap;

    [HideInInspector]
    public List<Weapon> fullyLeveledWeapons = new List<Weapon>();

    public int maxWeapons = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }

        moveSpeed = PlayerStatController.instance.moveSpeed[0].value;
        pickupRange = PlayerStatController.instance.pickupRange[0].value;
        maxWeapons = Mathf.RoundToInt(PlayerStatController.instance.maxWeapons[0].value);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        if (moveInput.x != 0)
        {
            if (moveInput.x > 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }

        transform.position += moveInput * moveSpeed * Time.deltaTime;

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, tilemap.CellToWorld(tilemap.cellBounds.min).x, tilemap.CellToWorld(tilemap.cellBounds.max).x), Mathf.Clamp(transform.position.y, tilemap.CellToWorld(tilemap.cellBounds.min).y, tilemap.CellToWorld(tilemap.cellBounds.max).y));

        if (moveInput != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void AddWeapon(int weaponNumber)
    {
        if (weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

    public void AddWeapon(Weapon weaponToAdd)
    {
        weaponToAdd.gameObject.SetActive(true);
        assignedWeapons.Add(weaponToAdd);
        unassignedWeapons.Remove(weaponToAdd);
    }

    public void BlinkSprite()
    {
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        sprite.enabled = false;

        yield return new WaitForSeconds(0.1f);

        sprite.enabled = true;
    }
}
