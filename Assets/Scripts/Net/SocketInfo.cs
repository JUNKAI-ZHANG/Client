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

public class SocketInfo : MonoBehaviour
{
    public static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    RecvFromServer recvFromServer = new RecvFromServer();

    private void Awake()
    {
        recvFromServer.Connection("10.0.128.153", 8080);
    }
    public static void CloseSocket()
    {
        // 关闭socket连接.
        SocketInfo.clientSocket.Shutdown(SocketShutdown.Both);
        SocketInfo.clientSocket.Close();
    }
    void OnDestroy()
    {
        Debug.LogError("Socket Closed");
        Performance.PlayerDestory(RoleInfo.identity);

        SendToServer.PlayerDropInfo();

        SocketInfo.CloseSocket();
    }
}
