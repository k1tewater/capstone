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
        objectViewPrefab = Resources.Load<GameObject>("ObjectView");
    }

    public Transform GetObjectTransform() {
        return objectViews.Last().Item2;
    }

    private void AddObjectView() {
        GameObject go = Instantiate(objectViewPrefab);
        go.name = "ObjectView";
        go.name += objectViews.Count;
        go.transform.position = new Vector3(objectViews.Count * 100, 0, 0);
        Transform trans = go.transform.GetChild(0);
        trans.gameObject.AddComponent<MoveObject>();
        objectViews.Add((go.GetComponent<Camera>(), trans));
    }

    public void SetVisualElementCamera(VisualElement screen) {
        AddObjectView();
        Camera cam = objectViews.Last().Item1;
        RenderTexture renderTexture = new RenderTexture((int)screen.contentRect.width, (int)screen.contentRect.height, 16);
        cam.targetTexture = renderTexture;
        screen.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(cam.targetTexture));
    }

    public void SaveObject(string fileName) {
        GameObject saveObject = objectViews.Last().Item2.GetChild(0).gameObject;

        // 다운로드 폴더 경로 설정
        string downloadsPath = "/storage/emulated/0/Download/";
        
        // .obj 및 .mtl 파일로 저장할 경로 설정
        string objFilePath = Path.Combine(downloadsPath, fileName + ".obj");
        string mtlFilePath = Path.Combine(downloadsPath, fileName + ".mtl");
        string textureFilePath = Path.Combine(downloadsPath, fileName + ".png");

        // MeshFilter 컴포넌트를 가져와서 .obj로 변환
        MeshFilter mf = saveObject.GetComponent<MeshFilter>();
        Renderer renderer = saveObject.GetComponent<Renderer>();

        if (mf != null && renderer != null) {
            string objData = MeshToObj(mf, renderer.material.name);
            File.WriteAllText(objFilePath, objData);

            string mtlData = GenerateMtlData(renderer.material);
            File.WriteAllText(mtlFilePath, mtlData);

            if (renderer.material.mainTexture != null) {
                Texture2D texture = renderer.material.mainTexture as Texture2D;
                byte[] textureData = texture.EncodeToPNG();
                File.WriteAllBytes(textureFilePath, textureData);
            }

            Debug.Log("Object exported to " + objFilePath);
            Debug.Log("Material exported to " + mtlFilePath);
            Debug.Log("Texture exported to " + textureFilePath);
        } else {
            Debug.LogError("No MeshFilter or Renderer found on the object.");
        
    }

    string MeshToObj(MeshFilter mf, string materialName) {
        Mesh m = mf.mesh;
        StringBuilder sb = new StringBuilder();

        sb.Append("o ").Append(mf.name).Append("\n");
        sb.Append("mtllib ExportedObject.mtl\n");
        sb.Append("usemtl ").Append(materialName).Append("\n");

        // Verts (정점)
        foreach (Vector3 v in m.vertices) {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");

        // Normals (법선 벡터)
        foreach (Vector3 n in m.normals) {
            sb.Append(string.Format("vn {0} {1} {2}\n", n.x, n.y, n.z));
        }
        sb.Append("\n");

        // UVs (텍스처 좌표)
        foreach (Vector2 uv in m.uv) {
            sb.Append(string.Format("vt {0} {1}\n", uv.x, uv.y));
        }
        sb.Append("\n");

        // Faces (면)
        for (int i = 0; i < m.subMeshCount; i++) {
            int[] triangles = m.GetTriangles(i);
            for (int j = 0; j < triangles.Length; j += 3) {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[j] + 1, triangles[j + 1] + 1, triangles[j + 2] + 1));
            }
        }
        return sb.ToString();
    }

    string GenerateMtlData(Material material) {
        StringBuilder sb = new StringBuilder();

        sb.Append("newmtl ").Append(material.name).Append("\n");
        sb.Append(string.Format("Kd {0} {1} {2}\n", material.color.r, material.color.g, material.color.b));

        if (material.mainTexture != null) {
            sb.Append("map_Kd ").Append(material.mainTexture.name).Append(".png\n");  // 텍스처 파일명이 필요합니다
        }

        return sb.ToString();
    }
}
}
