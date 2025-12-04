using UnityEngine;

public class HeldItemFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public Vector3 offsetRot;

    void LateUpdate()
    {
        if (target == null) return;

        // Yalnızca pozisyonu takip et
        transform.position = target.position + target.rotation * offsetPos;

        // ROTASYONU SABİT TUT
        transform.rotation = Quaternion.Euler(offsetRot);
    }
}
