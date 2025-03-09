using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость персонажа
    private Animator animator; // Ссылка на Animator
    private SpriteRenderer spriteRenderer; // Для отражения спрайта
    private bool canMove = true; // Флаг для управления движением

    void Start()
    {
        animator = GetComponent<Animator>(); // Находим Animator
        spriteRenderer = GetComponent<SpriteRenderer>(); // Находим Sprite Renderer

        // Сбрасываем параметры при старте
        animator.SetBool("isWalking", false);
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", 0);
    }

    void Update()
    {
        if (canMove) // Движение только если разрешено
        {
            float moveX = Input.GetAxisRaw("Horizontal"); // -1 (лево), 0, 1 (право)
            float moveY = Input.GetAxisRaw("Vertical");   // -1 (вниз), 0, 1 (вверх)

            // Движение
            Vector2 movement = new Vector2(moveX, moveY).normalized * speed * Time.deltaTime;
            transform.position += (Vector3)movement;

            // Управление анимацией
            if (moveX != 0 || moveY != 0) // Если двигается
            {
                animator.SetBool("isWalking", true); // Включаем анимацию
                animator.SetFloat("moveX", moveX);   // Направление по X
                animator.SetFloat("moveY", moveY);   // Направление по Y

                // Отражение спрайта для ходьбы влево
                spriteRenderer.flipX = (moveX < 0); // Отражаем, если идём влево
            }
            else
            {
                animator.SetBool("isWalking", false); // Выключаем анимацию, если стоит
                animator.SetFloat("moveX", 0);        // Сбрасываем направления
                animator.SetFloat("moveY", 0);
            }
        }
    }

    // Метод для блокировки/разблокировки движения
    public void SetCanMove(bool state)
    {
        canMove = state;
        if (!canMove)
        {
            animator.SetBool("isWalking", false); // Останавливаем анимацию ходьбы
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
        }
    }

    // Метод для проигрывания анимации падения
    public void PlayFallAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Fall"); // Запускаем анимацию падения
            SetCanMove(false); // Блокируем движение после падения
        }
    }
}