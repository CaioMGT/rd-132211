using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private Mesh mesh;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private enum BlockFace {        
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    private int vertexIndex;

    // Tamanho da Chunk em Blocos
    public static Vector3 ChunkSize = new Vector3(
        16, 
        64, 
        16
    );

    private BlockType[,,] voxelMap = new BlockType[
        (int)ChunkSize.x, 
        (int)ChunkSize.y, 
        (int)ChunkSize.z
    ];

    private BlockType blockType;

    public static List<Chunk> chunkList = new List<Chunk>();

    private void Start() {        
        chunkList.Add(this);

        ChunkGen();
    }

    private void Update() {
        
    }

    public void SetBlock(Vector3 worldPos, BlockType b) {
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        voxelMap[x, y, z] = b;

        ChunkRenderer();
    }

    public static Chunk GetChunk(Vector3 pos) {
        for(int i = 0; i < chunkList.Count; i++) {            
            Vector3 chunkPos = chunkList[i].transform.position;

            if(
                pos.x < chunkPos.x || pos.x >= chunkPos.x + ChunkSize.x || 
                pos.y < chunkPos.y || pos.y >= chunkPos.y + ChunkSize.y || 
                pos.z < chunkPos.z || pos.z >= chunkPos.z + ChunkSize.z
            ) {
                continue;
            }

            return chunkList[i];
        }

        return null;
    }
    
    // Adicione as camadas de blocos ao terreno
    private void ChunkLayersGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        // Somamos x, y, e z pela posição da Chunk para que não gere todas as Chunks iguais
        // Vamos ver isso mais claro quando adicionarmos Perlin Noise
        int _x = x + (int)transform.position.x;
        int _y = y + (int)transform.position.y;
        int _z = z + (int)transform.position.z;

        // Somamos x e z pelo tamanho do Mundo em Blocos para não haver um espelhamento ao gerar Chunks em x negativo e z negativo
        // Vamos ver isso mais claro quando adicionarmos Perlin Noise
        _x += (int)World.WorldSizeInBlocks.x;
        _z += (int)World.WorldSizeInBlocks.z;

        // Não podemos somar esses valores diretamente a x, y, e z pois o mapa de voxels necessidade de x, y, e z.
        
        // Gere a camada de Pedra
        if(_y < 32) {
            voxelMap[x, y, z] = BlockType.stone;
        }

        // Gere a camada de Grama
        else if(_y == 32) {
            voxelMap[x, y, z] = BlockType.grass_block;
        }
    }

    // Crie um mapa de voxels onde os blocos possam ser renderizados
    private void ChunkGen() {
        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    ChunkLayersGen(new Vector3(x, y, z));
                }
            }
        }

        ChunkRenderer();
    }

    // Renderize os voxels
    private void ChunkRenderer() {
        // Crie a malha
        mesh = new Mesh();
        mesh.name = "Chunk";

        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        vertexIndex = 0;

        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    if(voxelMap[x, y, z] != BlockType.air) {
                        BlockGen(new Vector3(x, y, z));
                    }
                }
            }
        }

        MeshGen();
    }

    private void MeshGen() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        // Adiciona a malha um colisor
        GetComponent<MeshCollider>().sharedMesh = mesh;

        // Adicione a malha ao MeshFilter do seu GameObject
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private bool HasAdjacentBlock(Vector3 adjacentBlock) {
        int x = (int)adjacentBlock.x;
        int y = (int)adjacentBlock.y;
        int z = (int)adjacentBlock.z;

        if(
            x < 0 || x > ChunkSize.x - 1 ||
            y < 0 || y > ChunkSize.y - 1 ||
            z < 0 || z > ChunkSize.z - 1
        ) {
            return false;
        }
        if(voxelMap[x, y, z] == BlockType.air) {
            return false;
        }
        else {
            return true;
        }        
    }

    private void BlockGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        // O tipo de bloco a ser renderizado é determinado pelo mapa de voxels
        blockType = voxelMap[x, y, z];

        // Gere a Face do Leste se não houver um bloco em +x
        if(!HasAdjacentBlock(new Vector3(1, 0, 0) + offset)) {
            VerticesAdd(BlockFace.EAST, offset);
        }

        // Gere a Face do Oeste se não houver um bloco -x
        if(!HasAdjacentBlock(new Vector3(-1, 0, 0) + offset)) {
            VerticesAdd(BlockFace.WEST, offset);
        }

        // Gere a Face do Topo se não houver um bloco +y
        if(!HasAdjacentBlock(new Vector3(0, 1, 0) + offset)) {
            VerticesAdd(BlockFace.TOP, offset);
        }

        // Gere a Face de Baixo se não houver um bloco -y
        if(!HasAdjacentBlock(new Vector3(0, -1, 0) + offset)) {
            VerticesAdd(BlockFace.BOTTOM, offset);
        }

        // Gere a Face do Norte se não houver um bloco +z
        if(!HasAdjacentBlock(new Vector3(0, 0, 1) + offset)) {
            VerticesAdd(BlockFace.NORTH, offset);
        }

        // Gere a Face do Sul se não houver um bloco -z
        if(!HasAdjacentBlock(new Vector3(0, 0, -1) + offset)) {
            VerticesAdd(BlockFace.SOUTH, offset);
        }
    }

    // Adicione os Vertices da Malha
    private void VerticesAdd(BlockFace face, Vector3 offset) {
        switch(face) {
            case BlockFace.EAST: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);

                break;
            }
            case BlockFace.WEST: {
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);

                break;
            }
            case BlockFace.TOP: {
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);

                break;
            }
            case BlockFace.BOTTOM: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);

                break;
            }
            case BlockFace.NORTH: {
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);

                break;
            }
            case BlockFace.SOUTH: {
                vertices.Add(new Vector3(0, 0, 0) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 0, 0) + offset);

                break;
            }
        }

        TrianglesAdd();

        UVsPos();
    }

    // Adicone os Triangulos dos Vertices para renderizar a face
    private void TrianglesAdd() {
        // Primeiro Tiangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(1 + vertexIndex);
        triangles.Add(2 + vertexIndex);

        // Segundo Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(2 + vertexIndex);
        triangles.Add(3 + vertexIndex);

        vertexIndex += 4;
    }

    // Adicione as UVs dos Vertices para renderizar a textura
    private void UVsAdd(Vector2 textureCoordinate) {
        Vector2 offset = new Vector2(
            0, 
            0
        );

        Vector2 textureSize = new Vector2(
            16 + offset.x,
            16 + offset.y
        );
        
        float x = textureCoordinate.x + offset.x;
        float y = textureCoordinate.y + offset.y;

        float _x = 1.0f / textureSize.x;
        float _y = 1.0f / textureSize.y;

        y = (textureSize.y - 1) - y;

        x *= _x;
        y *= _y;

        uv.Add(new Vector2(x, y));
        uv.Add(new Vector2(x, y + _y));
        uv.Add(new Vector2(x + _x, y + _y));
        uv.Add(new Vector2(x + _x, y));
    }

    // Pegue a posição da UV no Texture Atlas
    private void UVsPos() {
        // Pre-Classic | rd-132211
        
        // STONE
        if(blockType == BlockType.stone) {
            UVsAdd(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(blockType == BlockType.grass_block) {
            UVsAdd(new Vector2(0, 0));
        }
    }
}
