using UnityEngine;
using TMPro;

public class BlinkingUI : MonoBehaviour
{
    public TextMeshProUGUI textToBlink;
    public float blinkDuration = 1.0f;

    private float timer;
    private bool isBlinking = true;

    void Update()
    {
        if (!isBlinking) return;

        timer += Time.deltaTime;
        textToBlink.alpha = Mathf.Abs(Mathf.Sin(timer / blinkDuration * Mathf.PI));

        if (timer >= blinkDuration)
        {
            timer = 0f;
        }
    }

    public void ToggleBlinking(bool state)
    {
        isBlinking = state;
        textToBlink.alpha = state ? textToBlink.alpha : 1f;
    }
}
