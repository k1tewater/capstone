using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        trans.gameObject.AddComponent<MoveObject>();
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

    public void SaveObject()
    {
        GameObject saveObject = objectViews.Last().Item2.GetChild(0).gameObject;
        // 다운로드 폴더 경로 설정
        string downloadsPath = Application.persistentDataPath;
        
        // .obj 파일로 저장할 경로 설정
        string filePath = Path.Combine(downloadsPath, "ExportedObject.obj");
        
        // MeshFilter 컴포넌트를 가져와서 .obj로 변환
        MeshFilter mf = saveObject.GetComponent<MeshFilter>();
        if (mf != null)
        {
            string objData = MeshToObj(mf);
            File.WriteAllText(filePath, objData);
            Debug.Log("Object exported to " + filePath);
        }
        else
        {
            Debug.LogError("No MeshFilter found on the object.");
        }
    }

    string MeshToObj(MeshFilter mf)
    {
        Mesh m = mf.mesh;
        StringBuilder sb = new StringBuilder();

        sb.Append("o ").Append(mf.name).Append("\n");

        // Verts (정점)
        foreach (Vector3 v in m.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");

        // Normals (법선 벡터)
        foreach (Vector3 n in m.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", n.x, n.y, n.z));
        }
        sb.Append("\n");

        // UVs (텍스처 좌표)
        foreach (Vector2 uv in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", uv.x, uv.y));
        }
        sb.Append("\n");

        // Faces (면)
        for (int i = 0; i < m.subMeshCount; i++)
        {
            int[] triangles = m.GetTriangles(i);
            for (int j = 0; j < triangles.Length; j += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[j] + 1, triangles[j + 1] + 1, triangles[j + 2] + 1));
            }
        }
        return sb.ToString();
    }
}
