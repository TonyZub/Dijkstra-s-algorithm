using System.Collections.Generic;
using UnityEngine;


namespace TestAlgorithm
{
    [CreateAssetMenu(fileName = "GraphData", menuName = "CreateData/GraphData")]
    public sealed class GraphData : ScriptableObject
    {
        #region Fields

        [SerializeField] private List<NodeData> _nodeDatas;
        [SerializeField] private List<EdgeData> _edgeDatas;

        #endregion


        #region Properties

        public List<NodeData> NodeDatas => _nodeDatas;
        public List<EdgeData> EdgeDatas => _edgeDatas;

        #endregion
    }
}

