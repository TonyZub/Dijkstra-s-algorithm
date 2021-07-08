using UnityEngine;
using System.Collections.Generic;
using System;


namespace TestAlgorithm
{
    public sealed class InitializeController : MonoBehaviour
    {
        #region Fields

        public event Action OnModelsUpdate;

        [SerializeField] private Transform _graphNodesPool;
        [SerializeField] private Transform _graphEdgesPool;
        [SerializeField] private Transform _canvasOverlay;

        private ContextModel _contextModel;
        private CanvasController _canvasController;
        private GraphController _graphController;
        private PhysicsService _physics;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            CreateContext();
            CreateServices();
            CreateSceneObjects();
        }

        private void Start()
        {
            SighUpGraphControllerEvents();
            SighUpContextModelEvents();
        }

        private void OnApplicationQuit()
        {
            SighOutGraphControllerEvents();
            SighOutContextModelEvents();
            SaveChangedData();
        }

        #endregion


        #region Methods

        private void CreateContext()
        {
            _contextModel = gameObject.AddComponent<ContextModel>();
            _contextModel.InitRootTransforms(_graphNodesPool, _graphEdgesPool, _canvasOverlay);
        }

        private void CreateServices()
        {
            Services.SharedInstance.InitializeServices(_contextModel);
            _physics = Services.SharedInstance.PhysicsService;
        }

        private void CreateSceneObjects()
        {
            CreateNodesAndEdges();
            CreateCanvasPanels();
        }

        private void CreateNodesAndEdges()
        {
            List<NodeModel> nodes = CreateNodes();
            List<EdgeModel> edges = CreateEdges(nodes);
            _contextModel.InitCollections(Data.GraphData.NodeDatas,
                Data.GraphData.EdgeDatas, nodes, edges);
        }

        private void CreateCanvasPanels()
        {
            GameObject mainPanel = CreateCanvasPanel(Data.ProgrammData.MainPanelPrefab);
            GameObject nodePanel = CreateCanvasPanel(Data.ProgrammData.NodePanelPrefab);
            GameObject edgePanel = CreateCanvasPanel(Data.ProgrammData.EdgePanelPrefab);
            _contextModel.InitCanvasPanels(mainPanel, nodePanel, edgePanel);
        }

        private GameObject CreateCanvasPanel(GameObject prefab)
        {
            GameObject newPanel = Instantiate(prefab, _canvasOverlay);
            newPanel.name = prefab.name;
            return newPanel;
        }

        private void SighUpGraphControllerEvents()
        {
            _graphController = GetComponent<GraphController>();
            _graphController.OnAddNode += () => CreateNewNode();
            _graphController.OnCreateEdge += (startNode, endNode) => CreateNewEdge(startNode, endNode);
            _graphController.OnDeleteNode += (nodeModel) => DeleteNode(nodeModel);
            _graphController.OnDeleteEdge += (edgeModel) => DeleteEdge(edgeModel);
            _contextModel.SignUpForModelsUpdateEvent();
        }

        private void SighOutGraphControllerEvents()
        {
            _graphController.OnAddNode -= () => CreateNewNode();
            _graphController.OnCreateEdge -= (startNode, endNode) => CreateNewEdge(startNode, endNode);
            _graphController.OnDeleteNode -= (nodeModel) => DeleteNode(nodeModel);
            _graphController.OnDeleteEdge -= (edgeModel) => DeleteEdge(edgeModel);
            _contextModel.SignOutForModelsUpdateEvent();
        }

        private void SighUpContextModelEvents()
        {
            _contextModel.OnNewNodeCreateFromInspector += () => CreateNewNode();
            _contextModel.OnNodeDeleteFromInspector += (nodeModel) => DeleteNode(nodeModel);
            _contextModel.OnNewEdgeCreateFromInspector += (firstNode, secondNode) => CreateNewEdge(firstNode, secondNode);
            _contextModel.OnEdgeModelDeleteFromInspector += (edgeModel) => DeleteEdge(edgeModel);
        }

        private void SighOutContextModelEvents()
        {
            _contextModel.OnNewNodeCreateFromInspector -= () => CreateNewNode();
            _contextModel.OnNodeDeleteFromInspector -= (nodeModel) => DeleteNode(nodeModel);
            _contextModel.OnNewEdgeCreateFromInspector -= (firstNode, secondNode) => CreateNewEdge(firstNode, secondNode);
            _contextModel.OnEdgeModelDeleteFromInspector -= (edgeModel) => DeleteEdge(edgeModel);
        }

        private List<NodeModel> CreateNodes()
        {
            List<NodeModel> nodeModels = new List<NodeModel>();
            int maximaNodelIndex = Data.GraphData.NodeDatas.Count;
            for (int i = 0; i < maximaNodelIndex; i++)
            {
                GameObject newSceneObject = Instantiate(Data.ProgrammData.NodePrefab, _graphNodesPool);
                newSceneObject.name = i.ToString();
                Vector2 startPosition = _physics.ValidateStartPosition(Data.GraphData.NodeDatas[i].Position);
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
                if (nodeStartIndex != nodeEndIndex)
                {
                    if (_physics.ValidateNodeIndex(nodeStartIndex, maximaNodelIndex) &&
                        _physics.ValidateNodeIndex(nodeEndIndex, maximaNodelIndex))
                    {
                        GameObject newSceneObject = Instantiate(Data.ProgrammData.EdgePrefab, _graphEdgesPool);
                        newSceneObject.name = i.ToString();
                        Vector2 startPosition = _physics.ValidateStartPosition(Data.GraphData.EdgeDatas[i].Position);
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

        private void CreateNewNode()
        {
            Vector2 startPosition = _physics.GetRandomEmptyPosition();
            GameObject newSceneObject = Instantiate(Data.ProgrammData.NodePrefab, startPosition,
                Quaternion.identity, _graphNodesPool);
            int index = _contextModel.NodeModels.Count;
            newSceneObject.name = index.ToString();
            _contextModel.NodeModels.Add(new NodeModel(newSceneObject, startPosition, index));
            OnModelsUpdate?.Invoke();
        }

        private void DeleteNode(NodeModel nodeModel)
        {
            List<EdgeModel> connectedEdges = _contextModel.EdgeModels.FindAll(x => x.
                StartNodeModelIndex == nodeModel.Index || x.EndNodeModelIndex == nodeModel.Index);
            foreach (var edge in connectedEdges)
            {
                edge.DestroyObjectOnScene();
                _contextModel.EdgeModels.Remove(edge);
            }
            nodeModel.DestroyObjectOnScene();
            _contextModel.NodeModels.Remove(nodeModel);
            RenumerateNodes();
            OnModelsUpdate?.Invoke();
        }

        private void CreateNewEdge(NodeModel startNodeModel, NodeModel endNodeModel)
        {
            Vector2 startPosition = _physics.GetRandomEmptyPositionBetween(startNodeModel.
                ObjectTransform.position, endNodeModel.ObjectTransform.position);
            GameObject newSceneObject = Instantiate(Data.ProgrammData.EdgePrefab, startPosition,
                Quaternion.identity, _graphEdgesPool);
            int index = _contextModel.EdgeModels.Count;
            newSceneObject.name = index.ToString();
            _contextModel.EdgeModels.Add(new EdgeModel(newSceneObject, startPosition,
                startNodeModel, endNodeModel, 0));
            OnModelsUpdate?.Invoke();
        }

        private void DeleteEdge(EdgeModel edgeModel)
        {
            edgeModel.DestroyObjectOnScene();
            _contextModel.EdgeModels.Remove(edgeModel);
            RenameEdges();
            OnModelsUpdate?.Invoke();
        }

        private void RenumerateNodes()
        {
            for(int i = 0; i < _contextModel.NodeModels.Count; i++)
            {
                _contextModel.NodeModels[i].Index = i;
            }
        }

        private void RenameEdges()
        {
            for (int i = 0; i < _contextModel.EdgeModels.Count; i++)
            {
                _contextModel.EdgeModels[i].UpdateName(i);
            }
        }

        private void SaveChangedData()
        {
            if (Data.ProgrammData.SaveProgressAfterPlayMode)
            {
                Data.GraphData.SaveData(_contextModel.NodeDatas, _contextModel.EdgeDatas);
            }
        }

        #endregion
    }
}

