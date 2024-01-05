// namespace MpcDen22.Infrastructure.Net;
//
// /// <summary>
// ///     Network interface.
// /// </summary>
// public abstract class Network
// {
//     private readonly List<Channel> mChannels;
//
//     /// <summary>
//     ///     Constructor for the Network class.
//     /// </summary>
//     /// <param name="id">The identity of this party.</param>
//     /// <param name="size">The number of parties.</param>
//     protected Network(int id, int size)
//     {
//         Id = id;
//         Size = size;
//     }
//
//     // Конструктор
//     private Network(int id, int n, TransportType ttype, List<Channel> channels, ILogger logger)
//     {
//         mId = id;
//         mSize = n;
//         mTransportType = ttype;
//         mChannels = channels;
//         mLogger = logger;
//     }
//
//     /// <summary>
//     ///     Get the ID of this party.
//     /// </summary>
//     public int Id { get; }
//
//     /// <summary>
//     ///     Get the size of the network, i.e., the number of parties.
//     /// </summary>
//     public int Size { get; }
//
//     /// <summary>
//     ///     Send a vector of field elements to another party.
//     /// </summary>
//     /// <param name="id">The ID of the remote party.</param>
//     /// <param name="values">The field elements to send.</param>
//     public abstract void Send(int id, List<Field> values);
//
//     /// <summary>
//     ///     Send a vector of shares to another party.
//     /// </summary>
//     /// <param name="id">The ID of the remote party.</param>
//     /// <param name="shares">The shares to send.</param>
//     public abstract void SendShares(int id, List<Shr> shares);
//
//     /// <summary>
//     ///     Send a vector of bytes to another party.
//     /// </summary>
//     /// <param name="id">The ID of the remote party.</param>
//     /// <param name="data">The bytes to send.</param>
//     public abstract void SendBytes(int id, List<byte> data);
//
//     /// <summary>
//     ///     Receive a vector of field elements from a remote party.
//     /// </summary>
//     /// <param name="id">The ID of the sender.</param>
//     /// <param name="n">The number of elements to receive.</param>
//     /// <returns>The received elements.</returns>
//     public abstract List<Field> Recv(int id, int n);
//
//     /// <summary>
//     ///     Receive a vector of shares from a remote party.
//     /// </summary>
//     /// <param name="id">The ID of the sender.</param>
//     /// <param name="n">The number of shares to receive.</param>
//     /// <returns>The received shares.</returns>
//     public abstract List<Shr> RecvShares(int id, int n);
//
//     /// <summary>
//     ///     Receive a vector of bytes from a remote party.
//     /// </summary>
//     /// <param name="id">The ID of the sender.</param>
//     /// <param name="n">The number of bytes to receive.</param>
//     /// <returns>The received bytes.</returns>
//     public abstract List<byte> RecvBytes(int id, int n);
//
//     public void Connect()
//     {
//         for (var i = 0; i < Size; ++i)
//         {
//             Logger.Info($"connect {mChannels[i].ToString()}");
//             mChannels[i].Open();
//         }
//     }
// }