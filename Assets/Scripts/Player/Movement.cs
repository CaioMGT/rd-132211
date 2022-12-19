using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private CharacterController characterController;

    private Vector3 moveDirection;

    private float speed;

    // Velocidade andando
    private float walkingSpeed = 4.317f;
    
    private Vector3 velocity;
    
    // Velocidade de queda
    private float fallSpeed = -78.4f;

    private bool isGrounded;

    [SerializeField] private Transform groundCheck;
    private float groundDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;
    
    // Altura do pulo
    private float jumpHeight = 1.2522f;

    private float stepOffset = 1.0f;

    private void Start() {
        CharacterControllerValues();

        speed = walkingSpeed;
    }

    private void Update() {
        MovementUpdate();
        FallUpdate();
        JumpUpdate();
        
        Respawn();

        //StepOffsetUpdate();
    }

    private void CharacterControllerValues() {
        // Se Step Offset for maior que 0.0f, o jogador não passara por uma altura de 2 blocos.
        characterController.stepOffset = 0.0f;
        characterController.center = new Vector3(0.0f, 0.9f, 0.0f);
        characterController.radius = 0.3f;
        characterController.height = 1.8f;
    }

    private void MovementUpdate() {
        // Obtenha a entrada do usuário
        float x = Input.GetAxis("HorizontalAD");
        float z = Input.GetAxis("VerticalWS");

        // Crie um vetor de movimento na direção em que o jogador está olhando
        moveDirection = transform.TransformDirection(new Vector3(x, 0.0f, z));

        // Ajuste a velocidade de movimento
        moveDirection *= speed;

        // Mova o personagem com o Character Controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void FallUpdate() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        velocity.y += fallSpeed * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        if(isGrounded && velocity.y < 0) {
            velocity.y = -2.0f;
        }
    }

    private void JumpUpdate() {
        // Se o jogador estiver no chão e o usuário pressionar o botão de pulo
        if(isGrounded && Input.GetButton("Space")) {
            isGrounded = false;

            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * fallSpeed);
        }
    }

    private void Respawn() {
        // Gera números aleatórios para a posição x e z do jogador
        /*
        float x = Random.Range(-((World.WorldSize.x * Chunk.ChunkSize.x) / 2), ((World.WorldSize.x * Chunk.ChunkSize.x) / 2));
        float z = Random.Range(-((World.WorldSize.z * Chunk.ChunkSize.z) / 2), ((World.WorldSize.z * Chunk.ChunkSize.z) / 2));
        */
        float x = Random.Range(-((World.viewDistance * Chunk.ChunkSize.x) / 2), ((World.viewDistance * Chunk.ChunkSize.x) / 2));
        float z = Random.Range(-((World.viewDistance * Chunk.ChunkSize.z) / 2), ((World.viewDistance * Chunk.ChunkSize.z) / 2));
        
        // Cria um vetor de movimento com as novas coordenadas
        Vector3 respawn = new Vector3(x, 64.0f, z);

        if(Input.GetKeyDown(KeyCode.R)) {
            /// Aplica a movimentação usando o método Move() do Character Controller
            characterController.Move(respawn);
        }
    }

    private void StepOffsetUpdate() {
        // Envia um raio para baixo a partir da posição atual do personagem
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, stepOffset)) {
            // Se o raio colidir com um obstáculo, move o personagem para cima do obstáculo
            Vector3 newPosition = hit.point + Vector3.up * stepOffset;
            characterController.Move(newPosition - transform.position);
        }
        else {
            // Se o raio não colidir com nenhum obstáculo, move o personagem normalmente
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}
