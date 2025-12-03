using UnityEngine;

public class BreakableVase : MonoBehaviour
{
    [Header("Vazo Objeleri")]
    public GameObject intactVase;
    public GameObject brokenVase;

    [Header("Anahtar")]
    public GameObject keyObject;

    private bool isBroken = false;

    public void Break()
    {
        if (isBroken) return;
        isBroken = true;

        Debug.Log("Vazo kırıldı!");

        // 1) Sağlam vazoyu gizle
        intactVase.SetActive(false);

        // 2) Kırık vazo hazır pozisyonda aktifleştir
        brokenVase.SetActive(true);

        // 3) Kırık parçaları yere düşür
        Rigidbody[] parts = brokenVase.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in parts)
        {
            rb.isKinematic = false;   // Fizik aktif
            rb.useGravity  = true;
        }

        // 4) Anahtarı aktif et
        if (keyObject != null)
            keyObject.SetActive(true);
    }
}
