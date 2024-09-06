using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
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

    public void LoadImageToUI(GameObject self, string visualElementName, string imagePath)
    {
        var root = self.GetComponent<UIDocument>().rootVisualElement; // ���ӿ�����Ʈ�� ����� ui��� ��������
        VisualElement targetElement = root.Q<VisualElement>(visualElementName); //���� ������ ui����߿��� visaulElementName�� �����ͼ� targetElement�� ����

        if (targetElement == null) 
        {
            Debug.LogWarning($"VisualElement with name {visualElementName} not found.");
            return;
        } 

       
        byte[] imageData = File.ReadAllBytes(imagePath); // 1.���Ͽ��� �̹����� �о����
        Texture2D tex = new Texture2D(0, 0); // 2. tex��� �̸��� 2Dtexture �̹��� �����͸� ����
        tex.LoadImage(imageData); // 1������ �о�� �̹����� tex�� �ε��ϱ�
        targetElement.style.backgroundImage = new StyleBackground(tex); //tagetElement�� ����̹����� tex�� �����Ű��
    }

    public void LoadImageFromGallery(GameObject self, string visualElementName) 
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            if (string.IsNullOrEmpty(file)) //���������� ���� �����ߴµ� �ν� �� �� ��� �������� ����
            {
                Debug.LogWarning("No image selected.");
                return;
            }

            FileInfo selected = new FileInfo(file);
            if (selected.Length > 50000000) // 50MB ����
            {
                Debug.LogWarning("Selected image is too large.");
                return;
            }

            // �̹����� ���õ� ���, �ٷ� �ε� �� UI�� ����
            LoadImageToUI(self, visualElementName, file);
        });
    }
}