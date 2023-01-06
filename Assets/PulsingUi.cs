using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PulsingUi : MonoBehaviour
{
    private void FixedUpdate()
    {
        var scale = Mathf.PingPong(Time.time / 5, 0.1f) + 0.9f;
        transform.localScale = Vector3.one * scale;
    }
}