using UnityEngine;


namespace TestAlgorithm
{
    [System.Serializable]
    public sealed class EdgeData : SceneObjectData
    {
        #region Fields

        [SerializeField] private int _nodeStartIndex;
        [SerializeField] private int _nodeEndIndex;
        [SerializeField] private int _weight;

        #endregion


        #region Fields

        public int NodeStartIndex => _nodeStartIndex;
        public int NodeEndIndex => _nodeEndIndex;
        public int Weight => _weight;

        #endregion
    }
}

