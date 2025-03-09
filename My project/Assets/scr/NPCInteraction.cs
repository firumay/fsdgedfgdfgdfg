using UnityEngine;
using TMPro; // Для TextMeshPro (если используешь обычный Text, замени на UnityEngine.UI)
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Текст для диалога
    public TextMeshProUGUI answersText; // Текст для вариантов ответа
    public GameObject player; // Ссылка на игрока
    public float interactionDistance = 2f; // Дистанция взаимодействия
    public float textSpeed = 0.05f; // Скорость появления текста (секунд на букву)
    public AudioSource talkSound; // Фоновый звук "говорилки"

    private bool isInRange = false; // Игрок в зоне действия
    private bool isInDialogue = false; // Флаг, идёт ли диалог
    private bool isWaitingForAnswer = false; // Флаг, ждём ли выбор ответа
    private bool isTyping = false; // Флаг, идёт ли печать текста
    private string[] answers = new string[3]; // Варианты ответа
    private string[] responses = new string[3]; // Ответы NPC на выбор
    private int currentStep = 0; // Текущий шаг диалога

    void Start()
    {
        // Проверяем, всё ли привязано
        if (dialogueText == null || answersText == null)
        {
            Debug.LogError("DialogueText or AnswersText not assigned in Inspector!");
            return;
        }
        if (player == null)
        {
            Debug.LogError("Player not assigned in Inspector!");
            return;
        }
        if (talkSound == null)
        {
            Debug.LogError("TalkSound AudioSource not assigned in Inspector!");
            return;
        }

        // Отключаем UI при старте
        dialogueText.gameObject.SetActive(false);
        answersText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Проверяем дистанцию до игрока
        float distance = Vector2.Distance(transform.position, player.transform.position);
        isInRange = distance <= interactionDistance;

        // Если игрок в зоне и нажал E, начинаем диалог
        if (isInRange && !isInDialogue && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }

        // Если игрок отошёл, скрываем диалог
        if (!isInRange && isInDialogue)
        {
            EndDialogue();
        }

        // Если ждём выбор ответа
        if (isWaitingForAnswer)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowResponse(0); // Первый ответ
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShowResponse(1); // Второй ответ
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ShowResponse(2); // Третий ответ
            }
        }
    }

    void StartDialogue()
    {
        isInDialogue = true;
        dialogueText.gameObject.SetActive(true);
        answersText.gameObject.SetActive(false);

        // Первая фраза NPC
        string initialText = "Кто ты? Здесь опасно...";
        StartCoroutine(TypeText(initialText));
        currentStep = 1;
    }

    void ShowAnswers()
    {
        // Если диалог уже завершён (например, игрок отошёл), не показываем ответы
        if (!isInDialogue) return;

        answersText.gameObject.SetActive(true);
        answers[0] = "Я заблудился. Помоги мне!";
        answers[1] = "Я ищу выход. Ты знаешь, где он?";
        answers[2] = "Мне всё равно, уйди с дороги!";

        responses[0] = "Хорошо, я помогу... Но будь осторожен.";
        responses[1] = "Выход? Я знаю путь, но он опасен.";
        responses[2] = "Как грубо... Уходи, я не буду помогать!";

        answersText.text = "1. " + answers[0] + "\n2. " + answers[1] + "\n3. " + answers[2];
        isWaitingForAnswer = true;
    }

    void ShowResponse(int choice)
    {
        // Если диалог уже завершён (например, игрок отошёл), не показываем ответ
        if (!isInDialogue) return;

        isWaitingForAnswer = false;
        answersText.gameObject.SetActive(false);
        StartCoroutine(TypeText(responses[choice]));
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = ""; // Очищаем текст

        // Включаем звук, если он не проигрывается
        if (talkSound != null && !talkSound.isPlaying)
        {
            talkSound.Play();
        }

        // Печатаем текст по буквам
        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        // Останавливаем звук, когда текст полностью отобразился
        if (talkSound != null)
        {
            talkSound.Stop();
        }

        isTyping = false;

        // Если это начальная фраза, показываем варианты ответа
        if (currentStep == 1)
        {
            Invoke("ShowAnswers", 0.5f); // Небольшая пауза перед показом ответов
            currentStep++;
        }
        // Если это ответ NPC, завершаем диалог
        else if (currentStep == 2)
        {
            Invoke("EndDialogue", 2f);
        }
    }

    void EndDialogue()
    {
        isInDialogue = false;
        isWaitingForAnswer = false;
        isTyping = false;
        dialogueText.gameObject.SetActive(false);
        answersText.gameObject.SetActive(false);
        currentStep = 0;
        StopAllCoroutines(); // Останавливаем печать текста, если она идёт

        // Останавливаем звук при завершении диалога
        if (talkSound != null && talkSound.isPlaying)
        {
            talkSound.Stop();
        }
    }
}