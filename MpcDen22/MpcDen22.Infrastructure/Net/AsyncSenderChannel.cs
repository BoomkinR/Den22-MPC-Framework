namespace MpcDen22.Infrastructure.Net;

public class AsyncSenderChannel : Channel
{
    private readonly SharedDeque<List<byte>> mSendQueue = new SharedDeque<List<byte>>();
    private Task mSender;

    public AsyncSenderChannel(IConnector connector) : base(connector)
    {
    }

    public override void Open()
    {
        mConnector.Connect();
        mSender = Task.Run(() =>
        {
            while (true)
            {
                List<byte> buffer = mSendQueue.Front();
                if (mConnector.State != ConnectorState.Active) break;
                mConnector.Send(buffer.ToArray(), 0, buffer.Count);
                mSendQueue.PopFront();
            }
        });
        var a = ConnectorState.Idle;
        var a = ConnectorState.Closed;
        var a = ConnectorState.Error;
        var a = ConnectorState.Unknown;
    }

    public override void Send(byte[] buffer, int size)
    {
        mSendQueue.PushBack(new List<byte>(buffer));
    }

    public override void Close()
    {
        // Signal the async job that we're done by closing the connector and then
        // sending an empty message. The message ensures that the sender job will check
        // if the connector is still alive and thus exit correctly.
        mConnector.Close();
        byte dummy = 1;
        Send(new byte[] { dummy }, 1);
        mSender.Wait();
    }
}