using System.Net;
using System.Text;

namespace Terry.WebServer;
public class ResponsePacket
{
    public string? Redirect { get; set; }
    public byte[]? Data { get; set; }
    public string? ContentType { get; set; }
    public Encoding? Encoding { get; set; }
    public Server.ServerError Error { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public ResponsePacket()
    {
        Error = Server.ServerError.OK;
        StatusCode = HttpStatusCode.OK;
    }
}