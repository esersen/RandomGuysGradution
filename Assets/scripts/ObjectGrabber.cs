using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public float holdDistance = 2f;
    public float smooth = 10f;

    private Transform playerCam;
    private Rigidbody currentObj;
    private bool holding = false;

    void Start()
    {
        playerCam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!holding)
                TryGrab();
            else
                Drop();
        }

        if (holding && currentObj != null)
        {
            Vector3 targetPos = playerCam.position + playerCam.forward * holdDistance;
            currentObj.MovePosition(Vector3.Lerp(currentObj.position, targetPos, Time.deltaTime * smooth));
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(playerCam.position, playerCam.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                currentObj = rb;
                currentObj.useGravity = false;
                currentObj.freezeRotation = true;
                holding = true;
            }
        }
    }

    void Drop()
    {
        if (currentObj != null)
        {
            currentObj.useGravity = true;
            currentObj.freezeRotation = false;
            currentObj = null;
        }
        holding = false;
    }
}
