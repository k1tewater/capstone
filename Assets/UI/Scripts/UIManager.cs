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
        var root = self.GetComponent<UIDocument>().rootVisualElement; // 게임오브젝트에 연결된 ui요소 가져오기
        VisualElement targetElement = root.Q<VisualElement>(visualElementName); //위에 가져온 ui요소중에서 visaulElementName을 가져와서 targetElement에 저장

        if (targetElement == null) 
        {
            Debug.LogWarning($"VisualElement with name {visualElementName} not found.");
            return;
        } 

       
        byte[] imageData = File.ReadAllBytes(imagePath); // 1.파일에서 이미지를 읽어오기
        Texture2D tex = new Texture2D(0, 0); // 2. tex라는 이름의 2Dtexture 이미지 데이터를 생성
        tex.LoadImage(imageData); // 1번에서 읽어온 이미지를 tex에 로드하기
        targetElement.style.backgroundImage = new StyleBackground(tex); //tagetElement에 배경이미지로 tex를 적용시키기
    }

    public void LoadImageFromGallery(GameObject self, string visualElementName) 
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            if (string.IsNullOrEmpty(file)) //갤러리에서 파일 선택했는데 인식 못 할 경우 오류구문 띄우기
            {
                Debug.LogWarning("No image selected.");
                return;
            }

            FileInfo selected = new FileInfo(file);
            if (selected.Length > 50000000) // 50MB 제한
            {
                Debug.LogWarning("Selected image is too large.");
                return;
            }

            // 이미지가 선택된 경우, 바로 로드 및 UI에 적용
            LoadImageToUI(self, visualElementName, file);
        });
    }
}