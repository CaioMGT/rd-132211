using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv = new List<Vector2>();

    public enum BlockSide {        
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    int verticesCount;

    //public static int ChunkWidth = 16;
    //public static int ChunkHeight = 64;
    public static Vector3Int ChunkSize = new Vector3Int(
        16, 
        64, 
        16
    );

    BlockType[,,] blockData = new BlockType[ChunkSize.x, ChunkSize.y, ChunkSize.z];

    BlockType blockType;

    public static List<Chunk> chunkData = new List<Chunk>();

    void Start() {
        chunkData.Add(this);

        ChunkGen();
    }

    void Update() {
        
    }

    public void SetBlock(Vector3 worldPos, BlockType b) {
        Vector3 localPos = worldPos - transform.position;

        blockData[
            Mathf.FloorToInt(localPos.x), 
            Mathf.FloorToInt(localPos.y), 
            Mathf.FloorToInt(localPos.z)
        ] = b;

        ChunkRenderer();
    }

    public static Chunk GetChunk(int x, int y, int z) {
        Vector3 pos = new Vector3(x, y, z);
        
        for(int i = 0; i < chunkData.Count; i++) {            
            Vector3 cpos = chunkData[i].transform.position;

            if(
                pos.x < cpos.x || pos.x >= cpos.x + ChunkSize.x || 
                pos.y < cpos.y || pos.y >= cpos.y + ChunkSize.y || 
                pos.z < cpos.z || pos.z >= cpos.z + ChunkSize.z
            ) {
                continue;
            }

            return chunkData[i];
        }

        return null;
    }
    
    void ChunkLayersGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        float _x = x + transform.position.x;
        float _y = y + transform.position.y;
        float _z = z + transform.position.z;

        _x += (World.WorldSize.x * ChunkSize.x);
        //_y += (World.WorldSize.y * ChunkSize.y);
        _z += (World.WorldSize.z * ChunkSize.z);

        if(_y < 32) {
            blockData[x, y, z] = BlockType.stone;
        }
        else if(_y == 32) {
            blockData[x, y, z] = BlockType.grass_block;
        }
    }

    void ChunkGen() {
        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    ChunkLayersGen(new Vector3(x, y, z));
                }
            }
        }

        ChunkRenderer();
    }

    void ChunkRenderer() {
        mesh = new Mesh();

        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        verticesCount = 0;

        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    if(blockData[x, y, z] != BlockType.air) {
                        BlockGen(new Vector3(x, y, z));
                    }
                }
            }
        }

        MeshGen();
    }

    void MeshGen() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().mesh = mesh;
    }

    bool HasSolidNeighbor(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        if(
            x < 0 || x >= ChunkSize.x ||
            y < 0 || y >= ChunkSize.y ||
            z < 0 || z >= ChunkSize.z
        ) {
            return false;
        }
        if(blockData[x, y, z] == BlockType.air) {
            return false;
        }

        return true;
    }

    void BlockGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        blockType = blockData[x, y, z];

        if(!HasSolidNeighbor(new Vector3(1, 0, 0) + offset)) {
            VerticesGen(BlockSide.EAST, offset);
        }
        if(!HasSolidNeighbor(new Vector3(-1, 0, 0) + offset)) {
            VerticesGen(BlockSide.WEST, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 1, 0) + offset)) {
            VerticesGen(BlockSide.TOP, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, -1, 0) + offset)) {
            VerticesGen(BlockSide.BOTTOM, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 0, 1) + offset)) {
            VerticesGen(BlockSide.NORTH, offset);
        }
        if(!HasSolidNeighbor(new Vector3(0, 0, -1) + offset)) {
            VerticesGen(BlockSide.SOUTH, offset);
        }
    }

    void UVsGen(Vector2 textureCoordinate) {
        float offsetX = 0;
        float offsetY = 0;

        float textureSizeX = 16 + offsetX;
        float textureSizeY = 16 + offsetY;
        
        float x = textureCoordinate.x + offsetX;
        float y = textureCoordinate.y + offsetY;

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

    void VerticesGen(BlockSide side, Vector3 offset) {
        switch(side) {
            case BlockSide.EAST: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);

                break;
            }
            case BlockSide.WEST: {
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);

                break;
            }
            case BlockSide.TOP: {
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);

                break;
            }
            case BlockSide.BOTTOM: {
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);

                break;
            }
            case BlockSide.NORTH: {
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);

                break;
            }
            case BlockSide.SOUTH: {
                vertices.Add(new Vector3(0, 0, 0) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 0, 0) + offset);

                break;
            }
        }

        TrianglesGen();

        UVsPositionsGen();
    }

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
}
