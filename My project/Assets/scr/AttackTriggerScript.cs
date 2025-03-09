using UnityEngine;
using System.Collections;

public class AttackTriggerScript : MonoBehaviour
{
    public GameObject enemyObject; // Ссылка на объект врага
    public GameObject player; // Ссылка на игрока
    public Camera mainCamera; // Ссылка на главную камеру
    public float moveSpeed = 5f; // Скорость движения врага
    public float moveDistance = 5f; // Дистанция, на которую враг пробежит
    public float soundDelay = 0.5f; // Задержка перед воспроизведением звука (в секундах)
    public float flipDelay = 0.3f; // Задержка перед отражением (в секундах)
    public float zoomDelay = 0.5f; // Задержка перед началом приближения камеры
    public float zoomSpeed = 1f; // Скорость приближения камеры
    public float targetZoomSize = 2f; // Целевой размер камеры (меньше = ближе)
    private float initialZoomSize; // Начальный размер камеры
    private bool hasTriggered = false; // Флаг, чтобы срабатывало один раз

    void Start()
    {
        // Убедимся, что объект врага неактивен изначально
        if (enemyObject != null)
        {
            enemyObject.SetActive(false);
        }

        // Находим главную камеру, если она не привязана
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Сохраняем начальный размер камеры
        if (mainCamera != null)
        {
            initialZoomSize = mainCamera.orthographicSize;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в триггер вошёл игрок
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Срабатывает только один раз

            // Запускаем последовательность событий
            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        // Блокируем движение игрока
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        // Активируем врага и двигаем его вправо
        enemyObject.SetActive(true);
        Animator enemyAnimator = enemyObject.GetComponent<Animator>();
        Vector3 startPosition = enemyObject.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(moveDistance, 0, 0); // Движение вправо

        while (Vector3.Distance(enemyObject.transform.position, targetPosition) > 0.1f)
        {
            enemyObject.transform.position = Vector3.MoveTowards(enemyObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Запускаем анимацию атаки врага
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Attack");

            // Ждём заданную задержку перед воспроизведением звука
            yield return new WaitForSeconds(soundDelay);

            // Проигрываем звук выстрела
            AudioSource audioSource = enemyObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }

        // Ждём завершения анимации атаки (настрой под свою анимацию)
        yield return new WaitForSeconds(1f - soundDelay);

        // Запускаем анимацию падения персонажа
        if (playerMovement != null)
        {
            playerMovement.PlayFallAnimation();
        }

        // Ждём завершения анимации падения
        yield return new WaitForSeconds(1f);

        // Ждём задержку перед отражением
        yield return new WaitForSeconds(flipDelay);

        // Отражаем врага по горизонтали после задержки
        SpriteRenderer enemySprite = enemyObject.GetComponent<SpriteRenderer>();
        if (enemySprite != null)
        {
            enemySprite.flipX = true; // Отражаем спрайт по горизонтали
        }

        // Ждём задержку перед приближением камеры
        yield return new WaitForSeconds(zoomDelay);

        // Приближаем камеру
        if (mainCamera != null)
        {
            float currentZoom = mainCamera.orthographicSize;
            while (Mathf.Abs(currentZoom - targetZoomSize) > 0.01f)
            {
                currentZoom = Mathf.MoveTowards(currentZoom, targetZoomSize, zoomSpeed * Time.deltaTime);
                mainCamera.orthographicSize = currentZoom;
                yield return null;
            }
        }

        // Двигаем врага обратно влево
        Vector3 returnPosition = startPosition; // Возвращаемся в начальную позицию
        while (Vector3.Distance(enemyObject.transform.position, returnPosition) > 0.1f)
        {
            enemyObject.transform.position = Vector3.MoveTowards(enemyObject.transform.position, returnPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Отключаем врага после возвращения
        enemyObject.SetActive(false);
    }
}