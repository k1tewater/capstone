using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectManager : MonoBehaviour
{
    List<(Camera, Transform)> objectViews;
    GameObject objectViewPrefab;
    private void Awake() {
        objectViews = new List<(Camera, Transform)>();
        objectViewPrefab  = Resources.Load<GameObject>("ObjectView");
    }

    public Transform GetObjectTransform()
    {
        return objectViews.Last().Item2;
    }
    private void AddObjectView()
    {
        GameObject go = Instantiate(objectViewPrefab);
        go.name = "ObjectView";
        go.name += objectViews.Count;
        go.transform.position = new Vector3(objectViews.Count * 100, 0, 0);
        Transform trans = go.transform.GetChild(0);
        objectViews.Add((go.GetComponent<Camera>(), trans));
    }

    public void SetVisualElementCamera(VisualElement screen)
    {
        AddObjectView();
        Camera cam = objectViews.Last().Item1;
        RenderTexture renderTexture = new RenderTexture((int)screen.contentRect.width, (int)screen.contentRect.height, 16);
        cam.targetTexture = renderTexture;
        screen.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(cam.targetTexture));
    }

    //- 모델 생성시 Camera, Object 새로 배치하는 스크립트
}
