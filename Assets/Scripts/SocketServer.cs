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
    //  ��ȡ�ͻ��˴��������
    private string getClientSendMes;
    //  txt�ı���·��
    private string ipTxtPath = Application.streamingAssetsPath + "/IPAddress.txt";

    public string ipText = "192.168.42.34";
    /// <summary>
    /// Thread:��������socket���߳�
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
    /// �����ͻ��˷�������
    /// </summary>
    private void StartServer()
    {
        try
        {
            //  string _ip = "127.0.0.1";
            //string _ip = File.ReadAllText(ipTxtPath);

            //�����ʼ����ʱ �ڷ���˴���һ���������IP�Ͷ˿ںŵ�Socket
            Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipText);
            //��������˿�
            IPEndPoint point = new IPEndPoint(ip, 8001);

            socketWatch.Bind(point);//�󶨶˿ں�
            Debug.Log("�����ɹ�!");
            socketWatch.Listen(10);//���ü��������ͬʱ����10̨

            //���������߳�
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(socketWatch);
        }
        catch { }
    }

    /// <summary>
    /// �ȴ��ͻ��˵����� ���Ҵ�����֮ͨ�ŵ�Socket
    /// </summary>
    Socket socketSend;
    void Listen(object o)
    {
        try
        {
            Socket socketWatch = o as Socket;

            while (true)
            {
                socketSend = socketWatch.Accept();//�ȴ����տͻ�������
                Debug.Log(socketSend.RemoteEndPoint.ToString() + ":" + "���ӳɹ�!");
                //����һ�����̣߳�ִ�н�����Ϣ����
                Thread r_thread = new Thread(Received);
                r_thread.IsBackground = true;
                r_thread.Start(socketSend);
            }
        }
        catch { }
    }

    /// <summary>
    /// �������˲�ͣ�Ľ��տͻ��˷�������Ϣ
    /// </summary>
    /// <param name="o"></param>
    void Received(object o)
    {
        try
        {
            Socket socketSend = o as Socket;
            while (true)
            {
                //�ͻ������ӷ������ɹ��󣬷��������տͻ��˷��͵���Ϣ
                byte[] buffer = new byte[1024 * 1024 * 3];
                //ʵ�ʽ��յ�����Ч�ֽ���
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
                //  LoadImage(Application.streamingAssetsPath + "/" + str);  ֱ�Ӹ���UIû��Ӧ
                //  SendMesToClient("���յ���" + str);
            }
        }
        catch { }
    }

    /// <summary>
    /// ��������ͻ��˷�����Ϣ
    /// </summary>
    /// <param name="str"></param>
    void SendMesToClient(string str)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        socketSend.Send(buffer);
    }

    /// <summary>
    /// ����·������ͼƬ
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