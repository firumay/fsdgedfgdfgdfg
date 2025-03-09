using UnityEngine;
 // Для работы с Light 2D

public class BlinkingLight2D : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D light2D; // Ссылка на компонент Light 2D
    public float maxIntensity = 1.0f; // Максимальная интенсивность (включён)
    public float minIntensity = 0.0f; // Минимальная интенсивность (выключен)
    public float blinkSpeed = 0.2f; // Скорость изменения интенсивности
    public float minWaitTime = 0.1f; // Минимальное время ожидания между морганиями
    public float maxWaitTime = 0.5f; // Максимальное время ожидания между морганиями

    private float targetIntensity; // Целевая интенсивность
    private float waitTimer; // Таймер ожидания

    void Start()
    {
        // Находим компонент Light 2D
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (light2D == null)
        {
            Debug.LogError("Компонент Light 2D не найден на объекте!");
            return;
        }

        // Устанавливаем начальную интенсивность
        light2D.intensity = maxIntensity;
        targetIntensity = maxIntensity;
        SetRandomWaitTime();
    }

    void Update()
    {
        // Уменьшаем таймер ожидания
        waitTimer -= Time.deltaTime;

        // Когда таймер истёк, меняем состояние света
        if (waitTimer <= 0f)
        {
            // Переключаем цель: если свет был включён, гасим, и наоборот
            targetIntensity = (targetIntensity == maxIntensity) ? minIntensity : maxIntensity;
            SetRandomWaitTime();
        }

        // Плавно изменяем интенсивность к целевой
        light2D.intensity = Mathf.Lerp(light2D.intensity, targetIntensity, Time.deltaTime / blinkSpeed);
    }

    // Устанавливаем случайное время ожидания
    private void SetRandomWaitTime()
    {
        waitTimer = Random.Range(minWaitTime, maxWaitTime);
    }
}