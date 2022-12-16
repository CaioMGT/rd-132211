using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAddBlocks : MonoBehaviour {
    [SerializeField] Transform cam;

    float rangeHit = 5.0f;

    [SerializeField] LayerMask groundMask;
    
    void Start() {
        
    }

    void Update() {
        HitUpdates();
    }

    void HitUpdates() {
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
            bool drestroyingBlock = Input.GetMouseButtonDown(1);
            bool addingBlock = Input.GetMouseButtonDown(0);
            
            Vector3 pointPos = (
                drestroyingBlock
                    ? hit.point - hit.normal / 2
                    : hit.point + hit.normal / 2
            );

            if(drestroyingBlock) {
                Chunk c = Chunk.GetChunk(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                );

                c.SetBlock(pointPos, BlockType.air);
            }

            if(
                addingBlock &&
                this.transform.position.y - 1 > Chunk.ChunkSize.y
            ) {
                return;
            }

            bool isValidPosition = 
                Vector3.Distance(
                    this.transform.position, pointPos
                ) > 1.0f &&
                Vector3.Distance(
                    cam.position, pointPos
                ) > 1.0f;

            if(addingBlock && isValidPosition) {
                Chunk c = Chunk.GetChunk(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                );

                c.SetBlock(pointPos, BlockType.stone);
            }
        }
    }
}
