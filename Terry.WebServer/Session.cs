namespace Terry.WebServer;

public class Session
{
    public DateTime LastConnection { get; set; }
    public bool Authenticated { get; set; }
    private Dictionary<string, object> Objects { get; set; }

    public object? this[string objectKey]
    {
        get
        {
            Objects.TryGetValue(objectKey, out object? val);

            return val;
        }

        set { Objects[objectKey] = value; }
    }


    public T? GetObject<T>(string objectKey)
    {
        T? ret = default;

        if (Objects.TryGetValue(objectKey, out object? val))
        {
            ret = (T)Convert.ChangeType(val, typeof(T));
        }

        return ret;
    }

    public Session()
    {
        Objects = new Dictionary<string, object>();
        UpdateLateConnectionTime();
    }

    public void UpdateLateConnectionTime()
    {
        LastConnection = DateTime.Now;
    }

    public bool IsExpired(int expirationInSeconds)
    {
        return (DateTime.Now - LastConnection).TotalSeconds > expirationInSeconds;
    }

    public void Expire()
    {
        Authenticated = false;
    }
}