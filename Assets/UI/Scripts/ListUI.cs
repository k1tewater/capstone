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
        // ������ ��� GameObject�� ã���ϴ�.(������ ��� ������Ʈ�� ã�°ɷ� �س��µ� ����κ� �����ؼ� ������ ������Ʈ �ҷ����� �κ����� �����ϸ� �ɵ�)
        GameObject[] objects = FindObjectsOfType<GameObject>();

        // UI ����
        UpdateUI(new List<GameObject>(objects));
    }

    // ListView�� �� �������� ǥ���� ���ø� ����
    public void UpdateUI(List<GameObject> objects)
    {
        // ListView�� itemsSource ����
        List.itemsSource = objects;

        // �� �������� ���� ���� ����
        List.fixedItemHeight = 1000; // �� ���̴� ��ü �������� �����Դϴ�.

        // �� VisualElement ����( ���⼭ �θ� ������Ʈ �ڽ� ������Ʈ ����� ����Ʈ�信 ������ �־ �̷������� ���ϸ� ���Ʒ� ������ �������� ����������)
        List.makeItem = () =>
        {
            // �θ� VisualElement ����
            var parentElement = new VisualElement();

            // �ڽ� VisualElement ����
            var childElement = new VisualElement();

            // �θ� �ڽ� �߰�
            parentElement.Add(childElement);

            // ũ�� ����
            childElement.style.height = 700; // �ڽ��� ���� ����
            childElement.style.width = Length.Percent(80); // �¿� ���� �θ��� 80%��
            childElement.style.marginTop = Length.Percent(10); // ���� 10�� ���ذ�
            childElement.style.marginBottom = Length.Percent(10);// �Ʒ��� 10�� ���ذ�
            childElement.style.marginLeft = Length.Percent(10); // ���� 10�� ���ذ�
            childElement.style.marginRight = Length.Percent(10); // ���� 10�� ���ذ�

            // �׵θ� ���� (���� ������ �׵θ�)
            childElement.style.borderTopWidth = 10;
            childElement.style.borderBottomWidth = 10;
            childElement.style.borderLeftWidth = 10;
            childElement.style.borderRightWidth = 10;
            childElement.style.borderTopColor = Color.black;
            childElement.style.borderBottomColor = Color.black;
            childElement.style.borderLeftColor = Color.black;
            childElement.style.borderRightColor = Color.black;

            // ��� ���� ����
            childElement.style.backgroundColor = new Color(111f / 255f, 168f / 255f, 243f / 255f);

            // ���̾ƿ� ����
            childElement.style.flexDirection = FlexDirection.Column;

            return parentElement; // �θ� ��Ҹ� ��ȯ
        };

        // �� VisualElement�� �����͸� ���ε�(���⿡�� ������ ������Ʈ���� �ҷ��ͼ� childElement.add(���ñ�) �ؼ� ������ ��)
        List.bindItem = (element, i) =>
        {
            var parentElement = element as VisualElement; 
            var childElement = parentElement.ElementAt(0) as VisualElement; // ù ��° �ڽ� ��� ��������

            childElement.Clear(); // ����Ʈ�� ������ ������Ʈ �ɶ� ���������� �������� ���������ǵ� �ʿ���ų� �����ʿ��ϸ� �����ɵ�

            // (������Ʈ �̸��� �󺧷� �߰�(�̰Ŵ� �������ְ� ���ϴ��� �����ٷ��� �ϴ� �־�����Ŷ� ������Ʈ ã�Ƽ� ������ �� ������ �ٽ� ���� ���ѵ� ��)
            var label = new Label(objects[i].name);
            childElement.Add(label); // ���� �ڽ� ��ҿ� �߰�

            // �ʿ��ϸ� �� �� �߰��ϸ� �ɵ�?
        };

        // ����Ʈ�並 �ٽ� �׸����� ����
        List.RefreshItems();
    }

}



