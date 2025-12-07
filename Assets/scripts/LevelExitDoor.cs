using UnityEngine;
using UnityEngine.SceneManagement; // Sahne değişimi için şart

public class LevelExitDoor : MonoBehaviour
{
    [Header("Gerekli Anahtar")]
    public string requiredKeyName = "Ev Anahtari"; // Anahtarın Item Name'i ile BİREBİR aynı olmalı

    [Header("Gidilecek Sahne")]
    public string nextSceneName = "Level2"; // Sonraki bölümün dosya adı

    // Bu fonksiyonu ItemPickupSystem çağıracak
    public void TryOpenDoor(PickupItem heldItem)
    {
        // 1. Elde hiç eşya yoksa
        if (heldItem == null)
        {
            Debug.Log("Kapı kilitli! Bir anahtara ihtiyacım var.");
            return;
        }

        // 2. Eldeki eşya doğru anahtar mı?
        if (heldItem.itemName == requiredKeyName)
        {
            Debug.Log("Tebrikler! Bölüm geçiliyor...");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("Bu eşya kapıyı açmaz: " + heldItem.itemName);
        }
    }
}