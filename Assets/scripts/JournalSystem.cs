using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalSystem : MonoBehaviour
{
    [Header("Referanslar")]
    public InventoryUI inventoryUI;
    public FpsController fpsController;

    [Header("UI Panelleri")]
    public GameObject mainJournalPanel;
    public GameObject passwordPanel;
    public GameObject contentPanel;

    [Header("Çarklı Kilit Sistemi")]
    // 4 tane sayının Text'ini buraya bağlayacağız
    public TextMeshProUGUI[] digitTexts;

    // O anki şifre durumu
    private int[] currentDigits = { 0, 0, 0, 0 };

    [Header("İçerik UI")]
    public Image pageImageDisplay;
    public Button nextButton;
    public Button prevButton;
    public TextMeshProUGUI errorText; // Hata mesajı için (Opsiyonel)

    public bool isUiOpen = false;

    private PickupItem currentJournalItem;
    private int currentPageIndex = 0;

    void Start()
    {
        CloseJournal();
    }

    void Update()
    {
        // R tuşu
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isUiOpen) CloseJournal();
            else TryOpenJournal();
        }

        // ESC tuşu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isUiOpen) CloseJournal();
        }
    }

    void TryOpenJournal()
    {
        PickupItem item = inventoryUI.GetSelectedItem();
        if (item == null || !item.isJournal) return;

        currentJournalItem = item;
        OpenUI();
    }

    void OpenUI()
    {
        isUiOpen = true;
        if (mainJournalPanel != null) mainJournalPanel.SetActive(true);

        if (fpsController != null) fpsController.cameraFreeze = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (currentJournalItem.isLocked)
        {
            if (passwordPanel != null) passwordPanel.SetActive(true);
            if (contentPanel != null) contentPanel.SetActive(false);

            // Şifre ekranını sıfırla
            ResetLock();
            if (errorText) errorText.text = "";
        }
        else
        {
            currentPageIndex = 0;
            ShowContent();
        }
    }

    public void CloseJournal()
    {
        isUiOpen = false;
        if (mainJournalPanel != null) mainJournalPanel.SetActive(false);
        if (passwordPanel != null) passwordPanel.SetActive(false);
        if (contentPanel != null) contentPanel.SetActive(false);

        if (fpsController != null) fpsController.cameraFreeze = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- KİLİT MEKANİĞİ ---

    void ResetLock()
    {
        currentDigits = new int[] { 0, 0, 0, 0 };
        UpdateDigitDisplay();
    }

    // Butonlara tıklayınca sadece sayıyı değiştirir, kontrol ETMEZ.
    public void ChangeDigit(int digitIndex)
    {
        currentDigits[digitIndex]++;

        if (currentDigits[digitIndex] > 9)
            currentDigits[digitIndex] = 0;

        UpdateDigitDisplay();
    }

    void UpdateDigitDisplay()
    {
        for (int i = 0; i < digitTexts.Length; i++)
        {
            if (digitTexts[i] != null)
                digitTexts[i].text = currentDigits[i].ToString();
        }
    }

    // --- YENİ: ONANYLA BUTONU İÇİN FONKSİYON ---
    public void ConfirmPassword()
    {
        if (currentJournalItem == null) return;

        // Sayı dizisini String'e çevir
        string currentCodeString = "";
        foreach (int d in currentDigits)
        {
            currentCodeString += d.ToString();
        }

        // Doğru mu?
        if (currentCodeString == currentJournalItem.journalPassword)
        {
            currentJournalItem.isLocked = false;
            ShowContent();
        }
        else
        {
            // Yanlışsa
            if (errorText != null) errorText.text = "Hatalı Şifre!";
            Debug.Log("Girilen şifre yanlış: " + currentCodeString);
        }
    }

    // --- SAYFA SİSTEMİ ---
    void ShowContent()
    {
        if (passwordPanel != null) passwordPanel.SetActive(false);
        if (contentPanel != null) contentPanel.SetActive(true);
        UpdatePageImage();
    }

    void UpdatePageImage()
    {
        if (currentJournalItem.journalPages == null || currentJournalItem.journalPages.Length == 0) return;

        pageImageDisplay.sprite = currentJournalItem.journalPages[currentPageIndex];

        if (prevButton != null) prevButton.gameObject.SetActive(currentPageIndex > 0);
        if (nextButton != null) nextButton.gameObject.SetActive(currentPageIndex < currentJournalItem.journalPages.Length - 1);
    }

    public void NextPage()
    {
        if (currentJournalItem.journalPages == null) return;
        if (currentPageIndex < currentJournalItem.journalPages.Length - 1)
        {
            currentPageIndex++;
            UpdatePageImage();
        }
    }

    public void PrevPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageImage();
        }
    }
}