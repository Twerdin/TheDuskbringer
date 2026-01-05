using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Eðer yazý göstereceksen

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI infoText; // "Devam etmek için tuþa bas" yazýsý
    private bool canRespawn = false; // Tuþa basýlabilir mi?

    void Start()
    {
        // Baþlangýçta yazýyý gizle (veya "Bekleyin..." yaz)
        if (infoText != null) infoText.text = "Ruhun Yenileniyor...";

        // 5 Saniyelik sayacý baþlat
        StartCoroutine(WaitAndEnableRespawn());
    }

    IEnumerator WaitAndEnableRespawn()
    {
        yield return new WaitForSeconds(5f); // 5 saniye bekle

        canRespawn = true; // Artýk tuþa basabilir

        if (infoText != null)
            infoText.text = "Yeniden Doðmak Ýçin Herhangi Bir Tuþa Bas";
    }

    void Update()
    {
        // Süre dolduysa VE herhangi bir tuþa basýldýysa
        if (canRespawn && Input.anyKeyDown)
        {
            // Kayýtlý sahne ismini al
            string sceneToLoad = PlayerPrefs.GetString("SavedLevel", "Level1"); // Kayýt yoksa Level1 açar
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}