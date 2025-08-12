This is my integration of VOSK (taken from Yetibyte) and mixed with LLM for Unity plugin
from the Asset Store.

To test it, you might need to re-download LLM for Unity package since the streaming assets
are two heavy for GitHub (not sure it's necessary though).
You should also download the AI model you want to use by using the LLM Script on the LLM object on the scene, I strongly suggest Qwen3 4B.

French Speaker: Open the "Level Scene". 

English Speaker: Open the "Level Scene En".

It should work as it is so just play and WAIT for debug LLM service Created.

If you wanna try stuff, feel free to edit commands here

1- Sci-Fi_Soldier has an AI prompt, you can change it but keep in mind that you might lose the command parsing.
2- You can also add commands and link functions. Location: CommandListener script on the VoiceRecognitionHandler object.

To speak I'm using forward and back button of the mouse. If you don't have them you can edit the playerinput file located here:
Assets\Infima Games\Low Poly Shooter Pack - Free Sample\Input
The two last actions are for the menu commands and to talk with the AI, just pick another key but set it as a hold interaction.



