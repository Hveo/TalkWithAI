using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandListener : MonoBehaviour
{
    public List<VoiceCommand> VoiceCommands;

    private GameObject commandCaller;

    [System.Serializable]
    public struct VoiceCommand
    {
        public List<string> Keywords;
        public UnityEvent FunctionToRun;
    }

    public void ParseCommand(string Command, GameObject From)
    {
        int start = Command.IndexOf('[');
        int end = Command.IndexOf(']');

        string potentialCommand = Command.Substring(start + 1, end - start - 1).Trim();
        Debug.Log(potentialCommand);

        for (int i = 0; i < VoiceCommands.Count; ++i)
        {
            if (VoiceCommands[i].Keywords.Contains(potentialCommand))
            {
                commandCaller = From;
                VoiceCommands[i].FunctionToRun.Invoke();
                return;
            }
        }
    }

    public void FollowPlayer()
    {
        PlayerController playerController = commandCaller.GetComponent<PlayerController>();
        playerController.FollowPlayer();
    }

    public void StopFollow()
    {
        PlayerController playerController = commandCaller.GetComponent<PlayerController>();
        playerController.StopFollow();
    }
}
