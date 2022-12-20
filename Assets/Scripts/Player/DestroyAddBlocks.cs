using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAddBlocks : MonoBehaviour {
    [SerializeField] private Transform cam;

    private float rangeHit = 5.0f;

    [SerializeField] private LayerMask groundMask;
    
    private void Start() {
        
    }

    private void Update() {
        DestroyBlocks();
        AddBlocks();
    }

    private void DestroyBlocks() {
        if(Input.GetMouseButtonDown(1)) {
            RaycastHit hit;

            if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
                Vector3 pointPos = hit.point - hit.normal / 2;

                Chunk c = Chunk.GetChunk(new Vector3(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                ));

                c.SetBlock(pointPos, BlockType.air);
            }
        }
    }

    private void AddBlocks() {
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;

            if(Physics.Raycast(cam.position, cam.forward, out hit, rangeHit, groundMask)) {
                Vector3 pointPos = hit.point + hit.normal / 2;

                bool isValidPosition = 
                    // se o valor for abaixo de 0.82f, ainda coloca bloco dentro do player
                    // a altura do pulo é de 1.2522f
                    // não ta dando pra colcoar bloco em baixo do player quando pula
                    Vector3.Distance(this.transform.position, pointPos) > 1.0f &&
                    Vector3.Distance(cam.position, pointPos) > 1.0f;

                if(this.transform.position.y - 1 > Chunk.ChunkSize.y) {
                    return;
                }
                else if(isValidPosition) {
                    Chunk c = Chunk.GetChunk(new Vector3(
                        Mathf.FloorToInt(pointPos.x),
                        Mathf.FloorToInt(pointPos.y),
                        Mathf.FloorToInt(pointPos.z)
                    ));

                    c.SetBlock(pointPos, BlockType.stone);
                }                
            }
        }
    }
}
