using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class TextToUI : UIManager
{
    VisualElement confirmVisualElement;
    Label inputConfirmLabel;
    TextField inputTextField;
    ProgressBar textToBar;
    VisualElement textToScreen;
    APICaller apiCaller;

    protected override void Awake()
    {
        buttonNames = new string[] { "Input", "Reupload", "Save", "Yes", "No" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickInput, ClickReupload, ClickSave, ClickYes, ClickNo };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        confirmVisualElement = document.rootVisualElement.Q<VisualElement>("ConfirmVisualElement");
        inputConfirmLabel = document.rootVisualElement.Q<Label>("InputConfirm");
        inputTextField = document.rootVisualElement.Q<TextField>("InputTextField");
        textToBar = document.rootVisualElement.Q<ProgressBar>("TextToBar");
        textToScreen = document.rootVisualElement.Q<VisualElement>("TextToScreen");
        apiCaller = new APICaller();

        textToBar.style.display = DisplayStyle.None;
        confirmVisualElement.style.display = DisplayStyle.None;

        GetButton("Reupload").style.display = DisplayStyle.None;
        GetButton("Save").style.display = DisplayStyle.None;
    }

    void ClickInput(ClickEvent evt)
    {
        inputConfirmLabel.text = $"Prompt : {inputTextField.text}\nDo you want create a model with this value?";
        confirmVisualElement.style.display = DisplayStyle.Flex;
    }

    void ClickReupload(ClickEvent evt)
    {

    }

     void ClickSave(ClickEvent evt)
    {
        objectManager.SaveObject(inputTextField.text);
    }


    void ClickYes(ClickEvent evt)
    {
        confirmVisualElement.style.display = DisplayStyle.None;
        textToBar.style.display = DisplayStyle.Flex;
        //API호출시작
        StartCoroutine(apiCaller.TextTo(inputTextField.text, textToBar));
        objectManager.SetVisualElementCamera(textToScreen);

        GetButton("Input").style.display = DisplayStyle.None;
        GetButton("Reupload").style.display = DisplayStyle.Flex;
        GetButton("Save").style.display = DisplayStyle.Flex;
    }

    void ClickNo(ClickEvent evt)
    {
        confirmVisualElement.style.display = DisplayStyle.None;
    }

}
