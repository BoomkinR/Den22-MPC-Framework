// using Frn;
// using MpcDen22.Infrastructure.Net;
// using MpcDen22.Infrastructure.Sharing;
// using MpcDen22.Infrastructure.Sharing.Models;
//
// namespace MpcDen22.Infrastructure;
//
// public struct AddAndMsgs
// {
//     public Field AddShare { get; set; }
//     public List<Shr> Msgs { get; set; }
// }
//
// public class CheckData
// {
//     public List<Field> SharesSentToP1 { get; set; }
//     public List<List<Field>> SharesRecvByP1 { get; set; }
//     public List<Field> ValuesRecvFromP1 { get; set; }
//     public List<List<Shr>> Msgs { get; set; }
//     public int Counter { get; set; }
//
//     public CheckData(int threshold)
//     {
//         SharesRecvByP1 = new List<List<Field>>(2 * threshold + 1);
//         Counter = 0;
//     }
// }
//
// public class Mult
// {
//     private readonly Network _network;
//     private readonly Replicator<Field> _mReplicator;
//     private readonly int _mId;
//     private readonly int _mThreshold;
//     private readonly int _mSize;
//     private readonly ShrManipulator _mManipulator;
//     private readonly InputSetup.Correlator _mCorrelator;
//     private readonly CheckData _mCheckData;
//     private int _mCount;
//     private readonly List<RandomShare> _mRandomShares;
//     private readonly List<Field> _mSharesToSendP1;
//     private readonly List<List<Field>> _mSharesRecvByP1;
//     private List<Field> _mValuesRecvFromP1;
//     private List<Field> _mValuesSentFromP1;
//
//     public Mult(Network network, Replicator<Field> replicator,
//         ShrManipulator manipulator, InputSetup.Correlator correlator, CheckData checkData)
//     {
//         _network = network ?? throw new ArgumentNullException(nameof(network));
//         _mReplicator = replicator ?? throw new ArgumentNullException(nameof(replicator));
//         // _mId = network.Id();
//         // _mThreshold = replicator.Threshold();
//         // _mSize = network.Size();
//         _mManipulator = manipulator ?? throw new ArgumentNullException(nameof(manipulator));
//         _mCorrelator = correlator ?? throw new ArgumentNullException(nameof(correlator));
//         _mCheckData = checkData ?? throw new ArgumentNullException(nameof(checkData));
//         _mSharesRecvByP1 = new List<List<Field>>(2 * _mThreshold + 1);
//         _mRandomShares = new List<RandomShare>();
//         _mSharesToSendP1 = new List<Field>();
//     }
//
//     public void Prepare(Shr sharesX, Shr sharesY)
//     {
//         var randomShares = _mCorrelator.GenRandomShare();
//         _mRandomShares.Add(randomShares);
//         var output = MultiplyToAddAndMsgs(sharesX, sharesY, randomShares);
//
//         _mSharesToSendP1.Add(output.AddShare);
//
//         _mCheckData.SharesSentToP1.Add(output.AddShare);
//         _mCheckData.Msgs.Add(output.Msgs);
//
//         _mCount++;
//     }
//
//     public void Prepare(List<Shr> xs, List<Shr> ys)
//     {
//         for (int i = 0; i < xs.Count; i++)
//         {
//             Prepare(xs[i], ys[i]);
//         }
//     }
//
//     public List<Shr> Run()
//     {
//         _mCheckData.Counter += _mCount;
//         SendStep();
//         if (_mId == 0)
//         {
//             ReconstructionStep();
//         }
//         return OutputStep();
//     }
//
//     public void SendStep()
//     {
//         var timerSend = new Timer("SendStep_send");
//         timerSend.Start();
//         // Send shares to P1
//         if (_mId < 2 * _mThreshold + 1)
//         {
//             _network.Send(0, _mSharesToSendP1);
//         }
//         timerSend.Stop();
//
//         var timerReceive = new Timer("SendStep_receive");
//         timerReceive.Start();
//         // P1 receives the shares
//         if (_mId == 0)
//         {
//             for (int i = 0; i < 2 * _mThreshold + 1; i++)
//             {
//                 _mSharesRecvByP1[i] = _network.Recv(i, _mCount);
//
//                 _mCheckData.SharesRecvByP1[i].AddRange(_mSharesRecvByP1[i]);
//             }
//         }
//         timerReceive.Stop();
//     }
//
//     public void ReconstructionStep()
//     {
//         var timerReconstruction = new Timer("ReconstructionStep");
//         timerReconstruction.Start();
//         // P1 reconstructs the xy-r's
//         _mValuesSentFromP1 = new List<Field>(_mCount);
//         for (int multId = 0; multId < _mCount; multId++)
//         {
//             _mValuesSentFromP1[multId] = new Field(0);
//             for (int partyId = 0; partyId < 2 * _mThreshold + 1; partyId++)
//             {
//                 _mValuesSentFromP1[multId] += _mSharesRecvByP1[partyId][multId];
//             }
//         }
//
//         // P1 sends the reconstructions to parties in T=1...n-d
//         for (int partyId = 0; partyId < _mSize - _mThreshold; partyId++)
//         {
//             _network.Send(partyId, _mValuesSentFromP1);
//         }
//         timerReconstruction.Stop();
//     }
//
//     public List<Shr> OutputStep()
//     {
//         var timerReceive = new Timer("OutputStep_receive");
//         timerReceive.Start();
//         // Parties in T receive the message from P1
//         if (_mId < _mSize - _mThreshold)
//         {
//             _mValuesRecvFromP1 = _network.Recv(0, _mCount);
//
//             _mCheckData.ValuesRecvFromP1.AddRange(_mValuesRecvFromP1);
//         }
//         else
//         {
//             // Other parties can pretend they received 0 from P1
//             // This doesn't matter as they don't do anything when adding the constant
//             _mValuesRecvFromP1 = new List<Field>(_mCount);
//             for (int i = 0; i < _mCount; i++)
//             {
//                 _mValuesRecvFromP1.Add(new Field(0));
//             }
//         }
//         timerReceive.Stop();
//
//         var timerAddConstant = new Timer("OutputStep_add_constant");
//         timerAddConstant.Start();
//         // All parties compute the resulting shares
//         var output = new List<Shr>(_mCount);
//         for (int multId = 0; multId < _mCount; multId++)
//         {
//             output[multId] = _mManipulator.AddConstant(_mRandomShares[multId].RepShare, _mValuesRecvFromP1[multId]);
//         }
//         timerAddConstant.Stop();
//         return output;
//     }
//
//     private AddAndMsgs MultiplyToAddAndMsgs(Shr a, Shr b, RandomShare randomShares)
//     {
//         _mRandomShares.Add(randomShares);
//
//         // Initialize output
//         AddAndMsgs output;
//         output.AddShare = new Field(0);
//         output.Msgs = new List<Shr>(2 * _mThreshold + 1);
//         for (int i = 0; i < 2 * _mThreshold + 1; i++)
//         {
//             output.Msgs.Add(new Shr(_mManipulator.GetDoubleReplicator().ShareSize(), new Field(0)));
//         }
//
//         Field prod;
//
//         foreach (var tuple in _mManipulator.GetTableMult())
//         {
//             int srcA = tuple.SrcA;
//             int srcB = tuple.SrcB;
//             int dest = tuple.DestC;
//             int firstParty = tuple.FirstParty;
//
//             prod = a[srcA] * b[srcB];
//             output.Msgs[firstParty][dest] += prod;
//             if (_mId == firstParty)
//             {
//                 output.AddShare += prod;
//             }
//         }
//
//         // Subtract random keys
//         output.AddShare -= randomShares.AddShare;
//
//         // TODO subtract random key from Msgs
//         return output;
//     }
// }