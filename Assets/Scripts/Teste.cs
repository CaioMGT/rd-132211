using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour {
    [SerializeField] GameObject cubePrefab;

    int viewDistance = 5;
    
    void Start() {
        CreateSphere();
    }

    void Update() {
        
    }

    /*
    void CreateSphere() {
        int r2 = viewDistance * viewDistance;

        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < 1; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    Vector3 cubeOffset = new Vector3(
                            x,
                            y,
                            z
                        );
                    
                    if(new Vector3(x, y, z).sqrMagnitude < r2) {
                        Instantiate(cubePrefab, cubeOffset, Quaternion.identity, this.transform);
                    }
                }
            }
        }
    }
    //*/

    //*
    void CreateSphere() {
        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < 1; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    Vector3 cubeOffset = new Vector3(
                        x,
                        y,
                        z
                    );

                    float distance = Vector3.Distance(cubeOffset, Vector3.zero);

                    if(distance < viewDistance) {
                        Instantiate(cubePrefab, cubeOffset, Quaternion.identity, this.transform);
                    }
                }
            }
        }
    }
    //*/
}
