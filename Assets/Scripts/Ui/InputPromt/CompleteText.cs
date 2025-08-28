using TMPro;
using UnityEngine.InputSystem;

public static class CompleteText
{
	public static string ReadAndReplaceBinding(string message, InputBinding bindingPath, TMP_SpriteAsset spriteAsset)
	{
		string buttonName = bindingPath.effectivePath;
		buttonName = RenameString(buttonName);
		
		
		message = message.Replace("BUTTONPROMPT",$"<sprite=\"{spriteAsset.name}\" name=\"{buttonName}\">");
		return message;
	}
	static string RenameString(string buttonName)
	{
		//buttonName = buttonName.Replace($"{actionName}:", "");
		buttonName = buttonName.Replace("<Keyboard>/", "keyboard_");
		buttonName = buttonName.Replace("<Gamepad>/", "gamepad_");
		return buttonName;
	}
}
