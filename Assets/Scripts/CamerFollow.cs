using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        if (target == null)
            return;

        float x = Mathf.Clamp(target.position.x, -2.1f, 2.1f);
        float y = Mathf.Clamp(target.position.y, -0.7f, 32.8f);

        transform.position = new Vector3(
            x,
            y,
            transform.position.z
        );
    }
}

