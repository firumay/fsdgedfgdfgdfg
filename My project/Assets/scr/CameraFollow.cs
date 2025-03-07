using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Переменная для персонажа, за которым будет следовать камера
    public float smoothSpeed = 0.125f; // Скорость сглаживания движения камеры
    public Vector3 offset; // Смещение камеры (например, чтобы она была выше персонажа)

    void LateUpdate()
    {
        if (target != null)
        {
            // Вычисляем желаемую позицию камеры
            Vector3 desiredPosition = target.position + offset;
            // Сглаживаем движение
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}