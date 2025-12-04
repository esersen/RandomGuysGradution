using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Ayarlar")]
    public float grabRange = 3f;
    public float holdDistance = 2f;
    public float moveSpeed = 12f;
    public LayerMask grabbableMask;

    private Rigidbody heldRb;
    private Transform cam;

    private FpsController fps;

    // Döndürme
    private bool rotating = false;
    private Vector3 lastMouse;
    private Vector3 currentMouseDelta; 

    private float rotateSpeed = 10f; // Rotasyon hızı
    private float rotationFollowSpeed = 20f; // Rotasyon yumuşatma hızı (Yeni)
    private float followFactor = 0.75f; 

    void Start()
    {
        cam = Camera.main.transform;
        fps = cam.GetComponentInParent<FpsController>();
    }

    void Update()
    {
        HandleGrabInput();
        HandleRotateInput();

        if (rotating)
        {
            Vector3 delta = Input.mousePosition - lastMouse;
            currentMouseDelta += delta;
            lastMouse = Input.mousePosition;
        } 
        else
        {
            currentMouseDelta = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (heldRb != null)
        {
            if (!rotating)
            {
                MoveHeldObject();
                heldRb.angularVelocity = Vector3.zero; 
            }
            else
            {
                heldRb.linearVelocity = Vector3.zero;
            }

            if (rotating)
                RotateHeldObject();
        }
    }

    void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldRb == null)
                TryGrab();
            else
                Drop();
        }
    }

    // 🔥 DEĞİŞİKLİK: Grab Damping'i daha yumuşak ama kontrol edilebilir bir değere (5f) geri getirdik.
    void TryGrab()
    {
        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, grabbableMask))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb == null)
                rb = hit.collider.GetComponentInParent<Rigidbody>();

            if (rb == null) return;

            heldRb = rb;
            heldRb.isKinematic = false;
            heldRb.useGravity = false;
            
            // Damping değerleri geri ayarlandı.
            heldRb.linearDamping = 5f; 
            heldRb.angularDamping = 5f; 
        }
    }

    // 🔥 DEĞİŞİKLİK: Drop Damping değerleri geri ayarlandı.
    void Drop()
    {
        if (heldRb == null) return;

        heldRb.useGravity = true;
        heldRb.linearDamping = 0.05f; 
        heldRb.angularDamping = 0.05f;

        rotating = false;
        if (fps != null) fps.cameraFreeze = false;

        heldRb = null;
    }

    void MoveHeldObject()
    {
        Vector3 targetPos = cam.position + cam.forward * holdDistance;
        
        Vector3 displacement = targetPos - heldRb.position;
        
        Vector3 velocity = displacement / Time.fixedDeltaTime;

        heldRb.linearVelocity = velocity;
    }

    void HandleRotateInput()
    {
        if (heldRb == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            rotating = true;
            lastMouse = Input.mousePosition; 
            currentMouseDelta = Vector3.zero;

            if (fps != null)
                fps.cameraFreeze = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rotating = false;

            if (fps != null)
                fps.cameraFreeze = false;
        }
    }

    // 🔥 DÜZELTME: Yumuşak Slerp takibine geri döndük, Transform set'i kaldırdık.
    void RotateHeldObject()
    {
        if (currentMouseDelta.magnitude > 0) 
        {
            // Rotasyon miktarını hesapla
            Quaternion rotY = Quaternion.AngleAxis(currentMouseDelta.x * rotateSpeed, cam.up);
            Quaternion rotX = Quaternion.AngleAxis(-currentMouseDelta.y * rotateSpeed, cam.right);

            Quaternion desiredRotation = heldRb.rotation * rotY * rotX;

            // Rotasyonu yumuşakça hedef rotasyona doğru hareket ettir
            heldRb.MoveRotation(
                Quaternion.Slerp(
                    heldRb.rotation,
                    desiredRotation,
                    Time.fixedDeltaTime * rotationFollowSpeed // Sabit bir takip hızı
                )
            );
            
            // Mouse deltasını koru, çünkü Slerp her FixedUpdate'te mevcut deltaya yetişemeyebilir.
            // Bu, rotasyonun daha sürekli olmasını sağlar.
            // currentMouseDelta = Vector3.zero; // Bu satırı kaldırdık
        }
    }
}