using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector3 inputMovement;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����

    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {

        playerAnimator.SetFloat("Move", inputMovement.magnitude);

    }


    // FixedUpdate�� ���� ���� �ֱ⿡ ���� �����
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        //var pos = playerRigidbody.position;

        inputMovement = new Vector3(playerInput.horizontal, 0, playerInput.vertical);

        if (inputMovement.magnitude > 1f)
        {
            inputMovement.Normalize();
        }

        playerRigidbody.velocity = inputMovement * moveSpeed * Time.deltaTime;

    }

    private void Rotate()
    {
        Vector3 mousePosition = playerInput.mousePos;

        // ���콺 �������� ��ġ�� ���� ��ǥ�� ��ȯ
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));
        
        Vector3 direction = mousePosition - transform.position;
        
        Quaternion rotation = Quaternion.LookRotation(direction);
        
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }


}
