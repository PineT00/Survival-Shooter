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

    // ���� ������Ƽ
    public float vertical { get; private set; }
    public float horizontal { get; private set; }
    public bool fire { get; private set; } // ������ �߻� �Է°�
    public bool reload { get; private set; } // ������ ������ �Է°�




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
