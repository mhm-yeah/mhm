using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimation : MonoBehaviour
{
    public Sprite[] front;
    public Sprite[] back;
    public Sprite[] side;

    public float frameRate = 10f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float timer;
    private int frameIndex;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 move = rb.linearVelocity;

        if (move.sqrMagnitude < 0.01f)
        {
            frameIndex = 0;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer = 0;
            frameIndex++;
        }

        Sprite[] currentAnim;

        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            currentAnim = side;
            sr.flipX = move.x < 0;
        }
        else
        {
            sr.flipX = false;

            if (move.y > 0)
                currentAnim = back;
            else
                currentAnim = front;
        }

        frameIndex %= currentAnim.Length;
        sr.sprite = currentAnim[frameIndex];
    }
}