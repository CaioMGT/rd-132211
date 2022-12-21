using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    // Tamanho do Mundo em Blocos.
    public static Vector3 WorldSizeInBlocks = new Vector3(
        256,
        64,
        256
    );
    
    // Tamanho do Mundo em Chunks.
    public static Vector3 WorldSize = new Vector3(
        WorldSizeInBlocks.x / Chunk.ChunkSize.x,
        WorldSizeInBlocks.y / Chunk.ChunkSize.y,
        WorldSizeInBlocks.z / Chunk.ChunkSize.z
    );
    
    // Declare um Dictionary com chaves do tipo Vector3 e valores do tipo Chunk
    private Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();

    [SerializeField] private Transform player;

    public static int viewDistance = 5;

    // Declare o chunkPrefab como um GameObject
    [SerializeField] private GameObject chunkPrefab;

    private void Start() {
        // Gere o mundo chamando a função WorldGen
        WorldGen();
        
        //InitialWorldGen();
    }

    private void Update() {
        StartCoroutine(WorldRenderer());
    }

    // Declare a função WorldGen
    private void WorldGen() {
        // Percorra o Dictionary usando três loops for
        for(int x = -((int)WorldSize.x / 2); x < ((int)WorldSize.x / 2); x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -((int)WorldSize.z / 2); z < ((int)WorldSize.z / 2); z++) {
                    // Crie um Vector3 com o offset de chunk atual
                    Vector3 chunkOffset = new Vector3(
                        x * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        z * Chunk.ChunkSize.z
                    );

                    // Adicione uma nova instância de Chunk ao Dictionary usando chunkOffset como chave
                    chunks.Add(chunkOffset, new Chunk());
                }
            }
        }
    }

    /*
    private void InitialWorldGen() {
        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    // Crie um Vector3 com o offset de chunk atual
                    Vector3 chunkOffset = new Vector3(
                        x * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        z * Chunk.ChunkSize.z
                    );

                    //int r2 = viewDistance * viewDistance;

                    // Gere as chunks de forma circular
                    //if(new Vector3(x, y, z).sqrMagnitude < r2) {
                        Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                    //}
                }
            }
        }

        Vector3 spawn = new Vector3(
            0,
            WorldSizeInBlocks.y,
            0
        );
        
        player.position = spawn;
    }
    */

    private IEnumerator WorldRenderer() {
        int posX = Mathf.FloorToInt(player.position.x / Chunk.ChunkSize.x);
        int posZ = Mathf.FloorToInt(player.position.z / Chunk.ChunkSize.z);

        // Percorra o Dictionary usando três loops for
        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    // Crie um Vector3 com o offset de chunk atual
                    Vector3 chunkOffset = new Vector3(
                        (x + posX) * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        (z + posZ) * Chunk.ChunkSize.z
                    );

                    Chunk c = Chunk.GetChunk(new Vector3(
                        Mathf.FloorToInt(chunkOffset.x),
                        Mathf.FloorToInt(chunkOffset.y),
                        Mathf.FloorToInt(chunkOffset.z)
                    ));
                    
                    // Verifique se o chunkOffset está presente no Dictionary
                    if(chunks.ContainsKey(chunkOffset)) {
                        if(c == null) {
                            // Instancie o chunkPrefab usando as posições do Dictionary
                            GameObject chunk = Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                            chunk.name = "Chunk(" + (x + posX) + ", " + (z + posZ) + ")";
                        }
                    }

                    yield return null;
                }
            }
        }
    }
}
