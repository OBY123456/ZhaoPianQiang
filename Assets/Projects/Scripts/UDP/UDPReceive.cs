using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using Newtonsoft.Json;
using MTFrame.MTEvent;

public class UDPReceive : MonoBehaviour
{
    private IPEndPoint ipEndPoint;
    private Socket socket;
    private Thread thread;
    private byte[] bytes;           //接收到的字节
    private int bytesLength;        //长度
    private string receiveMsg = "";   //接收到的信息
    
    void Start()
    {
        Init();
    }
    //初始化
    private void Init()
    {
        string str = FileHandle.Instance.FileToString(Application.streamingAssetsPath +"/端口号.txt");
        ipEndPoint = new IPEndPoint(IPAddress.Any,int.Parse(str));    //端口号要与发送端一致
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipEndPoint);
        thread = new Thread(new ThreadStart(Receive));      //开启一个线程，接收发送端的消息
        thread.IsBackground = true;
        thread.Start();
    }
    //接收消息函数
    private void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)sender;
        while (true)
        {
            bytes = new byte[1024];
            bytesLength = socket.ReceiveFrom(bytes, ref remote);
            if (bytesLength <= 0)
            {
                return;
            }
            else
            {
                receiveMsg = Encoding.UTF8.GetString(bytes, 0, bytesLength);
                SentToState(receiveMsg);
                LogMsg.Instance.Log("接收的数据==" + receiveMsg);
            }
           
        }
    }
    //关闭socket，关闭thread
    private void OnDisable()
    {
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }
        if (thread != null)
        {
            thread.Interrupt();
            thread.Abort();
        }
    }

    private void SentToState(string msg)
    {
        EventParamete eventParamete = new EventParamete();
        eventParamete.AddParameter(msg);
        EventManager.TriggerEvent(GenericEventEnumType.Message, MTFrame.EventType.receiveMsg.ToString(), eventParamete);
    }
}
