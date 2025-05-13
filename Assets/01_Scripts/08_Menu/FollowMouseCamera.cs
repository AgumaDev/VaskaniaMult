using UnityEngine;
public class FollowMouseCamera : MonoBehaviour
{
    Vector3 newPosition;
    
	void Start () 
    {
        newPosition = transform.position;
	}
    void Update()
    {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                newPosition = hit.point;
                transform.position = newPosition;
            }
        
    }
}
