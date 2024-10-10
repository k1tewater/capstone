using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListUI : UIManager
{
    
    ListView List;
    // Start is called before the first frame update
    protected override void Awake()
    {
        buttonNames = new string[] { };
        clickEvts = new EventCallback<ClickEvent>[] { };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        List = document.rootVisualElement.Q<ListView>("List");
    }
    void Start()
    {
        // 씬에서 모든 GameObject를 찾습니다.(지금은 모든 오브젝트를 찾는걸로 해놨는데 여기부분 수정해서 저장한 오브젝트 불러오는 부분으로 수정하면 될듯)
        GameObject[] objects = FindObjectsOfType<GameObject>();

        // UI 갱신
        UpdateUI(new List<GameObject>(objects));
    }

    // ListView의 각 아이템을 표시할 템플릿 설정
    public void UpdateUI(List<GameObject> objects)
    {
        // ListView의 itemsSource 설정
        List.itemsSource = objects;

        // 각 아이템의 고정 높이 설정
        List.fixedItemHeight = 1000; // 이 높이는 전체 아이템의 높이입니다.

        // 각 VisualElement 생성( 여기서 부모 엘리먼트 자식 엘리먼트 만든건 리스트뷰에 ㅈ버그 있어서 이런식으로 안하면 위아래 간격을 못벌려서 만들어놓은거)
        List.makeItem = () =>
        {
            // 부모 VisualElement 생성
            var parentElement = new VisualElement();

            // 자식 VisualElement 생성
            var childElement = new VisualElement();

            // 부모에 자식 추가
            parentElement.Add(childElement);

            // 크기 설정
            childElement.style.height = 700; // 자식의 높이 설정
            childElement.style.width = Length.Percent(80); // 좌우 폭을 부모의 80%로
            childElement.style.marginTop = Length.Percent(10); // 위에 10퍼 폭준거
            childElement.style.marginBottom = Length.Percent(10);// 아래에 10퍼 폭준거
            childElement.style.marginLeft = Length.Percent(10); // 옆에 10퍼 폭준거
            childElement.style.marginRight = Length.Percent(10); // 옆에 10퍼 폭준거

            // 테두리 설정 (얇은 검정색 테두리)
            childElement.style.borderTopWidth = 10;
            childElement.style.borderBottomWidth = 10;
            childElement.style.borderLeftWidth = 10;
            childElement.style.borderRightWidth = 10;
            childElement.style.borderTopColor = Color.black;
            childElement.style.borderBottomColor = Color.black;
            childElement.style.borderLeftColor = Color.black;
            childElement.style.borderRightColor = Color.black;

            // 배경 색상 설정
            childElement.style.backgroundColor = new Color(111f / 255f, 168f / 255f, 243f / 255f);

            // 레이아웃 설정
            childElement.style.flexDirection = FlexDirection.Column;

            return parentElement; // 부모 요소를 반환
        };

        // 각 VisualElement에 데이터를 바인딩(여기에서 저장한 오브젝트들을 불러와서 childElement.add(뭐시기) 해서 넣으면 됨)
        List.bindItem = (element, i) =>
        {
            var parentElement = element as VisualElement; 
            var childElement = parentElement.ElementAt(0) as VisualElement; // 첫 번째 자식 요소 가져오기

            childElement.Clear(); // 리스트의 내용이 업데이트 될때 이전내용이 지워지게 만들어놓은건데 필요없거나 수정필요하면 지우면될듯

            // (오브젝트 이름을 라벨로 추가(이거는 직관성있게 뭐하는지 보여줄려고 일단 넣어놓은거라 오브젝트 찾아서 가져올 수 있으면 다시 삭제 시켜도 됨)
            var label = new Label(objects[i].name);
            childElement.Add(label); // 라벨을 자식 요소에 추가

            // 필요하면 뭐 더 추가하면 될듯?
        };

        // 리스트뷰를 다시 그리도록 강제
        List.RefreshItems();
    }

}



