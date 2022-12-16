using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    [SerializeField] GameObject chunkPrefab;

    public static Vector3Int WorldSize = new Vector3Int(
        ((256 / Chunk.ChunkSize.x) / 2),
        1,
        ((256 / Chunk.ChunkSize.z) / 2)
    );

    int[,,] chunkData = new int[WorldSize.x, WorldSize.y, WorldSize.z];

    [SerializeField] Transform player;

    int viewDistance = 2;

    void Start() {
        
    }

    void Update() {
        StartCoroutine(WorldGen());
    }

    IEnumerator WorldGen() {
        int posX = Mathf.FloorToInt(player.position.x / Chunk.ChunkSize.x);
        int posZ = Mathf.FloorToInt(player.position.z / Chunk.ChunkSize.z);

        for(int x = 0; x < viewDistance; x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = 0; z < viewDistance; z++) {
                    if(chunkData[x, y, z] != 0) {
                        Vector3 chunkOffset = new Vector3(
                            (x + posX) * Chunk.ChunkSize.x,
                            y * Chunk.ChunkSize.y,
                            (z + posZ) * Chunk.ChunkSize.z
                        );

                        Chunk c = Chunk.GetChunk(
                            Mathf.FloorToInt(chunkOffset.x),
                            Mathf.FloorToInt(chunkOffset.y),
                            Mathf.FloorToInt(chunkOffset.z)
                        );

                        if(c == null) {
                            Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                        }

                        yield return null;
                    }
                }
            }
        }
    }
}
