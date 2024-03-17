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
}
