using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCharacterController : MonoBehaviour {
    [SerializeField] Transform playerTransform;
    bool respawning;

    [Space(20)]
    [SerializeField] CharacterController player;

    float speed;

    // Velocidade andando
    float walking = 4.317f;
    
    // Altura do pulo
    float jumpHeight = 1.2522f;

    //float gravity = -9.807f;
    
    // Velocidade de queda
    float falling = -78.4f;

    Vector3 velocity;
    bool isGrounded;

    [SerializeField] Transform groundCheck;
    float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    void Start() {
        speed = walking;
    }

    void Update() {
        Respawn();
        
        if(!respawning) {
            Movement();        
            JumpMovement();

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            velocity.y += falling * Time.deltaTime;

            player.Move(velocity * Time.deltaTime);

            if(isGrounded && velocity.y < 0) {
                velocity.y = -2.0f;
            }
        }
    }

    void Respawn() {
        int x = Random.Range(-((World.WorldSize.x * Chunk.ChunkSize.x) / 2), ((World.WorldSize.x * Chunk.ChunkSize.x) / 2));
        int y = 64;
        int z = Random.Range(-((World.WorldSize.z * Chunk.ChunkSize.z) / 2), ((World.WorldSize.z * Chunk.ChunkSize.z) / 2));

        if(Input.GetKeyDown(KeyCode.R)) {
            respawning = true;

            playerTransform.position = new Vector3(x, y, z);
        }
        else {
            respawning = false;
        }
    }

    void Movement() {
        float x = Input.GetAxis("HorizontalAD");
        float z = Input.GetAxis("VerticalWS");

        Vector3 move = transform.right * x + transform.forward * z;

        player.Move(move * speed * Time.deltaTime);
    }

    void JumpMovement() {
        // Pressionar Space pula.
        if(Input.GetButton("Space") && isGrounded) {
            isGrounded = false;

            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * falling);
        }
    }
}
