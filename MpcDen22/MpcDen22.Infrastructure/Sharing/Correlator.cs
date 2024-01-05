// using MpcDen22.Infrastructure.CommonModels;
// using MpcDen22.Infrastructure.Sharing.Models;
//
namespace MpcDen22.Infrastructure.Sharing;
//
public class Correlstor
{
    
}
// public class Correlator
//     {
//         private frn.lib.secret_sharing.Replicator<Field> mReplicator;
//         private int mId;
//         private int mThreshold;
//         private int mSize;
//         private List<PRG> mOwnPRGs;
//         private List<List<PRG>> mRandPRGs;
//         private List<PRG> mZeroPRGs;
//
//         public Correlator(int id, frn.lib.secret_sharing.Replicator<Field> replicator)
//         {
//             mReplicator = replicator;
//             mId = id;
//             mThreshold = replicator.Threshold();
//             mSize = replicator.Size();
//             mOwnPRGs = new List<PRG>(mReplicator.AdditiveShareSize());
//             mRandPRGs = new List<List<PRG>>(2 * mThreshold + 1);
//             Init();
//         }
//
//         public AdditiveShare AdditiveShare()
//         {
//             // Implement the logic for AdditiveShare
//             // You may need to adapt the return type and implementation based on your specific logic.
//             return new AdditiveShare();
//         }
//
//         public AdditiveShare AdditiveShareDummy()
//         {
//             // Implement the logic for AdditiveShareDummy
//             // You may need to adapt the return type and implementation based on your specific logic.
//             return new AdditiveShare();
//         }
//
//         public RandomShare GenRandomShare()
//         {
//             // Implement the logic for GenRandomShare
//             // You may need to adapt the return type and implementation based on your specific logic.
//             return new RandomShare();
//         }
//
//         public RandomShare GenRandomShareDummy()
//         {
//             // Implement the logic for GenRandomShareDummy
//             // You may need to adapt the return type and implementation based on your specific logic.
//             return new RandomShare();
//         }
//
//         public void SetOwnPRGs(List<PRG> PRGs)
//         {
//             mOwnPRGs = PRGs;
//         }
//
//         public void SetRandPRGs(List<PRG> PRGs, int idx)
//         {
//             mRandPRGs[idx] = PRGs;
//         }
//
//         private void Init()
//         {
//             // Implement the initialization logic if needed
//         }
//     }