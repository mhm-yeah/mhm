using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public InputActionReference mousePositionAction;
    private GameManager gameManager;
    private GameObject hands;
    private Vector2 mousePosition;
    private Weapon[] weapons;
    private int weaponInd = 0;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hands = transform.Find("Hands").gameObject;
        weapons = transform.GetComponents<Weapon>();
    }

    void Update()
    {
        if (gameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeWeapon();
        }
        
        mousePosition = mousePositionAction.action.ReadValue<Vector2>();
        Vector2 mousePos = cam.ScreenToWorldPoint(mousePosition);
        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        hands.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ChangeWeapon()
    {
        weapons[weaponInd].enabled = false;
        weaponInd = (weaponInd + 1) % weapons.Length;
        weapons[weaponInd].enabled = true;
    }
}
