using System.Collections.Generic;
using UnityEngine;


namespace TestAlgorithm
{
    public sealed class ContextModel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<NodeData> _nodeDatas;
        [SerializeField] private List<EdgeData> _edgeDatas;

        #endregion


        #region Properties

        public List<NodeModel> NodeModels { get; private set; }
        public List<EdgeModel> EdgeModels { get; private set; }

        #endregion


        #region Methods

        public void Init(List<NodeData> nodeDatas, List<EdgeData> edgeDatas, List<NodeModel> nodeModels, List<EdgeModel> edgeModels)
        {
            _nodeDatas = nodeDatas;
            _edgeDatas = edgeDatas;
            NodeModels = nodeModels;
            EdgeModels = edgeModels;
        }

        #endregion
    }
}

