using UnityEngine;

public class MeshGenerator
{
    /// <summary>
    /// 四角形ポリゴン生成処理
    /// </summary>
    /// <param name="height_map">高低差を示すグれスケール画像</param>
    /// <param name="resolution">縦横の頂点数</param>
    /// <param name="height_Limit">高さの最大値</param>
    /// <param name="unit_scale">頂点間の大きさ</param>
    /// <param name="texture_sprite">縦横のテクスチャの枚数</param>
    /// <returns></returns>
    static public Mesh GenerateSquareMesh(Vector2Int resolution, float height_Limit, float unit_scale = 0.0f, int texture_sprite = 1, Texture2D height_map = null)
    {
        Mesh mesh = new Mesh();

        //メッシュの頂点座標配列
        Vector3[] vertices = new Vector3[resolution.x * resolution.y];
        //メッシュのUV座標配列
        Vector2[] uv = new Vector2[resolution.x * resolution.y];
        //頂点座標の組み合わせ順の配列
        int[] triangles = new int[(resolution.x - 1) * (resolution.y - 1) * 6];

        for (int y = 0; y < resolution.y; y++)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                int index = y * resolution.x + x;
                float posX = (x - resolution.x / 2.0f) * unit_scale;
                float posY = (y - resolution.y / 2.0f) * unit_scale;

                float pixelHeight = height_Limit;

                if (height_map != null)
                {
                    pixelHeight = height_map.GetPixel(x, y).grayscale * height_Limit;
                }
                   
                vertices[index] = new Vector3(posX, pixelHeight, posY);
                Vector2 uvVector = new Vector2((float)(posX / resolution.x), (float)(posY / resolution.y));
                uv[index] = uvVector * texture_sprite / unit_scale;
            }
        }

        int triIndex = 0;
        for (int y = 0; y < resolution.y - 1; y++)
        {
            for (int x = 0; x < resolution.x - 1; x++)
            {
                //三角形を生成するように各頂点座標の要素番号を代入
                int v0 = y * resolution.x + x;
                int v1 = v0 + 1;
                int v2 = v0 + resolution.x;
                int v3 = v2 + 1;

                triangles[triIndex++] = v0;
                triangles[triIndex++] = v2;
                triangles[triIndex++] = v3;

                triangles[triIndex++] = v0;
                triangles[triIndex++] = v3;
                triangles[triIndex++] = v1;
            }
        }

        //描画可能ポリゴン数の上限を引き上げる
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        return mesh;
    }

    /// <summary>
    /// メッシュを使用するのに必要なコンポーネント設定処理
    /// </summary>
    /// <param name="mesh_object">Meshを保持するオブジェクト</param>
    /// <param name="set_material">Meshに付与するマテリアル</param>
    /// <param name="set_mesh">付与するメッシュ</param>
    static public void SetMeshComponent(ref GameObject mesh_object, Material set_material, Mesh set_mesh)
    {
        MeshFilter meshFilter = mesh_object.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = mesh_object.AddComponent<MeshRenderer>();
        meshFilter.mesh = set_mesh;
        meshRenderer.material = set_material;

        // MeshColliderの適用
        MeshCollider meshCollider = mesh_object.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = set_mesh;
        meshCollider.cookingOptions = MeshColliderCookingOptions.None; // Fast Midphaseを無効化
    }
}
