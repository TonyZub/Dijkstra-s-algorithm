using UnityEngine;
using System.Collections.Generic;


namespace TestAlgorithm
{
    public sealed class InitializeController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _graphNodesPool;
        [SerializeField] private Transform _graphEdgesPool;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            CreateSceneObjects();
        }

        #endregion


        #region Methods

        private void CreateSceneObjects()
        {
            List<NodeModel> nodes = CreateNodes();
            List<EdgeModel> edges = CreateEdges(nodes);
            ContextModel context = gameObject.AddComponent<ContextModel>();
            context.Init(Data.GraphData.NodeDatas, Data.GraphData.EdgeDatas, nodes, edges);
        }

        private List<NodeModel> CreateNodes()
        {
            List<NodeModel> nodeModels = new List<NodeModel>();
            int maximaNodelIndex = Data.GraphData.NodeDatas.Count;
            for (int i = 0; i < maximaNodelIndex; i++)
            {
                GameObject newSceneObject = Instantiate(Data.ProgrammData.NodePrefab, _graphNodesPool);
                newSceneObject.name = i.ToString();
                Vector2 startPosition = ValidateStartPosition(Data.GraphData.NodeDatas[i].Position);
                nodeModels.Add(new NodeModel(newSceneObject, startPosition, i));
            }
            return nodeModels;
        }

        private List<EdgeModel> CreateEdges(List<NodeModel> nodeModels)
        {
            List<EdgeModel> edgeModels = new List<EdgeModel>();
            int maximaNodelIndex = Data.GraphData.NodeDatas.Count;
            int maximalEdgeIndex = Data.GraphData.EdgeDatas.Count;
            for (int i = 0; i < maximalEdgeIndex; i++)
            {
                int nodeStartIndex = Data.GraphData.EdgeDatas[i].NodeStartIndex;
                int nodeEndIndex = Data.GraphData.EdgeDatas[i].NodeEndIndex;
                if(nodeStartIndex != nodeEndIndex)
                {
                    if(ValidateNodeIndex(nodeStartIndex, maximaNodelIndex) && ValidateNodeIndex(nodeEndIndex, maximaNodelIndex))
                    {
                        GameObject newSceneObject = Instantiate(Data.ProgrammData.EdgePrefab, _graphEdgesPool);
                        newSceneObject.name = i.ToString();
                        Vector2 startPosition = ValidateStartPosition(Data.GraphData.EdgeDatas[i].Position);
                        NodeModel startNodeModel = nodeModels[nodeStartIndex];
                        NodeModel endNodeModel = nodeModels[nodeEndIndex];
                        edgeModels.Add(new EdgeModel(newSceneObject, Data.GraphData.EdgeDatas[i].Position,
                            startNodeModel, endNodeModel, Data.GraphData.EdgeDatas[i].Weight));
                    }
                    else
                    {
                        Debug.LogWarning($"Graph edge {i} has wrong start or end node indexes, not initialized");
                    }
                }
                else
                {
                    Debug.LogWarning($"Graph edge {i} has same start and end node indexes, not initialized");
                }
            }
            return edgeModels;
        }

        private Vector2 ValidateStartPosition(Vector2 startPosition)
        {
            startPosition.x = startPosition.x > Data.ProgrammData.ActiveScreenSize.x / 2 ?
                   Data.ProgrammData.ActiveScreenSize.x / 2 : startPosition.x;
            startPosition.y = startPosition.y > Data.ProgrammData.ActiveScreenSize.y / 2 ?
                Data.ProgrammData.ActiveScreenSize.y / 2 : startPosition.y;
            return startPosition;
        }

        private bool ValidateNodeIndex(int nodeIndex, int maximalNodeindex)
        {
            return nodeIndex >= 0 && nodeIndex < maximalNodeindex;
        }

        #endregion
    }
}

