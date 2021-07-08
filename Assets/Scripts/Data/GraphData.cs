using System.Collections.Generic;
using UnityEngine;


namespace TestAlgorithm
{
    [CreateAssetMenu(fileName = "GraphData", menuName = "CreateData/GraphData")]
    public sealed class GraphData : ScriptableObject
    {
        #region Constants

        public const string NODE_DATAS_FIELD_NAME = "_nodeDatas";
        public const string EDGE_DATAS_FIELD_NAME = "_edgeDatas";

        #endregion


        #region Fields

        [SerializeField] private List<NodeData> _nodeDatas;
        [SerializeField] private List<EdgeData> _edgeDatas;
        PhysicsService _physics = new PhysicsService();

        #endregion


        #region Properties

        public List<NodeData> NodeDatas => _nodeDatas;
        public List<EdgeData> EdgeDatas => _edgeDatas;

        #endregion


        #region Methods

        void OnValidate()
        {
            ValidateNodeInput();
            ValidateEdgeInput();
        }

        private void ValidateNodeInput()
        {
            ValidateNodeDatas(_nodeDatas);
        }

        private void ValidateEdgeInput()
        {
            ValidateEdgeDatas(_edgeDatas, _nodeDatas);
        }

        public void ValidateNodeDatas(List<NodeData> nodeDatas)
        {
            foreach (var node in nodeDatas)
            {
                node.OnValidate();
            }
        }

        public void ValidateEdgeDatas(List<EdgeData> edgeDatas, List<NodeData> nodeDatas)
        {
            int maximalNodeIndex = nodeDatas.Count - 1;
            int maximalEdgesCount = nodeDatas.Count * (nodeDatas.Count - 1) / 2;
            int edgesQuantity = edgeDatas.Count;
            int quantityDifference = edgesQuantity - maximalEdgesCount;
            while (quantityDifference > 0)
            {
                edgeDatas.Remove(edgeDatas[edgeDatas.Count - 1]);
                quantityDifference--;
            }

            for (int i = 0; i < edgeDatas.Count; i++)
            {
                int minimalIndex = Mathf.Min(edgeDatas[i].NodeStartIndex, edgeDatas[i].NodeEndIndex);
                int maximalIndex = Mathf.Max(edgeDatas[i].NodeStartIndex, edgeDatas[i].NodeEndIndex);
                minimalIndex = Mathf.Clamp(minimalIndex, 0, maximalNodeIndex);
                maximalIndex = Mathf.Clamp(maximalIndex, 0, maximalNodeIndex);
                edgeDatas[i] = new EdgeData(edgeDatas[i].Position, minimalIndex, maximalIndex, edgeDatas[i].Weight);
                edgeDatas[i].OnValidate();

                if (edgeDatas[i].NodeStartIndex == edgeDatas[i].NodeEndIndex || edgeDatas.FindAll(x => x.
                    NodeStartIndex == edgeDatas[i].NodeStartIndex && x.NodeEndIndex == edgeDatas[i].NodeEndIndex).Count > 1)
                {
                    if(GetNewEdgeNodesIndexes(nodeDatas, edgeDatas, out int[] newIndexes))
                    {
                        Vector2 newPosition = _physics.GetRandomEmptyPositionBetween(nodeDatas[newIndexes[0]].Position, 
                            nodeDatas[newIndexes[1]].Position, nodeDatas, edgeDatas);
                        edgeDatas[i] = new EdgeData(newPosition, newIndexes[0], newIndexes[1], edgeDatas[i].Weight);
                    }
                    else
                    {
                        edgeDatas.Remove(edgeDatas[i]);
                        i--;
                    }
                }
            }
        }

        public bool GetNewEdgeNodesIndexes(List<NodeData> nodeDatas, List<EdgeData> edgeDatas,
            out int[] newIndexes)
        {
            bool hasEmptyIndex = false;
            int[] indexes = new int[2];
            for (int i = 0; i < nodeDatas.Count; i++)
            {
                for (int j = 0; j < nodeDatas.Count; j++)
                {
                    if(i != j)
                    {
                        EdgeData foundEdge = edgeDatas.Find(x => (x.NodeStartIndex == i && x.NodeEndIndex == j) ||
                            (x.NodeStartIndex == j && x.NodeEndIndex == i));
                        if (foundEdge == null)
                        {
                            indexes[0] = Mathf.Min(i, j);
                            indexes[1] = Mathf.Max(i, j);
                            hasEmptyIndex = true;
                            break;
                        }
                    }
                }
            }
            newIndexes = indexes;
            return hasEmptyIndex;
        }

        public void SaveData(List<NodeData> nodeDatas, List<EdgeData> edgeDatas)
        {
            _nodeDatas = nodeDatas;
            _edgeDatas = edgeDatas;
        }

        #endregion
    }
}

