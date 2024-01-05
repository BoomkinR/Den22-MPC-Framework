// namespace MpcDen22.Infrastructure.Sharing;
//
// public class ShrManipulator
// {
//     private int mPartyId;
//     private int mParties;
//     private int mThreshold;
//
//     // Добавьте необходимые структуры данных и методы
//
//     public Shr Add(Shr a, Shr b)
//     {
//         Shr r = new Shr();
//         for (int i = 0; i < a.Count; i++)
//         {
//             r.Add(a[i] + b[i]);
//         }
//         return r;
//     }
//
//     public Shr AddConstant(Shr a, Field c)
//     {
//         if (mIndexForConstantOps == -1) return a;
//         Shr r = new Shr(a);
//         r[mIndexForConstantOps] += c;
//         return r;
//     }
//
//     public Shr Subtract(Shr a, Shr b)
//     {
//         Shr r = new Shr();
//         for (int i = 0; i < a.Count; i++)
//         {
//             r.Add(a[i] - b[i]);
//         }
//         return r;
//     }
//
//     public Shr SubtractConstant(Shr a, Field c)
//     {
//         if (mIndexForConstantOps == -1) return a;
//         Shr r = new Shr(a);
//         r[mIndexForConstantOps] -= c;
//         return r;
//     }
//
//     public Shr SubtractConstant(Field c, Shr a)
//     {
//         Shr r = new Shr();
//         for (int i = 0; i < a.Count; i++)
//         {
//             r.Add(-a[i]);
//         }
//         if (mIndexForConstantOps != -1) r[mIndexForConstantOps] += c;
//         return r;
//     }
//
//     public Shr MultiplyConstant(Shr a, Field c)
//     {
//         Shr r = new Shr();
//         for (int i = 0; i < a.Count; i++)
//         {
//             r.Add(c * a[i]);
//         }
//         return r;
//     }
//
//     public ShrD MultiplyToDoubleDegree(Shr a, Shr b)
//     {
//         List<Field> c = Enumerable.Repeat(new Field(0), mDoubleReplicator.ShareSize()).ToList();
//
//         foreach (var triple in mTableMult)
//         {
//             int i = triple.src_a;
//             int j = triple.src_b;
//             int idxInDoubleRepShare = triple.dest_c;
//             c[idxInDoubleRepShare] += a[i] * b[j];
//         }
//         return c;
//     }
//
//     public Field MultiplyToAdditive(Shr a, Shr b)
//     {
//         Field c = new Field(0);
//
//         foreach (var tuple in mTableMult)
//         {
//             int i = tuple.src_a;
//             int j = tuple.src_b;
//             if (mPartyId == tuple.first_party) c += a[i] * b[j];
//         }
//         return c;
//     }
//
//     // Добавьте необходимые методы и структуры данных
//
//     private const int INDEX_SHARE_FOR_CNST = 0;
//
//     private int IndexForConstantOperations()
//     {
//         var id = mPartyId;
//         var set = mReplicator.IndexSetFor(id);
//
//         var index = set.IndexOf(INDEX_SHARE_FOR_CNST);
//
//         return index != -1 ? index : -1;
//     }
//
//     private void Init()
//     {
//         // Добавьте необходимую инициализацию
//     }
//
//     private int ComputeIndexForDoubleMultiplication(int a, int b)
//     {
//         int a_ = mReplicator.IndexSetFor(mPartyId)[a];
//         int b_ = mReplicator.IndexSetFor(mPartyId)[b];
//
//         var setA = mReplicator.Combination(a_);
//         var setB = mReplicator.Combination(b_);
//
//         var intersection = setA.Intersect(setB).Take(mParties - 2 * mThreshold).ToList();
//
//         int targetSet = mDoubleReplicator.RevComb(intersection);
//
//         var indexSet = mDoubleReplicator.IndexSetFor(mPartyId);
//         int idx = indexSet.IndexOf(targetSet);
//
//         return idx != -1 ? idx : -1;
//     }
// }