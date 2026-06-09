using TMPro;
using UnityEngine;

public class TitleLerp : MonoBehaviour
{
    private TextMeshProUGUI titleText;

    public float startFontSize = 60f;
    public float endFontSize = 100f;

    public float lerpDuration = 2.0f;

    void Start()
    {
        titleText = GetComponent<TextMeshProUGUI>();

        if (titleText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject!");
        }
    }

    void Update()
    {
        if (!titleText) return;

        float t = Mathf.PingPong(Time.time / lerpDuration, 1f);

        t = Mathf.SmoothStep(0f, 1f, t);

        titleText.fontSize = Mathf.Lerp(startFontSize, endFontSize, t);
    }
}
