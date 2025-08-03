using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LLMUnity;
using Yetibyte.Unity.SpeechRecognition;

public class TextUI : MonoBehaviour
{
    public Image textContainer;
    public TextMeshProUGUI textMesh;
    public ScrollRect textScroller;
    public LLMCharacter character; //might need a dispatcher if we want several characters
    public float BGAlpha = 0.25f;
    public float TimeBeforeHiding = 10.0f;

    private float idleTime;
    private Vector2 initialContentSize;
    private string AIAnswer = string.Empty;
    private string currentChat = string.Empty;
    private bool isCommand = false;
    private CommandListener commandListener;

    private void Start()
    {
        textMesh.text = string.Empty;
        initialContentSize = textScroller.content.sizeDelta;
        commandListener = GetComponent<CommandListener>();
        GetComponent<VoskListener>().DisplayMessageOnChat += OnNewMessageRecognized;
    }

    private void OnDestroy()
    {
        GetComponent<VoskListener>().DisplayMessageOnChat -= OnNewMessageRecognized;
    }

    IEnumerator ActivateBackground()
    {
        textScroller.verticalScrollbar.value = 1.0f;
        textContainer.gameObject.SetActive(true);
        Color color = textContainer.color;

        for (float i = 0.0f; i < BGAlpha; i += 0.1f)
        {
            color.a = i;
            textContainer.color = color;
            yield return new WaitForEndOfFrame();
        }
    }

    void OnNewMessageRecognized(object sender, VoskResultEventArgs args)
    {
        StartCoroutine(AddNewMessage("You", args.Result.Text));
        currentChat = textMesh.text;
        _ = character.Chat(args.Result.Text, SetAIText, OnAIAnswerComplete, true);
        isCommand = false;
    }

    void SetAIText(string text)
    {
        AIAnswer = text;

        if (!AIAnswer.Contains("["))
        {
            StartCoroutine(AddNewMessage(character.AIName, AIAnswer, true));
        }
        else
        {
            isCommand = true;
        }
    }

    void OnAIAnswerComplete()
    {
        if (isCommand)
        {
            commandListener.ParseCommand(AIAnswer.ToLower(), character.gameObject);
        }
        else
        {
            textMesh.text += "\n";
        }
    }

    public IEnumerator AddNewMessage(string speaker, string message, bool isAIAnswer = false)
    {
        idleTime = 0.0f;

        if (!textContainer.gameObject.activeSelf)
        {
            yield return ActivateBackground();
        }

        if (isAIAnswer)
        {
            textMesh.text = currentChat + speaker + ": " + message;
        }
        else
        {
            textMesh.text += speaker + ": " + message + "\n";
        }

        ScrollIfNeeded();
    }

    private void Update()
    {
        if (textContainer.IsActive())
        {
            idleTime += Time.deltaTime;

            if (idleTime >= TimeBeforeHiding)
            {
                textContainer.gameObject.SetActive(false);
                textMesh.text = string.Empty;
                textScroller.content.sizeDelta = initialContentSize;
            }
        }
    }

    void ScrollIfNeeded()
    {
        int offsetY = 15;

        float newHeight = textMesh.preferredHeight;
        RectTransform content = textScroller.content;

        if (newHeight > content.rect.height - offsetY)
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, newHeight + offsetY);
            textScroller.verticalScrollbar.value = 0.0f;
        }
    }
}
