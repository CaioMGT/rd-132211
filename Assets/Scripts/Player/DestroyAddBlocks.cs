using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destruir e Colocar?
// Destruir e Adicionar?
public class DestroyAddBlocks : MonoBehaviour {
    [SerializeField] Transform cam;

    float rangeHit = 5.0f;

    [SerializeField] LayerMask groundMask;

    [SerializeField] GameObject Highlight;
    [SerializeField] Transform HighlightPos;
    
    void Start() {
        
    }

    void Update() {
        HitUpdates();
        HighlightUpdates();
    }

    void HitUpdates() {
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
            bool drestroyingBlock = Input.GetMouseButtonDown(1);
            bool addingBlock = Input.GetMouseButtonDown(0);
            
            if(
                addingBlock &&
                this.transform.position.y - 1 > Chunk.ChunkSize.y
            ) {
                return;
            }

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

            bool isValidPosition = 
                Vector3.Distance(
                    this.transform.position, pointPos
                ) > 0.8f &&
                Vector3.Distance(
                    cam.position, pointPos
                ) > 0.8f;

            if(addingBlock && isValidPosition) {
                Chunk c = Chunk.GetChunk(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                );

                c.SetBlock(pointPos, BlockType.stone);
            }
            
            /*
            Vector3 pointPos = hit.point - hit.normal / 2;

            if(Input.GetMouseButtonDown(1)) {                
                Chunk c = Chunk.GetChunk(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                );

                c.SetBlock(pointPos, BlockType.air);
            }
            if(Input.GetMouseButtonDown(0)) {
                pointPos = hit.point + hit.normal / 2;

                Chunk c = Chunk.GetChunk(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                );

                c.SetBlock(pointPos, BlockType.stone);
            }
            */

            /*
            Highlight.SetActive(true);

            pointPos = hit.point - hit.normal / 2;
            
            HighlightPos.position = new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            );
            */
        }
        /*
        else {
            Highlight.SetActive(false);            
            
            
            //HighlightPos.position = new Vector3(
            //    0,
            //    0,
            //    0
            //);      
        }
        */
    }

    void HighlightUpdates() {
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
            Highlight.SetActive(true);

            Vector3 pointPos = hit.point - hit.normal / 2;
            
            HighlightPos.position = new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            );
        }
        else {
            Highlight.SetActive(false);            
            
            /*
            HighlightPos.position = new Vector3(
                0,
                0,
                0
            );
            */            
        }
    }
}
