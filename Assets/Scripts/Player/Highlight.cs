using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {
    [SerializeField] GameObject highlight;
        
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    
    Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    [SerializeField] Material material;

    [SerializeField] Transform cam;
    float rangeHit = 5.0f;
    [SerializeField] LayerMask groundMask;

    enum HighlighSide {
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    int verticesCount;
    
    void Start() {
        meshFilter = (MeshFilter)highlight.AddComponent(typeof(MeshFilter));
        meshRenderer = (MeshRenderer)highlight.AddComponent(typeof(MeshRenderer));

        mesh = new Mesh();
        mesh.name = "Highlight";

        HighlighGen();
        MeshGen();
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

    void MeshGen() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        meshFilter.mesh = mesh;
    }

    /*
    bool PointBlock(Vector3 pointPos) {
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
            pointPos = hit.point - hit.normal / 2;

            return false;
        }
        else {
            return true;
        }                  
    }
    //*/

    void HighlighGen() {
        /*
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
            Vector3 pointPos = hit.point - hit.normal / 2;

            if(pointPos == new Vector3(1, 0, 0)) {
                VerticesGen(HighlighSide.EAST);
            }
            if(pointPos == new Vector3(-1, 0, 0)) {
                VerticesGen(HighlighSide.WEST);
            }
            if(pointPos == new Vector3(0, 1, 0)) {
                VerticesGen(HighlighSide.TOP);
            }
            if(pointPos == new Vector3(0, -1, 0)) {
                VerticesGen(HighlighSide.BOTTOM);
            }
            if(pointPos == new Vector3(0, 0, 1)) {
                VerticesGen(HighlighSide.NORTH);
            }
            if(pointPos == new Vector3(0, 0, -1)) {
                VerticesGen(HighlighSide.SOUTH);
            }
        }   
        */     
        
        /*
        if(PointBlock(new Vector3(1, 0, 0))) {
            VerticesGen(HighlighSide.EAST);
        }
        if(PointBlock(new Vector3(-1, 0, 0))) {
            VerticesGen(HighlighSide.WEST);
        }
        if(PointBlock(new Vector3(0, 1, 0))) {
            VerticesGen(HighlighSide.TOP);
        }
        if(PointBlock(new Vector3(0, -1, 0))) {
            VerticesGen(HighlighSide.BOTTOM);
        }
        if(PointBlock(new Vector3(0, 0, 1))) {
            VerticesGen(HighlighSide.NORTH);
        }
        if(PointBlock(new Vector3(0, 0, -1))) {
            VerticesGen(HighlighSide.SOUTH);
        }
        */

        //*
        VerticesGen(HighlighSide.EAST);
        VerticesGen(HighlighSide.WEST);
        VerticesGen(HighlighSide.TOP);
        VerticesGen(HighlighSide.BOTTOM);
        VerticesGen(HighlighSide.NORTH);
        VerticesGen(HighlighSide.SOUTH);
        //*/
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

    void VerticesGen(HighlighSide side) {
        switch(side) {
            case HighlighSide.EAST: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 0, 1));

                break;
            }
            case HighlighSide.WEST: {
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case HighlighSide.TOP: {
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 1, 0));

                break;
            }
            case HighlighSide.BOTTOM: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case HighlighSide.NORTH: {
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 0, 1));

                break;
            }
            case HighlighSide.SOUTH: {
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 0, 0));

                break;
            }
        }

        TrianglesGen();
    }
}
