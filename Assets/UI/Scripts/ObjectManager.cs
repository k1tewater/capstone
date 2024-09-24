using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class ObjectManager 
{
    public static void SetVisualElementCamera(VisualElement screen)
    {
        Camera cam = GameObject.Find("ObjectView").GetComponent<Camera>();
        RenderTexture renderTexture = new RenderTexture((int)screen.contentRect.width, (int)screen.contentRect.height, 16);
        cam.targetTexture = renderTexture;
        screen.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(cam.targetTexture));
    }
}
