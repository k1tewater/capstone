using UnityEngine;
using UnityEngine.UIElements;

public class CameraUI : UIManager
{
    protected override void Awake()
    {
        buttonNames = new string[] { "Capture" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickCapture };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;
    }

    void ClickCapture(ClickEvent evt)
    {
        Debug.Log("Capture clicked");
        Camera camera = GameObject.Find("ObjectView").GetComponent<Camera>();
        var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
        RenderTexture renderTexture = new RenderTexture((int)camVisual.contentRect.width, (int)camVisual.contentRect.height, 16);
        camera.targetTexture = renderTexture;

        camVisual.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(camera.targetTexture));
    }
}
