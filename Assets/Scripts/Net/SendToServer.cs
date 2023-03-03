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

public class SendToServer
{

    public static byte[] buffer = new byte[1 << 16];
    public static void SendLoginInfo(string username, string passwd)
    {
        UserAccountBody userAccountBody = new UserAccountBody
        {
            Name = username,
            Passwd = passwd,
        };

        var buffer2 = userAccountBody.ToByteArray();
        var buffer1 = IntToByte((1 << RecvFromServer.HEAD_OFFSET) + buffer2.Length);

        RecvFromServer.AddBuffer(ref buffer, buffer1, buffer2, RecvFromServer.HEAD_SIZE, buffer2.Length);
        SocketInfo.clientSocket.Send(buffer, buffer2.Length + RecvFromServer.HEAD_SIZE, 0);
    }


    // Using Dynamic Calc BodyLength
    public static void SendPositionInfo(int dir)
    {
        UserPositionBody userPositionBody = new UserPositionBody
        {
            Dir = dir,
            Identity = RoleInfo.identity,
            Framecount = 0,
        };

        var buffer2 = userPositionBody.ToByteArray();
        var buffer1 = IntToByte((2 << RecvFromServer.HEAD_OFFSET) + buffer2.Length);

        RecvFromServer.AddBuffer(ref buffer, buffer1, buffer2, RecvFromServer.HEAD_SIZE, buffer2.Length);
        SocketInfo.clientSocket.Send(buffer, buffer2.Length + RecvFromServer.HEAD_SIZE, 0);
    }

    public static void PlayerDropInfo()
    {

        DropBody dropBody = new DropBody
        {
            Identity = RoleInfo.identity,
        };

        var buffer2 = dropBody.ToByteArray();
        var buffer1 = IntToByte((3 << RecvFromServer.HEAD_OFFSET) + buffer2.Length);

        RecvFromServer.AddBuffer(ref buffer, buffer1, buffer2, RecvFromServer.HEAD_SIZE, buffer2.Length);
        SocketInfo.clientSocket.Send(buffer, buffer2.Length + RecvFromServer.HEAD_SIZE, 0);

    }

    public static byte[] IntToByte(int x)
    {
        byte[] ret = new byte[4];
        ret[0] = (byte)((x >> 0) & 255);
        ret[1] = (byte)((x >> 8) & 255);
        ret[2] = (byte)((x >> 16) & 255);
        ret[3] = (byte)((x >> 24) & 255);
        return ret;
    }

    public static int ByteToInt(byte[] x)
    {
        int ret = 0;
        ret = (ret << 8) + (int)x[3];
        ret = (ret << 8) + (int)x[2];
        ret = (ret << 8) + (int)x[1];
        ret = (ret << 8) + (int)x[0];
        return ret;
    }
}
