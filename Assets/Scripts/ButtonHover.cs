using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagicalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Animation")]
    public float hoverScale = 1.35f;
    public float clickScale = 0.95f;
    public float animationSpeed = 0.15f;

    private Vector3 originalScale;
    private Vector3 baseScale;
    private Vector3 targetScale;
    void Start()
    {
        originalScale = transform.localScale;
        baseScale = transform.localScale;
        targetScale = baseScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        originalScale = originalScale * hoverScale;
        targetScale = baseScale * hoverScale;
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        originalScale = originalScale / hoverScale;
        targetScale = baseScale;
       
    }

}