using UnityEngine;

public class playerM : MonoBehaviour
{
    public float speed = 5f; // Скорость персонажа

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // Влево/вправо (A/D или стрелки)
        float moveY = Input.GetAxis("Vertical");   // Вверх/вниз (W/S или стрелки)

        Vector2 movement = new Vector2(moveX, moveY).normalized * speed * Time.deltaTime;
        transform.position += (Vector3)movement; // Двигаем персонажа
    }
}
