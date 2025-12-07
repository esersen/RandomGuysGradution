using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // <-- Bu kütüphane şart!
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
    public TextMeshProUGUI[] digitTexts;
    private int[] currentDigits = { 0, 0, 0, 0 };

    [Header("İçerik UI")]
    public Image pageImageDisplay;
    public Button nextButton;
    public Button prevButton;
    public TextMeshProUGUI errorText;

    [Header("Olaylar (Events)")]
    // Şifre doğru girildiğinde ne olsun? (Inspector'dan seçeceğiz)
    public UnityEvent onPasswordCorrect;

    public bool isUiOpen = false;

    private PickupItem currentJournalItem;
    private int currentPageIndex = 0;

    // Şifre daha önce çözüldü mü kontrolü (Tekrar tekrar çalışmasın)
    private bool isSolved = false;

    void Start()
    {
        CloseJournal();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isUiOpen) CloseJournal();
            else TryOpenJournal();
        }

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

    void ResetLock()
    {
        currentDigits = new int[] { 0, 0, 0, 0 };
        UpdateDigitDisplay();
    }

    public void ChangeDigit(int digitIndex)
    {
        currentDigits[digitIndex]++;
        if (currentDigits[digitIndex] > 9) currentDigits[digitIndex] = 0;
        UpdateDigitDisplay();
    }

    void UpdateDigitDisplay()
    {
        for (int i = 0; i < digitTexts.Length; i++)
        {
            if (digitTexts[i] != null) digitTexts[i].text = currentDigits[i].ToString();
        }
    }

    public void ConfirmPassword()
    {
        if (currentJournalItem == null) return;

        string currentCodeString = "";
        foreach (int d in currentDigits) currentCodeString += d.ToString();

        if (currentCodeString == currentJournalItem.journalPassword)
        {
            // Şifre DOĞRU
            currentJournalItem.isLocked = false;

            // --- OLAY TETİKLEME ---
            // Eğer daha önce çözülmediyse Olayı çalıştır (Kapı aç, ses çal vs.)
            if (!isSolved)
            {
                isSolved = true;
                onPasswordCorrect.Invoke();
                Debug.Log("Şifre doğru! Gizli bölme açılıyor...");
            }
            // ----------------------

            ShowContent();
        }
        else
        {
            if (errorText != null) errorText.text = "Hatalı Şifre!";
        }
    }

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