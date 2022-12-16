using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRigidbody : MonoBehaviour {
    [SerializeField] Rigidbody player;

    float speed;

    // Velocidade andando
    float walking = 4.317f;
    
    // Altura do pulo
    float jumpHeight = 1.2522f;

    //float gravity = -9.807f;
    
    // Velocidade de queda
    float falling = -78.4f;
    
    void Start() {
        speed = walking;
    }

    void Update() {
        
    }

    void FixedUpdate() {
        Movement();
        JumpMovement();
    }

    void Movement() {
        float x = Input.GetAxis("HorizontalAD");
        float z = Input.GetAxis("VerticalWS");

        Vector3 move = transform.right * x + transform.forward * z;
        //Vector3 move = new Vector3(x, 0, z);

        //Move Position
        //player.MovePosition(transform.position + move * speed * Time.deltaTime);

        //Alterando direto a velocidade
        //player.velocity = move * speed;
        //player.velocity = new Vector3(x * speed, player.velocity.y, z * speed);
        //player.velocity = new Vector3(x * speed, falling, z * speed);

        // Adicioando força
        //player.AddForce(move * speed);
    }

    void JumpMovement() {
        // Pressionar Space pula.
        if(Input.GetButton("Space")) {
            
        }
    }
}
