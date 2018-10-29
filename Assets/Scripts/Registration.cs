using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registration : MonoBehaviour {

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Matrix4x4 transferMatrix;

    private Mesh pc_mesh;
    private Vector3[] pc_vertices;
    private int[] pc_triangles;

    [ContextMenu("Register")]
    // Update is called once per frame
    public void Register () {

        float THRES = 1e-16;
        int NUM_ITERATION = 50;

        float dis = 0.f;
        int num_iter = 0;

        // 获取数字模型的顶点、面片坐标
        GetModelVertex();

        while (dis > THRES && num_iter < NUM_ITERATION) {
            // 1. 获取实体模型的点云坐标
            GetPointCloud();

            // 2. 调用ICP方法计算模型的变换矩阵
            // update transferMatrix

            // 3. 对数字模型进行变换
            int i = 0;
            while (i < vertices.Length)
            {
                vertices[i] = transferMatrix.MultiplyPoint(vertices[i]);
                i++;
            }
            mesh.vertices = vertices;

            // 4. 重新计算距离
            // update dis
            num_iter++;
        }
	}

    private void GetModelVertex() {
        // 获取数字模型的顶点与面片信息
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        foreach (Vector3 vertex in vertices)
        {
            Debug.Log(vertex);
        }
    }

    private void GetPointCloud() {
        // Assuming you've scanned your space using the SpatialMappingManager, 
        // you can get all the mesh filter objects like so:
        List< MeshFilter > meshFilters = SpatialMappingManager.Instance.GetMeshFilters();

        // TODO:
        // 这里需要去除掉不必要的mesh部分
        foreach (MeshFilter meshFilter in meshFilters)
        {
            // 获取点云模型的顶点与面片信息
            pc_mesh = meshFilter.sharedMesh; // meshFilter.mesh;
            pc_vertices = pc_mesh.vertices;
            pc_triangles = pc_mesh.triangles;
            foreach (Vector3 pc_vertex in pc_vertices) {
                Debug.Log(pc_vertex);
            }
        }

    }
}
