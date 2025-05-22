using UnityEngine;

public class PlayerNameFollowCamera : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        Vector3 direction = transform.position - Camera.main.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}