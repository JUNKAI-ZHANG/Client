using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using Proto;


public class Move : MonoBehaviour
{
    private const int KEY_W = 1;
    private const int KEY_A = 2;
    private const int KEY_S = 4;
    private const int KEY_D = 8;

    private int KEY_PRE = 0;
    void Update()
    {
        // W A S D
        // 1 2 4 8

        int ret = 0;

        if (Input.GetKey(KeyCode.W))
        {
            ret |= KEY_W;
        }
        if (Input.GetKey(KeyCode.A))
        {
            ret |= KEY_A;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ret |= KEY_S;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ret |= KEY_D;
        }
        if ((ret & (KEY_W + KEY_S)) == (KEY_W + KEY_S))
        {
            ret ^= (KEY_W + KEY_S);
        }
        if ((ret & (KEY_A + KEY_D)) == (KEY_A + KEY_D))
        {
            ret ^= (KEY_A + KEY_D);
        }

        if (RecvFromServer.isDebug) Debug.Log("ret = " + ret);

        if (ret != KEY_PRE)
        {
            KEY_PRE = ret;
            // 发送位置信息给服务端
            SendToServer.SendPositionInfo(KEY_PRE);
        }
    }
    private static void OnDestroy()
    {
      
    }
}
