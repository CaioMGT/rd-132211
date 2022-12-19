using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {
    [SerializeField] private GameObject highlight;
        
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    private Mesh mesh;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    [SerializeField] private Material material;

    [SerializeField] private Transform cam;
    private float rangeHit = 5.0f;
    [SerializeField] private LayerMask groundMask;

    private enum HighlighSide {
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    private int verticesCount;
    
    private void Start() {
        meshFilter = (MeshFilter)highlight.AddComponent(typeof(MeshFilter));
        meshRenderer = (MeshRenderer)highlight.AddComponent(typeof(MeshRenderer));

        mesh = new Mesh();
        mesh.name = "Highlight";

        HighlighGen();
        MeshGen();
    }

    private void Update() {
        HighlightUpdates();
        ColorUpdate();
    }

    private void ColorUpdate() {
        Color colorA = material.color;
        colorA.a = 0.5f;

        Color colorB = material.color;
        colorB.a = 0.0f;

        float speed = 2;

        meshRenderer.material = material;
        meshRenderer.material.color = Color.Lerp(colorA, colorB, Mathf.PingPong(Time.time * speed, 1));
    }

    private void HighlightUpdates() {
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

    private void MeshGen() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        meshFilter.mesh = mesh;
    }

    /*
    private bool PointBlock(Vector3 pointPos) {
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

    private void HighlighGen() {
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

    private void TrianglesGen() {
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

    private void VerticesGen(HighlighSide side) {
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
