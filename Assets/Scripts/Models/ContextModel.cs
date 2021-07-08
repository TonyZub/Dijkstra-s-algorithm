using System.Collections.Generic;
using UnityEngine;
using System;


namespace TestAlgorithm
{
    public sealed class ContextModel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<NodeData> _nodeDatas;
        [SerializeField] private List<EdgeData> _edgeDatas;
        private InitializeController _initializeController;
        private GraphController _graphController;
        private bool _isInteractive;

        #endregion


        #region Properties

        public event Action OnNewNodeCreateFromInspector;
        public event Action<NodeModel> OnNodeDeleteFromInspector;
        public event Action<NodeModel, NodeModel> OnNewEdgeCreateFromInspector;
        public event Action<EdgeModel> OnEdgeModelDeleteFromInspector;

        public Transform GraphNodesPool { get; private set; }
        public Transform GraphEdgesPool { get; private set; }
        public Transform CanvasOverlay { get; private set; }
        public InputModel InputModel { get; private set; }
        public List<NodeModel> NodeModels { get; private set; }
        public List<EdgeModel> EdgeModels { get; private set; }
        public List<NodeData> NodeDatas => _nodeDatas;
        public List<EdgeData> EdgeDatas => _edgeDatas;
        public GameObject MainPanel { get; private set; }
        public GameObject NodePanel { get; private set; }
        public GameObject EdgePanel { get; private set; }

        #endregion


        #region UnityMethods

        private void OnValidate()
        {
            if (_isInteractive)
            {
                ValidateDatas();
                UpdateModels();
            }
        }

        #endregion


        #region Methods

        public void InitRootTransforms(Transform graphNodesPool, Transform graphEdgesPool, Transform canvasOverlay)
        {
            GraphNodesPool = graphNodesPool;
            GraphEdgesPool = graphEdgesPool;
            CanvasOverlay = canvasOverlay;
        }

        public void InitInputModel(InputModel inputModel)
        {
            InputModel = inputModel;
        }

        public void InitCollections(List<NodeData> nodeDatas, List<EdgeData> edgeDatas, 
            List<NodeModel> nodeModels, List<EdgeModel> edgeModels)
        {
            _nodeDatas = nodeDatas;
            _edgeDatas = edgeDatas;
            NodeModels = nodeModels;
            EdgeModels = edgeModels;
            _isInteractive = true;
        }

        public void InitCanvasPanels(GameObject mainPanel, GameObject nodePanel, GameObject edgePanel)
        {
            MainPanel = mainPanel;
            NodePanel = nodePanel;
            EdgePanel = edgePanel;
        }

        public SceneObjectModel GetSceneObjectByInstanceId(int id)
        {
            SceneObjectModel neededObject = NodeModels.Find(x => x.ObjectId == id);
            if (neededObject == null)
            {
                neededObject = EdgeModels.Find(x => x.ObjectId == id);
            }
            return neededObject;
        }

        public void SignUpForModelsUpdateEvent()
        {
            _initializeController = GetComponent<InitializeController>();
            _initializeController.OnModelsUpdate += () => UpdateDatas();
            _graphController = GetComponent<GraphController>();
            _graphController.OnModelsUpdate += () => UpdateDatas();
        }

        public void SignOutForModelsUpdateEvent()
        {
            _initializeController.OnModelsUpdate -= () => UpdateDatas();
            _graphController.OnModelsUpdate -= () => UpdateDatas();
        }

        private void UpdateDatas()
        {
            _nodeDatas = new List<NodeData>();
            _edgeDatas = new List<EdgeData>();

            int edgeQuantityDifference = EdgeDatas.Count - NodeDatas.Count;
            for(int i = 0; i < NodeModels.Count; i++)
            {
                _nodeDatas.Add(new NodeData(NodeModels[i].ObjectTransform.position));
            }
            for (int i = 0; i < EdgeModels.Count; i++)
            {
                _edgeDatas.Add(new EdgeData(EdgeModels[i].ObjectTransform.position, 
                    EdgeModels[i].StartNodeModelIndex, EdgeModels[i].EndNodeModelIndex, EdgeModels[i].Weight));
            }
        }

        private void ValidateDatas()
        {
            int maximalNodeIndex = _nodeDatas.Count - 1;
            Data.GraphData.ValidateNodeDatas(_nodeDatas);
            Data.GraphData.ValidateEdgeDatas(_edgeDatas, _nodeDatas);
        }

        private void UpdateModels()
        {
            int nodeQuantityDifference = NodeModels.Count - NodeDatas.Count;
            bool hasNodeDifference = nodeQuantityDifference != 0;
            if(nodeQuantityDifference > 0)
            {
                if(NodeModels.Count == 1)
                {
                    OnNodeDeleteFromInspector?.Invoke(NodeModels[0]);
                }
                else
                {
                    for (int i = 0; i < NodeModels.Count; i++)
                    {
                        if (NodeDatas.Find(x => x.Position == (Vector2)(NodeModels[i].ObjectTransform.position)) != null)
                        {
                            OnNodeDeleteFromInspector?.Invoke(NodeModels[i]);
                            break;
                        }
                    }
                }       
            }
            else if(nodeQuantityDifference < 0)
            {
                OnNewNodeCreateFromInspector?.Invoke();
            }

            if (!hasNodeDifference)
            {
                List<NodeModel> newNodeModels = new List<NodeModel>();
                for (int i = 0; i < _nodeDatas.Count; i++)
                {
                    newNodeModels.Add(new NodeModel(NodeModels[i].ObjectOnScene, NodeDatas[i].Position, i));
                }
                NodeModels = newNodeModels;
            }

            int edgeQuantityDifference = EdgeModels.Count - EdgeDatas.Count;
            bool hasEdgeDifference = edgeQuantityDifference > 0;
            if (edgeQuantityDifference > 0)
            {
                for (int i = 0; i < EdgeModels.Count; i++)
                {
                    if (EdgeDatas.Find(x => x.Position == (Vector2)(EdgeModels[i].ObjectTransform.position)) == null)
                    {
                        OnEdgeModelDeleteFromInspector?.Invoke(EdgeModels[i]);
                        break;
                    }
                }
            }
            else if (edgeQuantityDifference < 0)
            {
                for (int i = 0; i < EdgeDatas.Count; i++)
                {
                    if (EdgeModels.Find(x => (Vector2)(x.ObjectTransform.position) == EdgeDatas[i].Position) == null)
                    {
                        NodeModel startNode = NodeModels[EdgeDatas[i].NodeStartIndex];
                        NodeModel endNode = NodeModels[EdgeDatas[i].NodeEndIndex];
                        OnNewEdgeCreateFromInspector?.Invoke(startNode, endNode);
                        break;
                    }
                }               
            }

            if (!hasEdgeDifference)
            {
                List<EdgeModel> newEdgeModels = new List<EdgeModel>();
                for (int i = 0; i < EdgeDatas.Count; i++)
                {
                    newEdgeModels.Add(new EdgeModel(EdgeModels[i].ObjectOnScene, EdgeDatas[i].Position,
                        NodeModels[EdgeDatas[i].NodeStartIndex], NodeModels[EdgeDatas[i].NodeEndIndex],
                            EdgeDatas[i].Weight));
                }
                EdgeModels = newEdgeModels;
            }
        }

        #endregion
    }
}

