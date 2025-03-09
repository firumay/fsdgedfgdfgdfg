using UnityEngine;
 // Для работы с Light 2D
using System.Collections; // Добавлено для IEnumerator

public class LightOffTriggerScript : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D targetLight; // Ссылка на Light 2D, который нужно выключить
    public float fadeSpeed = 1f; // Скорость затухания света
    private AudioSource audioSource; // Ссылка на звук лопнувшей лампы
    private bool hasTriggered = false; // Флаг, чтобы срабатывало один раз

    void Start()
    {
        // Находим AudioSource на этом объекте
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource не найден на объекте!");
        }

        // Проверяем, что Light 2D привязан
        if (targetLight == null)
        {
            Debug.LogError("Light 2D не привязан!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в триггер вошёл игрок
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Срабатывает только один раз

            // Запускаем выключение света и звук
            StartCoroutine(FadeOutLight());
        }
    }

    IEnumerator FadeOutLight()
    {
        // Проигрываем звук лопнувшей лампы
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Затухание света
        if (targetLight != null)
        {
            while (targetLight.intensity > 0f)
            {
                targetLight.intensity -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
            targetLight.intensity = 0f; // Убеждаемся, что свет полностью выключен
        }
    }
}