using DG.Tweening;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class LookAtPlayer : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(GetComponentInParent<WeepingAngel>().playerList[0].transform.position);
    }
}
