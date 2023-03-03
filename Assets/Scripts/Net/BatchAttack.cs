using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEditor;
using System.Text;
using System.Net.Sockets;
using Google.Protobuf;
using Proto;

public class BatchAttack : MonoBehaviour
{
    void Update()
    {
        /*
        // 创建一个TCP/IP socket对象.
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 连接到服务器.
        IPAddress ipAddress = IPAddress.Parse("10.0.128.153");
        IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, 8000);
        clientSocket.Connect(serverEndPoint);

        int sz = 19;

        Header header = new Header
        {
            BodyLength = sz,
            Type = Header.Types.DataType.UserAccountBody,
        };

        UserAccountBody userAccountBody = new UserAccountBody
        {
            Name = "Zjkai",
            Passwd = "123456",
        };

        // 发送数据到服务器.
        var it1 = header.ToByteArray();
        var it2 = userAccountBody.ToByteArray();
        byte[] data = new byte[sz];
        for (int i = 0; i < sz; i++) data[i] = 0x0;
        for (int i = 0; i < System.Math.Min(4, (int)it1.Length); i++) data[i] = it1[i];
        for (int i = 0; i < System.Math.Min(sz - 4, (int)it2.Length); i++) data[i + 4] = it2[i];
        clientSocket.Send(data, sz, 0);

        // 从服务器接收数据.
        byte[] buffer = new byte[1024];
        int bytesRead = clientSocket.Receive(buffer);
        Header header1 = new Header();
        header1.MergeFrom(buffer, 0, 4);
        Debug.Log(header1.BodyLength);
        LoginResponse resp = new LoginResponse();
        resp.MergeFrom(buffer, 4, (header1.BodyLength) - 4);

        Debug.Log(resp.Result);
        Debug.Log(resp.Msg);

        byte[] receiveBytes = new byte[1024];
        int receiveSize = clientSocket.Receive(receiveBytes);
        string receiveString = System.Text.Encoding.UTF8.GetString(receiveBytes, 0, receiveSize);
        Debug.Log(receiveString);

        // 关闭socket连接.
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
        */
    }

}