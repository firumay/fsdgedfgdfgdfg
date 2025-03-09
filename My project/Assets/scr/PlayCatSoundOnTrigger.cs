using UnityEngine;

public class PlayCatSoundOnTrigger : MonoBehaviour
{
    private AudioSource catSound; // Ссылка на AudioSource
    private bool hasPlayed = false; // Флаг, чтобы звук проигрывался только один раз

    void Start()
    {
        // Находим компонент AudioSource на этом объекте
        catSound = GetComponent<AudioSource>();
        if (catSound == null)
        {
            Debug.LogError("AudioSource не найден на объекте!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в триггер вошёл игрок
        if (other.CompareTag("Player") && !hasPlayed)
        {
            // Проигрываем звук
            if (catSound != null)
            {
                catSound.Play();
                hasPlayed = true; // Устанавливаем флаг, чтобы звук больше не проигрывался
            }
        }
    }
}