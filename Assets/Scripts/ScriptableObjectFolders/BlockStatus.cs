using UnityEngine;

namespace AMARI.Assets.Scripts
{
    [CreateAssetMenu(
        fileName = "BlockStatus", 
        menuName = "ScriptableObject/BlockStatus", 
        order    = 0)
    ]
    public class BlockStatus : ScriptableObject
    {
        [SerializeField]
        private int[] blockRndNumber;
        public int this[int blkNum] => blockRndNumber[blkNum];
    }
}