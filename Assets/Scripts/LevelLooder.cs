using UnityEngine;
using UnityEngine.SceneManagement; // <-- Sahne geçiþi için BU ÞART!

public class LevelLoader : MonoBehaviour
{
    [Header("Gidilecek Sahne Adý")]
    public string sceneName; // Inspector'dan gideceðimiz sahnenin adýný yazacaðýz

    // Karakter kapýya deðdiðinde çalýþýr
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer çarpan obje "Player" etiketine sahipse
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
