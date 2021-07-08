using UnityEngine;
using System;
using System.Collections.Generic;


namespace TestAlgorithm
{
    public sealed class GraphController : MonoBehaviour
    {
        #region Fields

        public event Action<GraphControllerStates> OnCurrentGraphControllerStateChanged;
        public event Action OnAddNode;
        public event Action<NodeModel> OnDeleteNode;
        public event Action<NodeModel, NodeModel> OnCreateEdge;
        public event Action<EdgeModel> OnDeleteEdge;
        public event Action OnModelsUpdate;

        private GraphControllerStates _currentState;
        private ContextModel _contextModel;
        private CanvasController _canvasController;
        private PhysicsService _physicsService;
        private MainCanvasService _canvasService;
        private DijkstrasAlgorithmService _algorythmService;
        private SceneObjectModel _chosenObjectModel;
        private List<EdgeModel> _edgesConnectedToChosenNode;
        private List<EdgeModel> _edgesInFoundPath;

        private int _chosenObjectId;
        private bool _isMovingObject;

        #endregion


        #region Properties

        private GraphControllerStates CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (value != _currentState)
                {
                    PreviousState = _currentState;
                    _currentState = value;
                    OnCurrentGraphControllerStateChanged?.Invoke(CurrentState);
                }
            }
        }
        private GraphControllerStates PreviousState { get; set; }

        #endregion


        #region UnityMethods

        private void Start()
        {
            _contextModel = GetComponent<ContextModel>();
            _canvasController = GetComponent<CanvasController>();
            SignUpInputEvents();
            SighUpCanvasEvents();
            SignUpContextEvents();
            _physicsService = Services.SharedInstance.PhysicsService;
            _canvasService = Services.SharedInstance.MainCanvasService;
            _algorythmService = Services.SharedInstance.DijkstrasAlgorithmService;
            _edgesConnectedToChosenNode = new List<EdgeModel>();
            _edgesInFoundPath = new List<EdgeModel>();
            CurrentState = GraphControllerStates.SelectionMode;
        }

        private void Update()
        {
            MoveChosenObject();
        }

        private void OnApplicationQuit()
        {
            SignOutInputEvents();
            SignOutCanvasEvents();
            SignOutContextEvents();
        }

        #endregion


        #region Methods

        private void SignUpInputEvents()
        {
            _contextModel.InputModel.OnMousePressed += (isPressed) => OnMousePressHandler(isPressed);
        }

        private void SignOutInputEvents()
        {
            _contextModel.InputModel.OnMousePressed -= (isPressed) => OnMousePressHandler(isPressed);
        }

        private void SignUpContextEvents()
        {
            _contextModel.OnNewNodeCreateFromInspector += () => DeletePath();
            _contextModel.OnNodeDeleteFromInspector += (nodeModel) => DeletePath();
            _contextModel.OnNewEdgeCreateFromInspector += (firstNode, secondNode) => DeletePath();
            _contextModel.OnEdgeModelDeleteFromInspector += (edgeModel) => DeletePath();
        }

        private void SignOutContextEvents()
        {
            _contextModel.OnNewNodeCreateFromInspector -= () => DeletePath();
            _contextModel.OnNodeDeleteFromInspector -= (nodeModel) => DeletePath();
            _contextModel.OnNewEdgeCreateFromInspector -= (firstNode, secondNode) => DeletePath();
            _contextModel.OnEdgeModelDeleteFromInspector -= (edgeModel) => DeletePath();
        }

        private void SighUpCanvasEvents()
        {
            _canvasController.OnPressCreateNewNode += () => OnPressAddNodeHandler();
            _canvasController.OnPressDeletePath += () => OnPressDeletePathHandler();
            _canvasController.OnPressQuit += () => OnPressQuitHandler();
            _canvasController.OnPressCreateNewEdge += () => OnPressCreateEdgeHandler();
            _canvasController.OnPressFindPath += () => OnPressFindPathHandler();
            _canvasController.OnPressDeleteNode += () => OnPressDeleteNodeHandler();
            _canvasController.OnPressIncreaceWeight += () => OnPressIncreaceWeightHandler();
            _canvasController.OnPressDecreaseWeight += () => OnPressDecreaseWeightHandler();
            _canvasController.OnPressDeleteEdge += () => OnPressDeleteEdgeHandler();
        }

        private void SignOutCanvasEvents()
        {
            _canvasController.OnPressCreateNewNode -= () => OnPressAddNodeHandler();
            _canvasController.OnPressDeletePath -= () => OnPressDeletePathHandler();
            _canvasController.OnPressQuit -= () => OnPressQuitHandler();
            _canvasController.OnPressCreateNewEdge -= () => OnPressCreateEdgeHandler();
            _canvasController.OnPressFindPath -= () => OnPressFindPathHandler();
            _canvasController.OnPressDeleteNode -= () => OnPressDeleteNodeHandler();
            _canvasController.OnPressIncreaceWeight -= () => OnPressIncreaceWeightHandler();
            _canvasController.OnPressDecreaseWeight -= () => OnPressDecreaseWeightHandler();
            _canvasController.OnPressDeleteEdge -= () => OnPressDeleteEdgeHandler();
        }

        private void OnPressAddNodeHandler()
        {
            OnAddNode?.Invoke();
        }

        private void OnPressDeletePathHandler()
        {
            DeletePath();
        }

        private void OnPressQuitHandler()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void OnPressFindPathHandler()
        {
            CurrentState = GraphControllerStates.PathFinding;
        }

        private void OnPressDeleteNodeHandler()
        {
            DeletePath();
            NodeModel chosenNode = _chosenObjectModel as NodeModel;
            OnDeleteNode?.Invoke(chosenNode);
            Unselect();
        }

        private void OnPressCreateEdgeHandler()
        {
            DeletePath();
            CurrentState = GraphControllerStates.SecondNodeForNewEdgeChoosing;
        }

        private void OnPressIncreaceWeightHandler()
        {
            DeletePath();
            EdgeModel chosenEdge = _chosenObjectModel as EdgeModel;
            chosenEdge.ChangeWeight(chosenEdge.Weight + 1);
        }

        private void OnPressDecreaseWeightHandler()
        {
            DeletePath();
            EdgeModel chosenEdge = _chosenObjectModel as EdgeModel;
            chosenEdge.ChangeWeight(chosenEdge.Weight - 1);
        }

        private void OnPressDeleteEdgeHandler()
        {
            DeletePath();
            EdgeModel chosenEdge = _chosenObjectModel as EdgeModel;
            OnDeleteEdge?.Invoke(chosenEdge);
            Unselect();
        }

        private void OnMousePressHandler(bool isMouseButtonDown)
        {
            if (isMouseButtonDown)
            {
                OnMouseButtonDown();
            }
            else
            {
                OnMouseButtonUp();
                OnModelsUpdate?.Invoke();
            }
        }

        private void OnMouseButtonDown()
        {
            switch (CurrentState)
            {
                case GraphControllerStates.SelectionMode:
                    SelectObjectPress();
                    break;
                case GraphControllerStates.NodeSelected:
                    HoldObjectPress();
                    break;
                case GraphControllerStates.PathFinding:
                    SelectPathFindPress();
                    break;
                case GraphControllerStates.SecondNodeForNewEdgeChoosing:
                    SelectSecondNodeForEdgePress();
                    break;
                case GraphControllerStates.EdgeSelected:
                    HoldObjectPress();
                    break;
                default:
                    break;
            }
        }

        private void OnMouseButtonUp()
        {
            switch (CurrentState)
            {
                case GraphControllerStates.SelectionMode:
                    SelectObjectRelease();
                    break;
                case GraphControllerStates.NodeSelected:
                    SelectObjectRelease();
                    break;
                case GraphControllerStates.ObjectMovement:
                    HoldObjectRelease();
                    break;
                case GraphControllerStates.EdgeSelected:
                    SelectObjectRelease();
                    break;
                default:
                    break;
            }

        }

        private void MoveChosenObject()
        {
            if (_isMovingObject)
            {
                _chosenObjectModel.Move(_contextModel.InputModel.MousePosition);
                if(_chosenObjectModel.ObjectType == SceneObjectTypes.GraphNode)
                {
                    foreach (var edge in _edgesConnectedToChosenNode)
                    {
                        edge.SetLinePosition();
                    }
                }
            }
        }

        private void GetEdgesConnectedToChosenNode()
        {
            int chosenNodeIndex = (_chosenObjectModel as NodeModel).Index;
            _edgesConnectedToChosenNode = _contextModel.EdgeModels.FindAll(x =>
                x.StartNodeModelIndex == chosenNodeIndex || x.EndNodeModelIndex == chosenNodeIndex);
        }

        private void HoldObjectPress()
        {
            if (!_canvasService.CreateRayCastCanvas())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_physicsService.CreateRayCast(ray, out RaycastHit hit, Data.ProgrammData.CameraRaycastLength))
                {
                    _chosenObjectId = hit.collider.gameObject.GetInstanceID();
                    if (_chosenObjectId == _chosenObjectModel.ObjectId)
                    {
                        _isMovingObject = true;
                        CurrentState = GraphControllerStates.ObjectMovement;
                    }
                }
                else
                {
                    Unselect();
                }
            }
        }

        private void HoldObjectRelease()
        {
            if (!_canvasService.CreateRayCastCanvas())
            {
                _isMovingObject = false;
                CurrentState = PreviousState;
            }
        }

        private void SelectSecondNodeForEdgePress()
        {
            if (!_canvasService.CreateRayCastCanvas())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_physicsService.CreateRayCast(ray, out RaycastHit hit, Data.ProgrammData.CameraRaycastLength))
                {
                    int focusedObjectId = hit.collider.gameObject.GetInstanceID();
                    SceneObjectModel foundModel = _contextModel.GetSceneObjectByInstanceId(focusedObjectId);
                    if (foundModel.ObjectType == SceneObjectTypes.GraphNode)
                    {
                        NodeModel firstNode = _chosenObjectModel as NodeModel;
                        NodeModel secondNode = foundModel as NodeModel;
                        EdgeModel similarEdge = _contextModel.EdgeModels.Find(x => 
                            (x.StartNodeModelIndex == firstNode.Index && x.EndNodeModelIndex == secondNode.Index) || 
                                (x.EndNodeModelIndex == firstNode.Index && x.StartNodeModelIndex == secondNode.Index));
                        if(similarEdge == null) OnCreateEdge?.Invoke(firstNode, secondNode);
                    }
                    Unselect();
                }
                else
                {
                    Unselect();
                }
            }
        }

        private void SelectPathFindPress()
        {
            if (!_canvasService.CreateRayCastCanvas())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_physicsService.CreateRayCast(ray, out RaycastHit hit, Data.ProgrammData.CameraRaycastLength))
                {
                    int focusedObjectId = hit.collider.gameObject.GetInstanceID();
                    SceneObjectModel foundModel = _contextModel.GetSceneObjectByInstanceId(focusedObjectId);
                    if (foundModel.ObjectType == SceneObjectTypes.GraphNode)
                    {
                        NodeModel firstNode = _chosenObjectModel as NodeModel;
                        NodeModel secondNode = foundModel as NodeModel;
                        DeletePath();
                        _edgesInFoundPath = _algorythmService.GetEdgesInShortestPath(firstNode, secondNode);
                        if (_edgesInFoundPath.Count > 0) ShowPath();
                    }
                    Unselect();
                }
                else
                {
                    Unselect();
                }
            }
        }

        private void SelectObjectPress()
        {
            if (!_canvasService.CreateRayCastCanvas())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_physicsService.CreateRayCast(ray, out RaycastHit hit, Data.ProgrammData.CameraRaycastLength))
                {
                    _chosenObjectId = hit.collider.gameObject.GetInstanceID();
                }
                else
                {
                    _chosenObjectId = 0;
                }
            }
        }

        private void SelectObjectRelease()
        {
            if (!_canvasService.CreateRayCastCanvas())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_physicsService.CreateRayCast(ray, out RaycastHit hit, Data.ProgrammData.CameraRaycastLength))
                {
                    int stayedObjectId = hit.collider.gameObject.GetInstanceID();
                    if (stayedObjectId == _chosenObjectId)
                    {
                        SceneObjectModel foundModel = _contextModel.GetSceneObjectByInstanceId(_chosenObjectId);
                        if (foundModel.ObjectType == SceneObjectTypes.GraphNode)
                        {
                            SelectNode(foundModel as NodeModel);
                        }
                        else
                        {
                            SelectEdge(foundModel as EdgeModel);
                        }
                    }
                }
                else
                {
                    Unselect();
                }
            }
        }

        private void SelectNode(NodeModel nodeModel)
        {
            if (_chosenObjectModel != null) _chosenObjectModel.Unselect();
            _chosenObjectModel = nodeModel;
            GetEdgesConnectedToChosenNode();
            nodeModel.Select();
            CurrentState = GraphControllerStates.NodeSelected;
        }

        private void SelectEdge(EdgeModel edgeModel)
        {
            if (_chosenObjectModel != null) _chosenObjectModel.Unselect();
            _chosenObjectModel = edgeModel;
            edgeModel.Select();
            CurrentState = GraphControllerStates.EdgeSelected;
        }

        private void Unselect()
        {
            _chosenObjectId = 0;
            if (_chosenObjectModel != null) _chosenObjectModel.Unselect();
            _chosenObjectModel = null;
            CurrentState = GraphControllerStates.SelectionMode;
        }

        private void DeletePath()
        {
            _edgesInFoundPath = null;
            foreach (var edge in _contextModel.EdgeModels)
            {
                edge.UnselectForPath();
            }
        }

        private void ShowPath()
        {
            foreach (var edge in _edgesInFoundPath)
            {
                edge.SelectForPath();
            }
        }

        #endregion
    }
}

