using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SocketServer : MonoBehaviour
{
    [SerializeField] private Image image;

    private bool isSuccessGetStr;
    //  获取客户端传输的数据
    private string getClientSendMes;
    //  txt文本的路径
    private string ipTxtPath = Application.streamingAssetsPath + "/IPAddress.txt";

    public string ipText = "192.168.42.34";
    /// <summary>
    /// Thread:定义启动socket的线程
    /// </summary>
    //  private Thread thStartServer;

    void Update()
    {
        if (isSuccessGetStr)
        {
            isSuccessGetStr = !isSuccessGetStr;
            LoadImage(getClientSendMes);
        }
    }
    void Start()
    {
        StartServer();
        //thStartServer = new Thread(StartServer);
        //thStartServer.IsBackground = true;
        //thStartServer.Start();
    }

    /// <summary>
    /// 监听客户端发起连接
    /// </summary>
    private void StartServer()
    {
        try
        {
            //  string _ip = "127.0.0.1";
            //string _ip = File.ReadAllText(ipTxtPath);

            //点击开始监听时 在服务端创建一个负责监听IP和端口号的Socket
            Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipText);
            //创建对象端口
            IPEndPoint point = new IPEndPoint(ip, 8001);

            socketWatch.Bind(point);//绑定端口号
            Debug.Log("监听成功!");
            socketWatch.Listen(10);//设置监听，最大同时连接10台

            //创建监听线程
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(socketWatch);
        }
        catch { }
    }

    /// <summary>
    /// 等待客户端的连接 并且创建与之通信的Socket
    /// </summary>
    Socket socketSend;
    void Listen(object o)
    {
        try
        {
            Socket socketWatch = o as Socket;

            while (true)
            {
                socketSend = socketWatch.Accept();//等待接收客户端连接
                Debug.Log(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功!");
                //开启一个新线程，执行接收消息方法
                Thread r_thread = new Thread(Received);
                r_thread.IsBackground = true;
                r_thread.Start(socketSend);
            }
        }
        catch { }
    }

    /// <summary>
    /// 服务器端不停的接收客户端发来的消息
    /// </summary>
    /// <param name="o"></param>
    void Received(object o)
    {
        try
        {
            Socket socketSend = o as Socket;
            while (true)
            {
                //客户端连接服务器成功后，服务器接收客户端发送的消息
                byte[] buffer = new byte[1024 * 1024 * 3];
                //实际接收到的有效字节数
                int len = socketSend.Receive(buffer);
                if (len == 0)
                {
                    break;
                }
                //string str = Encoding.UTF8.GetString(buffer, 0, len);

                //getClientSendMes = Application.streamingAssetsPath + "/" + str;
                //isSuccessGetStr = true;

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(buffer);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
                //  LoadImage(Application.streamingAssetsPath + "/" + str);  直接更新UI没反应
                //  SendMesToClient("我收到了" + str);
            }
        }
        catch { }
    }

    /// <summary>
    /// 服务器向客户端发送消息
    /// </summary>
    /// <param name="str"></param>
    void SendMesToClient(string str)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        socketSend.Send(buffer);
    }

    /// <summary>
    /// 本地路径加载图片
    /// </summary>
    /// <param name="path"></param>
    private void LoadImage(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }
}