using UnityEngine;
using UnityEngine.UIElements;

public class TextToUI : UIManager
{
    VisualElement confirmVisualElement;
    Label inputConfirmLabel;
    TextField inputTextField;
    ProgressBar apiProgressBar;

    protected override void Awake()
    {
        buttonNames = new string[] { "Input", "Yes", "No" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickInput, ClickYes, ClickNo };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        confirmVisualElement = document.rootVisualElement.Q<VisualElement>("ConfirmVisualElement");
        inputConfirmLabel = document.rootVisualElement.Q<Label>("InputConfirm");
        inputTextField = document.rootVisualElement.Q<TextField>("InputTextField");
        apiProgressBar = document.rootVisualElement.Q<ProgressBar>("ApiProgressBar");
        apiProgressBar.lowValue = 0f;
        apiProgressBar.highValue = 100f;
        apiProgressBar.style.display = DisplayStyle.None;

        confirmVisualElement.style.display = DisplayStyle.None;
    }

    void ClickInput(ClickEvent evt)
    {
        inputConfirmLabel.text = $"입력한 값 : {inputTextField.text}\n모델을 만드시겠습니까?";
        confirmVisualElement.style.display = DisplayStyle.Flex;
    }
    void ClickYes(ClickEvent evt)
    {
        confirmVisualElement.style.display = DisplayStyle.None;
        apiProgressBar.style.display = DisplayStyle.Flex;
        //API호출시작
        APICaller apiCaller = new APICaller();
        StartCoroutine(apiCaller.TextTo(inputTextField.text));
        CameraSetting();
    }

    void ClickNo(ClickEvent evt)
    {
        confirmVisualElement.style.display = DisplayStyle.None;
    }

    void CameraSetting()
    {
        Camera camera = GameObject.Find("ObjectView").GetComponent<Camera>();
        var camVisual = document.rootVisualElement.Q<VisualElement>("ObjectViewVisualElement");
        RenderTexture renderTexture = new RenderTexture((int)camVisual.contentRect.width, (int)camVisual.contentRect.height, 16);
        camera.targetTexture = renderTexture;
        camVisual.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(camera.targetTexture));
    }
        
}
