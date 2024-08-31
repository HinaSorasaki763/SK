using UnityEngine;

public class Indicator : MonoBehaviour
{
    private Vector3 initPos;
    public GameObject indicatorText;

    public void ResetPos(Vector3 pos)
    {
        initPos = pos;
        transform.position = pos;
    }

    private void Update()
    {
        transform.position = new Vector3(initPos.x, initPos.y + Mathf.Sin(Time.time * 2f) * 0.15f, initPos.z);

        if (indicatorText != null)
        {
            indicatorText.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.25f);
        }
    }
}
