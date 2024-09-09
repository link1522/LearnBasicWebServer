using System.Net;

namespace Terry.WebServer;

public class SessionManager
{
    protected Server _server;
    protected Dictionary<IPAddress, Session?> _sessionMap;

    public SessionManager(Server server)
    {
        _server = server;
        _sessionMap = new Dictionary<IPAddress, Session?>();
    }

    public Session GetSession(IPEndPoint remoteEndPoint)
    {
        Session? session = _sessionMap.GetValueOrDefault(remoteEndPoint.Address);

        if (session == null)
        {
            session = new Session();
            session[_server.VaildationTokenName] = Guid.NewGuid().ToString();
            _sessionMap[remoteEndPoint.Address] = session;
        }

        return session;
    }
}