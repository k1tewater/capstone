using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
public class UIManager : MonoBehaviour
{
    public UIDocument document;
    protected string[] buttonNames;
    protected EventCallback<ClickEvent>[] clickEvts;
    protected bool isRunningAPI = false;
    
    protected virtual void Awake() 
    {
        document = gameObject.GetComponent<UIDocument>();
        SetClickEvt(buttonNames, clickEvts);
    }
    
    private void SetClickEvt(string[] buttonNames, EventCallback<ClickEvent>[] clickEvts)
    {
        List<Button> buttons = new List<Button>();

        for (int i = 0; i < buttonNames.Length; i++)
        {
            Button button = document.rootVisualElement.Q<Button>(buttonNames[i]);
            buttons.Add(button);
            button.RegisterCallback<ClickEvent>(clickEvts[i]);
        }
    }
    
    protected void SwitchUI(string otherName)
    {
        GameObject otherObject = GameObject.Find(otherName);
        otherObject.GetComponent<UIManager>().document.rootVisualElement.style.display = DisplayStyle.Flex;
        GameObject.Find("UIs").GetComponent<Current>().ui.Push(otherObject);
    }
    
}