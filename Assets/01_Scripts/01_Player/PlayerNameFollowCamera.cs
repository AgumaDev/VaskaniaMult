using UnityEngine;

public class PlayerNameFollowCamera : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        Vector3 direction = transform.position - Camera.main.transform.position;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
