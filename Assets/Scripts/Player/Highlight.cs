using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {
    GameObject highlight;    
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    
    Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    [SerializeField] Material material;

    [SerializeField] Transform cam;
    float rangeHit = 5.0f;
    [SerializeField] LayerMask groundMask;

    enum BlockSide {
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    int verticesCount;
    
    void Start() {
        CreateGameObject();
        CreateMesh();
    }

    void Update() {
        HighlightUpdates();
        ColorUpdate();
    }

    void ColorUpdate() {
        Color colorA = material.color;
        colorA.a = 0.5f;

        Color colorB = material.color;
        colorB.a = 0.0f;

        float speed = 2;

        meshRenderer.material = material;
        meshRenderer.material.color = Color.Lerp(colorA, colorB, Mathf.PingPong(Time.time * speed, 1));
    }

    void HighlightUpdates() {
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
            highlight.SetActive(true);

            Vector3 pointPos = hit.point - hit.normal / 2;
            
            highlight.transform.position = new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            );
        }
        else {
            highlight.SetActive(false);          
        }
    }

    void CreateMesh() {
        mesh = new Mesh();
        mesh.name = "Highlight";

        BlockGen();
        MeshGen();
    }

    void MeshGen() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        meshFilter.mesh = mesh;
    }

    void BlockGen() {
        // Se eu estiver olhando a face Leste
            VerticesGen(BlockSide.EAST);

        // Se eu estiver olhando a face Oeste
            VerticesGen(BlockSide.WEST);

        // Se eu estiver olhando a face de Cima
            VerticesGen(BlockSide.TOP);

        // Se eu estiver olhando a face de Baixo
            VerticesGen(BlockSide.BOTTOM);

        // Se eu estivre olhando a face Norte
            VerticesGen(BlockSide.NORTH);

        // Se eu estiver olhando a face Sul
            VerticesGen(BlockSide.SOUTH);
    }

    void TrianglesGen() {
        // Primeiro Tiangulo
        triangles.Add(0 + verticesCount);
        triangles.Add(1 + verticesCount);
        triangles.Add(2 + verticesCount);

        // Segundo Triangulo
        triangles.Add(0 + verticesCount);
        triangles.Add(2 + verticesCount);
        triangles.Add(3 + verticesCount);

        verticesCount += 4;
    }

    void VerticesGen(BlockSide side) {
        switch(side) {
            case BlockSide.EAST: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 0, 1));

                break;
            }
            case BlockSide.WEST: {
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case BlockSide.TOP: {
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 1, 0));

                break;
            }
            case BlockSide.BOTTOM: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case BlockSide.NORTH: {
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 0, 1));

                break;
            }
            case BlockSide.SOUTH: {
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 0, 0));

                break;
            }
        }

        TrianglesGen();
    }

    void CreateGameObject() {
        highlight = new GameObject("Highlight");

        meshFilter = (MeshFilter)highlight.AddComponent(typeof(MeshFilter));
        meshRenderer = (MeshRenderer)highlight.AddComponent(typeof(MeshRenderer));
    }
}
