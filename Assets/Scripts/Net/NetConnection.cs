using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Net;
using UnityEditor;
using System.Text;
using System.Net.Sockets;
using Google.Protobuf;
using Proto;

public class NetConnection {

    //[MenuItem("Nets/TcpConnection")]
    public static void GetTcp()
    {
        // 创建一个TCP/IP socket对象.
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 连接到服务器.
        IPAddress ipAddress = IPAddress.Parse("10.0.128.153");
        IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, 8080);
        clientSocket.Connect(serverEndPoint);

        // 发送数据到服务器.
        byte[] bytesSend = System.Text.Encoding.UTF8.GetBytes("Hello, server!!!");
        clientSocket.Send(bytesSend);
        
        // 从服务器接收数据.
        byte[] buffer = new byte[1024];
        int bytesRead = clientSocket.Receive(buffer);
        string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Debug.Log(receivedData);
        if (receivedData == "Success")
        {
            SceneManager.LoadScene(1);
        }

        // 关闭socket连接.
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}
