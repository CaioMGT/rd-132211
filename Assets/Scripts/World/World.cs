using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    [SerializeField] GameObject chunkPrefab;

    //public static int WorldWidth = ((256 / Chunk.ChunkSize.x) / 2);
    //public static int WorldHeight = 1;
    //public static int WorldDepth = 0;
    public static Vector3Int WorldSize = new Vector3Int(
        ((256 / Chunk.ChunkSize.x) / 2),
        1,
        ((256 / Chunk.ChunkSize.z) / 2)
    );

    [SerializeField] Transform player;

    int viewDistance = 2;

    void Start() {
        //StartCoroutine(WorldGen());
        //WorldGen();
    }

    void Update() {
        StartCoroutine(WorldGen());
    }

    //*
    IEnumerator WorldGen() {
        int posX = Mathf.FloorToInt(player.position.x / Chunk.ChunkSize.x);
        int posZ = Mathf.FloorToInt(player.position.z / Chunk.ChunkSize.z);

        for(int x = -viewDistance; x < viewDistance; x++) {
            //for(int y = -WorldDepth; y < WorldSize.y; y++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
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

                    /*
                    if(c == null) {
                        Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                    }
                    //*/

                    //*  
                    if(
                        (x + posX) >= -WorldSize.x || (x + posX) <= WorldSize.x ||
                        (z + posZ) >= -WorldSize.z || (z + posZ) <= WorldSize.z
                    ) {
                        if(c == null) {                        
                            Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                        }
                    }
                    else {
                        yield return null;
                    }
                    //*/

                    //yield return null;
                    
                    //Debug.Log("x, z = " + x + ", " + z);
                    //Debug.Log("x, z = " + posX + ", " + posZ);
                    Debug.Log("x, z = " + (x + posX) + ", " + (z + posZ));
                    Debug.Log("World Size = " + (WorldSize.x) + ", " + (WorldSize.z));
                    //Debug.Log("x, z = " + chunkOffset.x + ", " + chunkOffset.z);
                }
            }
        }
    }
    //*/

    /*
    IEnumerator WorldGen() {
        int posX = Mathf.FloorToInt(player.position.x / Chunk.ChunkSize.x);
        int posZ = Mathf.FloorToInt(player.position.z / Chunk.ChunkSize.z);

        for(int x = -viewDistance; x < viewDistance; x++) {
            //for(int y = -WorldDepth; y < WorldSize.y; y++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
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
                    
                    Debug.Log("x, z = " + x + ", " + z);
                    Debug.Log("posX, posZ = " + posX + ", " + posZ);
                }
            }
        }
    }
    //*/

    /*
    IEnumerator WorldGen() {
        for(int x = -WorldSize.x; x < WorldSize.x; x++) {
            //for(int y = -WorldDepth; y < WorldSize.y; y++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -WorldSize.z; z < WorldSize.z; z++) {
                    Vector3 chunkOffset = new Vector3(
                        x * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        z * Chunk.ChunkSize.z
                    );

                    Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);

                    yield return null;
                }
            }
        }
    }
    //*/

    /*
    void WorldGen() {
        for(int x = -WorldSize.x; x < WorldSize.x; x++) {
            //for(int y = -WorldDepth; y < WorldSize.y; y++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -WorldSize.z; z < WorldSize.z; z++) {
                    Vector3 chunkOffset = new Vector3(
                        x * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        z * Chunk.ChunkSize.z
                    );

                    Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                }
            }
        }
    }
    //*/
}
