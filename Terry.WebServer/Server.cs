using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Terry.WebServer;

public class Server
{
    public static int maxSimultaneousConnections = 20;
    private static Semaphore sem = new Semaphore(maxSimultaneousConnections, maxSimultaneousConnections);


    public static void Start()
    {
        List<IPAddress> localHostIPs = GetLocalHostIPs();
        HttpListener listener = InitializeListener(localHostIPs);
        Start(listener);
    }

    private static void Start(HttpListener listener)
    {
        listener.Start();
        Task.Run(() => RunServer(listener));
    }

    private static void RunServer(HttpListener listener)
    {
        while (true)
        {
            sem.WaitOne();
            StartConnectionListener(listener);
        }
    }

    private static async void StartConnectionListener(HttpListener listener)
    {
        HttpListenerContext context = await listener.GetContextAsync();

        string response = "Hello Browser!";
        byte[] encoded = Encoding.UTF8.GetBytes(response);
        context.Response.ContentLength64 = encoded.Length;
        context.Response.OutputStream.Write(encoded, 0, encoded.Length);
        context.Response.OutputStream.Close();

        sem.Release();
        Log(context.Request);
    }

    private static List<IPAddress> GetLocalHostIPs()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        List<IPAddress> ret
            = host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();

        return ret;
    }

    private static HttpListener InitializeListener(List<IPAddress> localhostIps)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost/");

        localhostIps.ForEach(ip =>
        {
            Console.WriteLine("Listening on IP " + "http://" + ip.ToString() + "/");
            listener.Prefixes.Add("http://" + ip.ToString() + "/");
        });

        return listener;
    }

    public static void Log(HttpListenerRequest request)
    {
        Console.WriteLine($"{request.RemoteEndPoint} {request.HttpMethod} {request.Url?.AbsoluteUri}");
    }
}