using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI answersText;
    public GameObject player;
    public float interactionDistance = 2f;
    public float textSpeed = 0.05f;
    public AudioSource talkSound;
    public ScaleAnimationTriggerScript scaleTrigger; // Ссылка на скрипт с масштабом

    private bool isInRange = false;
    private bool isInDialogue = false;
    private bool isWaitingForAnswer = false;
    private bool isTyping = false;
    private string[] answers = new string[3];
    private string[] responses = new string[3];
    private int currentStep = 0;
    private string textToType;

    void Start()
    {
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
        if (scaleTrigger == null)
        {
            Debug.LogError("ScaleAnimationTriggerScript not assigned in Inspector!");
        }

        dialogueText.gameObject.SetActive(false);
        answersText.gameObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        isInRange = distance <= interactionDistance;

        if (isInRange && !isInDialogue && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }

        if (!isInRange && isInDialogue)
        {
            EndDialogue();
        }

        if (isWaitingForAnswer)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowResponse(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShowResponse(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ShowResponse(2);
            }
        }

        if (isTyping && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            dialogueText.text = textToType;
            isTyping = false;
            if (talkSound != null && talkSound.isPlaying)
            {
                talkSound.Stop();
            }
            if (currentStep == 1) ShowAnswers();
            else if (currentStep == 2) Invoke("EndDialogue", 2f);
        }
    }

    void StartDialogue()
    {
        isInDialogue = true;
        dialogueText.gameObject.SetActive(true);
        answersText.gameObject.SetActive(false);

        // Останавливаем анимацию масштаба и скрываем объект
        if (scaleTrigger != null)
        {
            scaleTrigger.StopScalingAndHide();
        }

        string initialText = "Кто ты? Здесь опасно...";
        StartCoroutine(TypeText(initialText));
        currentStep = 1;
    }

    void ShowAnswers()
    {
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
        if (!isInDialogue) return;

        isWaitingForAnswer = false;
        answersText.gameObject.SetActive(false);
        StartCoroutine(TypeText(responses[choice]));
    }

    IEnumerator TypeText(string text)
    {
        textToType = text;
        isTyping = true;
        dialogueText.text = "";

        if (talkSound != null && !talkSound.isPlaying)
        {
            talkSound.Play();
        }

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        if (talkSound != null)
        {
            talkSound.Stop();
        }

        isTyping = false;

        if (currentStep == 1)
        {
            Invoke("ShowAnswers", 0.5f);
            currentStep++;
        }
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
        StopAllCoroutines();

        if (talkSound != null && talkSound.isPlaying)
        {
            talkSound.Stop();
        }
    }
}