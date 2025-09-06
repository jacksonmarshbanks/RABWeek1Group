using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;
    private Vector3 originalScale;

    // This script doesn't work yet, I might just get rid of it. It's supposed to make buttons shrink and grow.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => AnimateButton());
        }
    }

    public void Awake()
    {
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void AnimateButton()
    {
        // AudioManager.instance.PlaySound(AudioManager.instance.placeholder);
        transform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.OutQuad) // Shrink
            .OnComplete(() => transform.DOScale(originalScale, 0.1f).SetEase(Ease.OutQuad)); // Grow back
    }
}