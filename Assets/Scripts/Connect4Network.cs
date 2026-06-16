    
   using UnityEngine;
using TMPro;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class Connect4Network : MonoBehaviour
{
    [Header("Rede")]
    public bool isServer; //True para a máquina que fica "esperando", false para a outra
    public string serverIP = "127.0.0.1"; //IP da máquina que foi ligada primeiro
    public int port = 7777;

    [Header("Cena")]
    public TMP_Text logText;
    public GameObject remoteCube;

    TcpListener listener;
    TcpClient client;
    NetworkStream stream;

    Thread networkThread;
    Thread receiveThread;

    Queue<string> messages =
        new Queue<string>();

    void Start()
    {
        if(isServer)
        {
            networkThread =
                new Thread(StartServer);

            networkThread.Start();
        }
        else
        {
            networkThread =
                new Thread(StartClient);

            networkThread.Start();
        }
    }

    void StartServer()
    {
        AddMessage("Servidor iniciado");

        listener =
            new TcpListener(
                IPAddress.Any,
                port);

        listener.Start();

        AddMessage("Esperando conexão...");

        client =
            listener.AcceptTcpClient();

        stream =
            client.GetStream();

        AddMessage("Cliente conectado");

        StartReceiveThread();
    }

    void StartClient()
    {
        AddMessage("Conectando...");

        client =
            new TcpClient();

        client.Connect(
            serverIP,
            port);

        stream =
            client.GetStream();

        AddMessage("Conectado");

        StartReceiveThread();
    }

    void StartReceiveThread()
    {
        receiveThread =
            new Thread(ReceiveLoop);

        receiveThread.Start();
    }

    void ReceiveLoop()
    {
        byte[] buffer =
            new byte[1024];

        while(true)
        {
            int size =
                stream.Read(
                    buffer,
                    0,
                    buffer.Length);

            string msg =
                Encoding.UTF8.GetString(
                    buffer,
                    0,
                    size);

            AddMessage(
                "Recebido: " + msg);

            HandleMessage(msg);
        }
    }

    void HandleMessage(string msg)
    {
        if(msg.StartsWith("PLAY_"))
        {
            string[] data =
                msg.Split('_');

            int column =
                int.Parse(data[1]);

            int player =
                int.Parse(data[2]);

            MainThreadAction(() =>
            {
                PieceSpawner.Instance.ReceivePlay(
                    column,
                    player);
            });
        }
    }

    public void SendPlay(
        int column,
        int player)
    {
        Send(
            "PLAY_" +
            column +
            "_" +
            player);
    }

    Queue<System.Action> actions =
        new Queue<System.Action>();

    void MainThreadAction(
        System.Action action)
    {
        lock(actions)
        {
            actions.Enqueue(action);
        }
    }

    public void SendLightOn()
    {
        Send("LIGHT_ON");
    }

    public void SendLightOff()
    {
        Send("LIGHT_OFF");
    }

    void Send(string msg)
    {
        if(stream == null)
            return;

        byte[] data =
            Encoding.UTF8.GetBytes(msg);

        stream.Write(
            data,
            0,
            data.Length);

        AddMessage(
            "Enviado: " + msg);
    }

    void AddMessage(string msg)
    {
        lock(messages)
        {
            messages.Enqueue(msg);
        }
    }

    void Update()
    {
        lock(messages)
        {
            while(messages.Count > 0)
            {
                logText.text +=
                    "\n" +
                    messages.Dequeue();
            }
        }

        lock(actions)
        {
            while(actions.Count > 0)
            {
                actions.Dequeue().Invoke();
            }
        }
    }

    void OnDestroy()
    {
        receiveThread?.Abort();
        networkThread?.Abort();

        stream?.Close();
        client?.Close();
        listener?.Stop();
    }
}