using UnityEngine;

public class MeshGenerator
{
    /// <summary>
    /// �l�p�`�|���S����������
    /// </summary>
    /// <param name="height_map">���፷�������O��X�P�[���摜</param>
    /// <param name="resolution">�c���̒��_��</param>
    /// <param name="height_Limit">�����̍ő�l</param>
    /// <param name="unit_scale">���_�Ԃ̑傫��</param>
    /// <param name="texture_sprite">�c���̃e�N�X�`���̖���</param>
    /// <returns></returns>
    static public Mesh GenerateSquareMesh(Vector2Int resolution, float height_Limit, float unit_scale = 0.0f, int texture_sprite = 1, Texture2D height_map = null)
    {
        Mesh mesh = new Mesh();

        //���b�V���̒��_���W�z��
        Vector3[] vertices = new Vector3[resolution.x * resolution.y];
        //���b�V����UV���W�z��
        Vector2[] uv = new Vector2[resolution.x * resolution.y];
        //���_���W�̑g�ݍ��킹���̔z��
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
                //�O�p�`�𐶐�����悤�Ɋe���_���W�̗v�f�ԍ�����
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

        //�`��\�|���S�����̏���������グ��
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        return mesh;
    }

    /// <summary>
    /// ���b�V�����g�p����̂ɕK�v�ȃR���|�[�l���g�ݒ菈��
    /// </summary>
    /// <param name="mesh_object">Mesh��ێ�����I�u�W�F�N�g</param>
    /// <param name="set_material">Mesh�ɕt�^����}�e���A��</param>
    /// <param name="set_mesh">�t�^���郁�b�V��</param>
    static public void SetMeshComponent(ref GameObject mesh_object, Material set_material, Mesh set_mesh)
    {
        MeshFilter meshFilter = mesh_object.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = mesh_object.AddComponent<MeshRenderer>();
        meshFilter.mesh = set_mesh;
        meshRenderer.material = set_material;

        // MeshCollider�̓K�p
        MeshCollider meshCollider = mesh_object.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = set_mesh;
        meshCollider.cookingOptions = MeshColliderCookingOptions.None; // Fast Midphase�𖳌���
    }
}
