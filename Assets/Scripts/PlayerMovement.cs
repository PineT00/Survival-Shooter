using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector3 inputMovement;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {

        playerAnimator.SetFloat("Move", inputMovement.magnitude);

    }


    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
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

        // 마우스 포인터의 위치를 월드 좌표로 변환
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));
        
        Vector3 direction = mousePosition - transform.position;
        
        Quaternion rotation = Quaternion.LookRotation(direction);
        
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }


}
