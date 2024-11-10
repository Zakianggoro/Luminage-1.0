using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBase : MonoBehaviour
{
    public int maxHealthLoss = 5;
    private int currentHealthLoss = 0;
    public string levelName;

    void ChangeScene()
    {
        SceneManager.LoadScene(levelName);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek apakah objek yang bertabrakan memiliki tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Kurangi nyawa Base jika belum mencapai batas maksimum
            if (currentHealthLoss < maxHealthLoss)
            {
                currentHealthLoss++;
                Debug.Log("Base terkena serangan! Nyawa berkurang 1. Total nyawa yang hilang: " + currentHealthLoss);

                // Jika nyawa telah mencapai batas maksimum
                if (currentHealthLoss >= maxHealthLoss)
                {
                    ChangeScene();

                    Debug.Log("Base sudah kehilangan 5 nyawa. Tidak bisa kehilangan nyawa lagi.");
                }
            }
        }
    }
}
