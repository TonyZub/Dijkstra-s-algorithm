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


        #region Constructor

        public EdgeData(Vector2 position, int nodeStartindex, int nodeEndIndex, int weight) : 
            base(position)
        {
            _nodeStartIndex = Mathf.Min(nodeStartindex, nodeEndIndex);
            _nodeEndIndex = Mathf.Max(nodeStartindex, nodeEndIndex);
            _weight = Mathf.Clamp(weight, 0, Data.ProgrammData.MaximalEdgeWeight);
        }

        #endregion


        #region Methods

        public override void OnValidate()
        {
            base.OnValidate();
            _weight = Mathf.Clamp(_weight, 0, Data.ProgrammData.MaximalEdgeWeight);
        }

        #endregion
    }
}

