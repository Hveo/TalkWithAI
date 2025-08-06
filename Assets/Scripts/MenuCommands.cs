using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using Yetibyte.Unity.SpeechRecognition;

public class MenuCommands : MonoBehaviour
{
    [System.Serializable]
    public struct MenuCommand
    {
        public int LayerIndex;
        public List<string> Keywords;
        public UnityEvent FunctionToRun;
    }

    public List<MenuCommand> voiceCommands;

    [SerializeField]
    private GameObject MenuContainer;

    [SerializeField]
    private GameObject OptionMessage;

    private int currentLayerIndex = 0;

    private void Start()
    {
        GetComponent<VoskListener>().OnUICommandAsked += OnMenuCommandReceived;
    }

    public void OnMenuCommandReceived(object sender, VoskResultEventArgs args)
    {
        string parsedCommand = args.Result.Text.Trim().ToLower();

        for (int i = 0; i < voiceCommands.Count; ++i)
        {
            if (voiceCommands[i].Keywords.Contains(parsedCommand) && currentLayerIndex == voiceCommands[i].LayerIndex)
            {
                voiceCommands[i].FunctionToRun.Invoke();
                return;
            }
        }
    }

    private void OnDestroy()
    {
        GetComponent<VoskListener>().OnUICommandAsked -= OnMenuCommandReceived;
    }

    #region MenuCommands
    public void OpenMenu()
    {
        currentLayerIndex = 1;
        MenuContainer.SetActive(true);
    }

    public void OptionMenu()
    {
        OptionMessage.SetActive(true);   
    }

    public void CloseMenu()
    {
        MenuContainer.SetActive(false);
        OptionMessage.SetActive(false);
        currentLayerIndex = 0;
    }
    #endregion
}
