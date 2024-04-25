using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string verticlalAxisName = "Vertical";
    public static readonly string HorizontalAxisName = "Horizontal";
    public static readonly string fireButtonName = "Fire1";
    public static readonly string reloadButtonName = "Reload";

    public Vector3 mousePos;

    // 오토 프로퍼티
    public float vertical { get; private set; }
    public float horizontal { get; private set; }
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값




    private void Update()
    {
        //if (GameManager.instance != null && GameManager.instance.isGameover)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    reload = false;
        //    return;
        //}

        vertical = Input.GetAxis(verticlalAxisName);
        horizontal = Input.GetAxis(HorizontalAxisName);

        mousePos = Input.mousePosition;

        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}
