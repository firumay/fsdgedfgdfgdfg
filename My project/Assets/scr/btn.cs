using UnityEngine;
using System.Collections;

public class ScaleAnimationTriggerScript : MonoBehaviour
{
    public GameObject scalingObject; // Ссылка на объект, который будет менять масштаб
    public float scaleSpeed = 1f; // Скорость изменения масштаба
    public float maxScale = 1.5f; // Максимальный масштаб
    public float minScale = 0.5f; // Минимальный масштаб
    private bool isScaling = false; // Флаг, идёт ли анимация масштаба
    private bool isInTrigger = false; // Флаг, находится ли игрок в триггере

    void Start()
    {
        // Убедимся, что объект неактивен изначально
        if (scalingObject != null)
        {
            scalingObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в триггер вошёл игрок
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            if (!isScaling && scalingObject != null)
            {
                scalingObject.SetActive(true); // Активируем объект
                StartCoroutine(ScaleAnimation()); // Запускаем анимацию масштаба
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, что игрок покинул триггер
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            if (scalingObject != null)
            {
                scalingObject.SetActive(false); // Деактивируем объект
                StopAllCoroutines(); // Останавливаем анимацию
            }
        }
    }

    IEnumerator ScaleAnimation()
    {
        isScaling = true;
        Vector3 targetScale = new Vector3(maxScale, maxScale, 1f); // Начальный масштаб — максимальный
        Vector3 currentScale = scalingObject.transform.localScale;

        while (isInTrigger) // Продолжаем, пока игрок в триггере
        {
            // Увеличиваем масштаб до maxScale
            while (currentScale.x < maxScale - 0.01f)
            {
                currentScale = Vector3.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);
                scalingObject.transform.localScale = currentScale;
                yield return null;
            }

            // Уменьшаем масштаб до minScale
            targetScale = new Vector3(minScale, minScale, 1f);
            while (currentScale.x > minScale + 0.01f)
            {
                currentScale = Vector3.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);
                scalingObject.transform.localScale = currentScale;
                yield return null;
            }

            // Меняем направление обратно к maxScale
            targetScale = new Vector3(maxScale, maxScale, 1f);
        }

        isScaling = false;
    }

    // Публичный метод для скрытия объекта и остановки анимации
    public void StopScalingAndHide()
    {
        isInTrigger = false; // Сбрасываем флаг триггера
        if (scalingObject != null)
        {
            scalingObject.SetActive(false); // Деактивируем объект
            StopAllCoroutines(); // Останавливаем анимацию
        }
        isScaling = false; // Сбрасываем флаг анимации
    }
}