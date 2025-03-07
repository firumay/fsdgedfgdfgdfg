using UnityEngine;
using TMPro;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    private bool isPlayerInRange;
    public string dialogueText = "Привет! Это жуткое место, правда?";
    public GameObject dialogueBox; // Ссылка на "DialogueText"
    private TMPro.TextMeshProUGUI textComponent;
    private AudioSource audioSource; // Для фонового звука диалога
    public float typingSpeed = 0.1f; // Скорость появления текста (в секундах на букву)
    private Coroutine typingCoroutine; // Для управления корутиной

    void Start()
    {
        if (dialogueBox == null)
            dialogueBox = GameObject.Find("DialogueText");
        textComponent = dialogueBox.GetComponent<TMPro.TextMeshProUGUI>();
        audioSource = dialogueBox.GetComponent<AudioSource>(); // Находим Audio Source
        dialogueBox.SetActive(false); // Скрываем при старте
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueBox.activeSelf)
            {
                ShowDialogue();
            }
            else
            {
                HideDialogue();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Игрок вошёл в зону!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Игрок вышел из зоны!");
        }
    }

    void ShowDialogue()
    {
        dialogueBox.SetActive(true);
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine); // Останавливаем предыдущую корутину
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // Запускаем звук
        }
        typingCoroutine = StartCoroutine(TypeText(dialogueText));
    }

    void HideDialogue()
    {
        dialogueBox.SetActive(false);
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine); // Останавливаем корутину
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Останавливаем звук
        }
    }

    IEnumerator TypeText(string fullText)
    {
        textComponent.text = ""; // Очищаем текст
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // Запускаем звук
        }
        foreach (char letter in fullText)
        {
            textComponent.text += letter; // Добавляем букву
            yield return new WaitForSeconds(typingSpeed); // Ждём перед следующей буквой
        }
        // Останавливаем звук, когда текст закончен
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Останавливаем звук после завершения текста
        }
    }
}