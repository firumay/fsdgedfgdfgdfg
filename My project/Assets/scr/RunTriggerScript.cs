using UnityEngine;
using System.Collections; // Добавлено для IEnumerator

public class RunTriggerScript : MonoBehaviour
{
    public GameObject runningObject; // Ссылка на объект, который будет пробегать
    public float moveSpeed = 5f; // Скорость движения объекта
    public float moveDistance = 10f; // Дистанция, на которую объект пробежит
    private bool hasTriggered = false; // Флаг, чтобы срабатывало один раз

    void Start()
    {
        // Убедимся, что объект неактивен изначально
        if (runningObject != null)
        {
            runningObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в триггер вошёл игрок
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Устанавливаем флаг, чтобы сработало только один раз

            // Активируем объект и запускаем движение
            if (runningObject != null)
            {
                runningObject.SetActive(true);
                StartCoroutine(MoveAndDisable(runningObject));
            }
        }
    }

    IEnumerator MoveAndDisable(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsRunning", true); // Запускаем анимацию (если есть параметр)
        }

        // Начальная позиция
        Vector3 startPosition = obj.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(moveDistance, 0, 0); // Движение вправо

        // Двигаем объект
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Останавливаем анимацию и отключаем объект
        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
        }
        obj.SetActive(false);
    }
}