namespace TestAlgorithm
{
    public struct ShortNodeData
    {
        #region Properties

        public int Index { get; }
        public int FromNodeIndex { get; }
        public int Weight { get; }

        #endregion


        #region Constructor

        public ShortNodeData(int index, int fromNodeIndex, int weight)
        {
            Index = index;
            FromNodeIndex = fromNodeIndex;
            Weight = weight;
        }

        #endregion
    }
}

