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

public class RecvFromServer
{
    public static bool isDebug = false;
    public static int HEAD_SIZE = 4;
    public static int HEAD_OFFSET = 16;
    private static int MAX_BUFFER_SIZE = 65536;

    public static UserPositionBody respBody = new UserPositionBody();

    private int offset = 0;
    private static byte[] buffer = new byte[MAX_BUFFER_SIZE];
    public void Connection(string serverIP, int serverPort) // 从服务器接收数据.
    {
        // 解析服务器IP地址
        IPAddress ipAddress = IPAddress.Parse(serverIP);

        // 连接到服务器
        SocketInfo.clientSocket.Connect(new IPEndPoint(ipAddress, serverPort));

        // 连接成功，开始异步接收数据
        SocketInfo.clientSocket.BeginReceive(buffer, offset, buffer.Length - offset, SocketFlags.None, ReceiveCallback, null);
    }
    private void ReceiveCallback(IAsyncResult ar)
    {
        int bytesReceive = SocketInfo.clientSocket.EndReceive(ar);
        if (bytesReceive > 0)
        {
            bool flag_header = false, flag_body = false;
            offset += bytesReceive;

            byte[] head = new byte[4];
            byte[] body = new byte[2048];
            do
            {
                int bodySize = 0, bodyType = -1, bodyLength = 0;
                flag_header = false;
                flag_body = false;

                if (bodySize <= 0 && offset >= HEAD_SIZE)
                {
                    head = GetResponseHeader(buffer, HEAD_SIZE);

                    bodyLength = SendToServer.ByteToInt(head);

                    bodySize = bodyLength & ((1 << HEAD_OFFSET) - 1);
                    bodyType = bodyLength >> HEAD_OFFSET;

                    flag_header = true;

                }

                if (bodySize > 0 && offset >= bodySize + HEAD_SIZE)
                {
                    body = GetResponseBody(buffer, bodySize);

                    bodySize += HEAD_SIZE;

                    for (int i = 0; i < offset - bodySize; i++)
                    {
                        buffer[i] = buffer[i + bodySize];
                    }
                    offset -= bodySize;
                    bodySize = 0;
                    flag_body = true;

                }
                if (flag_header && flag_body)
                {
                    bodySize = bodyLength & ((1 << HEAD_OFFSET) - 1);
                    bodyType = bodyLength >> (HEAD_OFFSET);

                    if (bodyType == 0)
                    {
                        // Protobuf Will Do Optimistic, FORBIDDEN
                    }
                    if (bodyType == 1)
                    {
                        // Login Response
                        LoginResponse loginResponse = new LoginResponse();
                        loginResponse.MergeFrom(body, 0, bodySize);

                        Debug.Log(loginResponse.ResponseMsg);

                        if (loginResponse.Status == true)
                        {
                            //Debug.Log(loginResponse);

                            RoleInfo.identity = loginResponse.Identity;
                            Debug.Log("identity = " + RoleInfo.identity);

                            // SceneManager.LoadScene(1);    <==>   这里异步回调的实现原理是多线程CallBack，多线程不能调用一些关于修改UI的函数
                            // 处理方法：使用一个空的 GameObject ，Update里监听这里的事件即可
                            ComponmentScripts.willLoadingScene = 1;
                        }
                    }
                    if (bodyType == 2)
                    {
                        // Recv User Position

                        respBody = new UserPositionBody();
                        respBody.MergeFrom(body, 0, bodySize);

                        //Performance.DoPerformance(respBody);
                        Performance.listenEvent = 1;

                    }
                    if (bodyType == 3)
                    {
                        respBody = new UserPositionBody();
                        respBody.MergeFrom(body, 0, bodySize);

                        Performance.listenEvent = 2;
                    }
                }
            } while (flag_header && flag_body);
        }
        SocketInfo.clientSocket.BeginReceive(buffer, offset, buffer.Length - offset, SocketFlags.None, ReceiveCallback, null);
    }

    public static void AddBuffer(ref byte[] ResultBuffer, byte[] bf1, byte[] bf2, int sz1, int sz2)
    {
        for (int i = 0; i < sz1 + sz2; i++) ResultBuffer[i] = 0x0;
        for (int i = 0; i < System.Math.Min(sz1, (int)bf1.Length); i++) ResultBuffer[i] = bf1[i];
        for (int i = 0; i < System.Math.Min(sz2, (int)bf2.Length); i++) ResultBuffer[i + sz1] = bf2[i];
    }
    public static byte[] GetResponseHeader(byte[] buf, int recvLen)
    {
        if (recvLen >= HEAD_SIZE)
        {
            byte[] head = new byte[HEAD_SIZE];

            for (int i = 0; i < HEAD_SIZE; i++)
            {
                head[i] = buf[i];
            }

            return head;
        }

        return null;
    }

    public static byte[] GetResponseBody(byte[] buf, int recvLen)
    {
        if (recvLen > 0)
        {
            byte[] body = new byte[recvLen];

            for (int i = 0; i < recvLen; i++)
            {
                body[i] = buf[i + HEAD_SIZE];
            }

            return body;
        }

        return null;
    }
}