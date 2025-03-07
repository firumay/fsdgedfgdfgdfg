using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource audioSource;
    public float stepInterval = 0.5f;
    private float stepTimer;
    private bool isMoving;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stepTimer = stepInterval;
        audioSource.loop = false; // Убедись, что звук не зациклен
    }

    void Update()
    {
        // Проверяем движение
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        isMoving = (moveX != 0 || moveY != 0);

        if (isMoving)
        {
            // Если персонаж двигается, уменьшаем таймер
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0 && !audioSource.isPlaying) // Проверяем, не играет ли звук
            {
                audioSource.Play();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Если персонаж остановился, сбрасываем таймер и останавливаем звук
            stepTimer = stepInterval;
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}