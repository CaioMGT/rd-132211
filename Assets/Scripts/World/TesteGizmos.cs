using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteGizmos : MonoBehaviour {
    void Start() {
        
    }

    void Update() {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}
