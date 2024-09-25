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
        buttonNames = new string[] { "Input", "Yes", "No" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickInput, ClickYes, ClickNo };
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
    }

    void ClickInput(ClickEvent evt)
    {
        inputConfirmLabel.text = $"Prompt : {inputTextField.text}\nDo you want create a model with this value?";
        confirmVisualElement.style.display = DisplayStyle.Flex;
    }
    void ClickYes(ClickEvent evt)
    {
        confirmVisualElement.style.display = DisplayStyle.None;
        textToBar.style.display = DisplayStyle.Flex;
        //API호출시작
        StartCoroutine(apiCaller.TextTo(inputTextField.text, textToBar));
        ObjectManager.SetVisualElementCamera(textToScreen);
    }

    void ClickNo(ClickEvent evt)
    {
        confirmVisualElement.style.display = DisplayStyle.None;
    }

    void CameraSetting()
    {
        Camera camera = GameObject.Find("ObjectView").GetComponent<Camera>();
        var camVisual = document.rootVisualElement.Q<VisualElement>("TextToScreen");
        RenderTexture renderTexture = new RenderTexture((int)camVisual.contentRect.width, (int)camVisual.contentRect.height, 16);
        camera.targetTexture = renderTexture;
        camVisual.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(camera.targetTexture));
    }

}
