using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowVolley : Ability
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private int arrowCount = 5;
    [SerializeField] private float spreadAngle = 30f;

    private Camera mainCam;
    private GameObject projectilesFolder;

    void Start()
    {
        mainCam = Camera.main;
        projectilesFolder = GameObject.Find("Projectiles");
    }

    private void OnSpellCast(InputValue input)
    {
        if (input.isPressed)
        {
            FireVolley();
        }
    }

    void FireVolley()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue(); //fire where mouse cursor is
        Vector3 mousePos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (arrowCount - 1);

        for (int i = 0; i < arrowCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 rotatedDir = RotateVector(direction, angle);
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, projectilesFolder.transform);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = rotatedDir * arrowSpeed;
            }
            //rotate arrow to face 
            float rotZ = Mathf.Atan2(rotatedDir.y, rotatedDir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90f); //due to sprite rotation being up
        }
    }

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        return new Vector2(
            cos * v.x - sin * v.y,
            sin * v.x + cos * v.y
        );
    }
}