using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    private Socket serverSocket;
    private byte[] buffer = new byte[60000];
    //public string IP = "";
    public int Port = 8001;

    public RawImage Image;

    void Start()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
        serverSocket.Listen(10);
        serverSocket.BeginAccept(AcceptCallback, null);
    }

    private void AcceptCallback(System.IAsyncResult ar)
    {
        
        Socket clientSocket = serverSocket.EndAccept(ar);
        Debug.Log("客户端连接成功！！"+ clientSocket);
        serverSocket.BeginAccept(AcceptCallback, null);
        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
    }

    private void ReceiveCallback(System.IAsyncResult ar)
    {
        Socket clientSocket = (Socket)ar.AsyncState;
        int received = clientSocket.EndReceive(ar);
        if (received > 0)
        {
            byte[] imageBytes = new byte[received];
            Buffer.BlockCopy(buffer, 0, imageBytes, 0, received);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes); // 应用图片字节流
            //Texture2D texture=new Texture2D(2048, 1024);
            //texture.LoadImage(buffer);
            Image.texture = texture;


            //string message = Encoding.UTF8.GetString(buffer, 0, received);
            //Debug.Log("Received: " + message);
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
        }
    }
}