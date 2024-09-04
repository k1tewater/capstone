using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager
{
    public void SetClickEvt(GameObject self, string[] buttonNames, EventCallback<ClickEvent>[] clickEvts)
    {
        UIDocument document = self.GetComponent<UIDocument>();
        List<Button> buttons = new List<Button>();

        for (int i = 0; i < buttonNames.Length; i++)
        {
            Button button = document.rootVisualElement.Q<Button>(buttonNames[i]);
            buttons.Add(button);
            button.RegisterCallback<ClickEvent>(clickEvts[i]);
        }
    }

    public void SwitchUI(GameObject self, string otherObjectName)
    {
        self.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        GameObject.Find(otherObjectName).GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
    }
}
