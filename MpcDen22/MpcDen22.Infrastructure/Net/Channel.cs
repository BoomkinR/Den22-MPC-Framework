namespace MpcDen22.Infrastructure.Net;

public class Channel
{
    protected readonly IConnector mConnector;

    public Channel(IConnector connector)
    {
        mConnector = connector ?? throw new ArgumentNullException(nameof(connector));
    }

    public virtual void Open()
    {
        mConnector.Connect();
    }

    public virtual void Close()
    {
        mConnector.Close();
    }

    public virtual void Send(byte[] buffer, int size)
    {
        int bytesSent = 0;
        while (bytesSent < size)
        {
            int n = mConnector.Send(buffer, bytesSent, size - bytesSent);
            bytesSent += n;
        }
    }

    public virtual void Receive(byte[] buffer, int size)
    {
        int bytesReceived = 0;
        while (bytesReceived < size)
        {
            int n = mConnector.Receive(buffer, bytesReceived, size - bytesReceived);
            bytesReceived += n;
        }
    }

    public ConnectorState State => mConnector.State;

    public override string ToString()
    {
        return $"<Channel({mConnector.ToString()})>";
    }
}

public interface IConnector
{
    ConnectorState State { get; }

    void Connect();

    void Close();

    int Send(byte[] buffer, int offset, int size);

    int Receive(byte[] buffer, int offset, int size);

    string ToString();
}

public enum ConnectorState
{
    //! State of the connector after initilizations
    Idle,

    //! State of the connector after a connection have been established
    Active,

    //! State of the connector after a connection have been closed
    Closed,

    //! State of the connector if a critical error happens
    Error,

    //! Dummy
    Unknown
}