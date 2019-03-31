using UnityEngine;

namespace AMARI.Assets.Scripts.ScriptableObjectFolder
{
    [CreateAssetMenu(
        fileName = "BlockStatus", 
        menuName = "ScriptableObject/BlockStatus", 
        order    = 0)
    ]
    public class BlockStatus : ScriptableObject
    {
        [SerializeField]
        private int[] blockRndNumber = (default);
        public int this[int blkIndex]
        {
            get => blockRndNumber[blkIndex];
            set => blockRndNumber[blkIndex] = value;
        }
    }
}