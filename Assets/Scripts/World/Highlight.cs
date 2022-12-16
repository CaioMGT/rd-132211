using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {
    Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    //List<Vector2> uv = new List<Vector2>();

    public enum BlockSide {
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    int verticesCount;

    //[SerializeField] BlockType blockType;    
    
    void Start() {
        mesh = new Mesh();

        BlockGen();

        MeshGen();
    }

    void Update() {
        //GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        
        /*
        float r = 1.0f;
        float g = 1.0f;
        float b = 1.0f;
        float a = 0.5f;

        GetComponent<MeshRenderer>().material.color = new Color(r, g, b, a);
        */

        /*
        Color color = Color.white;
        color.a = 0.5f;  

        GetComponent<MeshRenderer>().material.color = color;
        //*/

        /*
        Color color;

        ColorUtility.TryParseHtmlString("#0094FF", out color);
        color.a = 0.5f;

        GetComponent<MeshRenderer>().material.color = color;
        */

        Color colorA = Color.white;
        colorA.a = 0.5f;

        Color colorB = Color.white;
        colorB.a = 0.0f;

        float speed = 2;

        GetComponent<MeshRenderer>().material.color = Color.Lerp(colorA, colorB, Mathf.PingPong(Time.time * speed, 1));
    }

    void MeshGen() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        //mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        //GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void BlockGen() {
        VerticesGen(BlockSide.EAST);
        VerticesGen(BlockSide.WEST);
        VerticesGen(BlockSide.TOP);
        VerticesGen(BlockSide.BOTTOM);
        VerticesGen(BlockSide.NORTH);
        VerticesGen(BlockSide.SOUTH);
    }

    /*
    void UVsGen(Vector2 textureCoordinate) {
        float offsetX = 0;
        float offsetY = 0;
        
        float x = textureCoordinate.x + offsetX;
        float y = textureCoordinate.y + offsetY;

        float textureSizeX = 16 + offsetX;
        float textureSizeY = 16 + offsetY;

        float _x = 1.0f / textureSizeX;
        float _y = 1.0f / textureSizeY;

        float invertY = textureSizeY - 1;
        y = invertY - y;

        x *= _x;
        y *= _y;

        uv.Add(new Vector2(x, y));
        uv.Add(new Vector2(x, y + _y));
        uv.Add(new Vector2(x + _x, y + _y));
        uv.Add(new Vector2(x + _x, y));
    }
    */

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
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 0, 1));

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

        //UVsPositionsGen();
    }

    /*
    void UVsPositionsGen() {
        // Pre-Classic | rd-132211
        
        // STONE
        if(blockType == BlockType.stone) {
            UVsGen(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(blockType == BlockType.grass_block) {
            UVsGen(new Vector2(0, 0));
        }
    }
    */
}
