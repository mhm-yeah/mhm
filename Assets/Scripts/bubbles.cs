using UnityEngine;

public class FloatingOrb : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatHeight = 20f;
    public float fadeSpeed = 2f;

    private Vector3 startPosition;
    private float timeOffset;
    private UnityEngine.UI.Image orbImage;

    void Start()
    {
        startPosition = transform.localPosition;
        timeOffset = Random.Range(0f, 10f);
        orbImage = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        float yOffset = Mathf.Sin((Time.time + timeOffset) * floatSpeed) * floatHeight;
        transform.localPosition = startPosition + new Vector3(0, yOffset, 0);

        float alpha = (Mathf.Sin(Time.time * fadeSpeed + timeOffset) + 1f) / 2f;
        alpha = alpha * 0.3f;

        if (orbImage != null)
        {
            Color color = orbImage.color;
            color.a = alpha;
            orbImage.color = color;
        }
    }
}