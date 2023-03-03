using System.Text;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using Proto;
using System;
using System.Threading;

public class Performance : MonoBehaviour
{
    public static int listenEvent = 0;

    private const int FRAME_COUNT = 30;
    private const int MAX_PLAYER_COUNT = 16;
    private static bool[] isExist = new bool[MAX_PLAYER_COUNT];

    private static int speed = 120;

    private static Vector3 unitDis = new Vector3();

    private void Start()
    {

    }

    private void Update()
    {
        if (listenEvent != 0)
        {
            if (listenEvent == 1)
            {
                DoPerformance(RecvFromServer.respBody);
            }
            if (listenEvent == 2)
            {
                DoDeletePlayer(RecvFromServer.respBody);
            }
            listenEvent = 0;
        }
        if (!Application.isPlaying)
        {
            Debug.LogError("Socket Closed");
            Performance.PlayerDestory(RoleInfo.identity);

            SendToServer.PlayerDropInfo();

            SocketInfo.CloseSocket();
        }
        if (Time.timeScale == 0)
        {
            // 游戏结束播放，执行相关操作
            Debug.LogError("Socket Closed");
            Performance.PlayerDestory(RoleInfo.identity);

            SendToServer.PlayerDropInfo();

            SocketInfo.CloseSocket();
        }
    }

    public static void DoDeletePlayer(UserPositionBody userPosition)
    {
        if (isExist[userPosition.Identity])
        {
            isExist[userPosition.Identity] = false;

            GameObject.Find(userPosition.Identity.ToString()).SetActive(false);
        }
    }
    public static void DoPerformance(UserPositionBody userPosition)
    {
        // Debug.Log("id = " + userPosition.Identity);
        if (!isExist[userPosition.Identity])
        {
            isExist[userPosition.Identity] = true;

            GameObject instance = GameObject.Instantiate(GameObject.Find("My"));
            //GameObject prefabInstance = Resources.Load<GameObject>("PrefabRole");
            //GameObject instance = Instantiate(prefabInstance);

            instance.transform.position = Vector3.zero;
            instance.name = userPosition.Identity.ToString();
        }
        // string str = (userPosition.Identity == RoleInfo.identity) ? "My" : userPosition.Identity.ToString();
        string str = userPosition.Identity.ToString();
        GameObject tmp = GameObject.Find(str);

        Debug.Log("NAME = " + tmp.name);

        // 渲染帧的处理，我们可以进行插值 -> 提高渲染帧的Hz

        int dir = userPosition.Dir;

        if ((dir & 1) == 1)
        {
            unitDis.y = 1;
        }
        if ((dir & 2) == 2)
        {
            unitDis.x = -1;
        }
        if ((dir & 4) == 4)
        {
            unitDis.y = -1;
        }
        if ((dir & 8) == 8)
        {
            unitDis.x = 1;
        }
        tmp.transform.position += unitDis * speed * (float)(1.0f / FRAME_COUNT);
        unitDis = Vector3.zero;
    }

    public static void PlayerDestory(int identity)
    {
        isExist[identity] = false;
    }
}
