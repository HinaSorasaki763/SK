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
        if (isBlinking)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Abs(Mathf.Sin(timer / blinkDuration * Mathf.PI));
            textToBlink.alpha = alpha;

            if (timer >= blinkDuration)
            {
                timer = 0f;
            }
        }
    }

    // 用來控制開始和停止閃爍
    public void ToggleBlinking(bool state)
    {
        isBlinking = state;
        if (!isBlinking)
        {
            textToBlink.alpha = 1; // 確保停止閃爍時文本是可見的
        }
    }
}
