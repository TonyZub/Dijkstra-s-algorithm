using UnityEngine;
using UnityEngine.UI;
using System;


namespace TestAlgorithm
{
    public sealed class CanvasController : MonoBehaviour
    {
        #region Fields

        public event Action OnPressCreateNewNode;
        public event Action OnPressDeletePath;
        public event Action OnPressQuit;
        public event Action OnPressFindPath;
        public event Action OnPressDeleteNode;
        public event Action OnPressCreateNewEdge;
        public event Action OnPressIncreaceWeight;
        public event Action OnPressDecreaseWeight;
        public event Action OnPressDeleteEdge;

        private ContextModel _contextModel;
        private GraphController _graphController;

        private Button[] _mainPanelButtons;
        private Button[] _nodePanelButtons;
        private Button[] _edgePanelButtons;
        private Text _nodePanelText;
        private Text _edgePanelText;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _contextModel = GetComponent<ContextModel>();
            SetButtonsFunctions();
            _contextModel.MainPanel.SetActive(true);
            _graphController = GetComponent<GraphController>();
            SignUpGraphControllerEvent();
            InitPanelTitles();
        }

        private void OnApplicationQuit()
        {
            RemoveButtonFunction();
            SignOutGraphControllerEvent();
        }

        #endregion


        #region Methods

        private void InitPanelTitles()
        {
            _nodePanelText = _contextModel.NodePanel.GetComponentsInChildren<Text>()[0];
            _edgePanelText = _contextModel.EdgePanel.GetComponentsInChildren<Text>()[0];
            SetNodePanelTitle(Data.ProgrammData.NodePanelTitle, Color.black);
            SetEdgePanelTitle(Data.ProgrammData.EdgePanelTitle, Color.black);
        }

        private void SignUpGraphControllerEvent()
        {
            _graphController.OnCurrentGraphControllerStateChanged += (state) => GraphControllerStateChangeHandler(state);
        }

        private void SignOutGraphControllerEvent()
        {
            _graphController.OnCurrentGraphControllerStateChanged -= (state) => GraphControllerStateChangeHandler(state);
        }

        private void SetButtonsFunctions()
        {
            _mainPanelButtons = _contextModel.MainPanel.GetComponentsInChildren<Button>();
            _nodePanelButtons = _contextModel.NodePanel.GetComponentsInChildren<Button>();
            _edgePanelButtons = _contextModel.EdgePanel.GetComponentsInChildren<Button>();

            _mainPanelButtons[0]?.onClick.AddListener(() => OnPressCreateNewNode?.Invoke());
            _mainPanelButtons[1]?.onClick.AddListener(() => OnPressDeletePath?.Invoke());
            _mainPanelButtons[2]?.onClick.AddListener(() => OnPressQuit?.Invoke());
            _nodePanelButtons[0]?.onClick.AddListener(() => OnPressCreateNewEdge?.Invoke());
            _nodePanelButtons[1]?.onClick.AddListener(() => OnPressFindPath?.Invoke());
            _nodePanelButtons[2]?.onClick.AddListener(() => OnPressDeleteNode?.Invoke());
            _edgePanelButtons[0]?.onClick.AddListener(() => OnPressIncreaceWeight?.Invoke());
            _edgePanelButtons[1]?.onClick.AddListener(() => OnPressDecreaseWeight?.Invoke());
            _edgePanelButtons[2]?.onClick.AddListener(() => OnPressDeleteEdge?.Invoke());
        }

        private void RemoveButtonFunction()
        {
            _mainPanelButtons[0]?.onClick.RemoveAllListeners();
            _mainPanelButtons[1]?.onClick.RemoveAllListeners();
            _mainPanelButtons[2]?.onClick.RemoveAllListeners();
            _nodePanelButtons[0]?.onClick.RemoveAllListeners();
            _nodePanelButtons[1]?.onClick.RemoveAllListeners();
            _nodePanelButtons[2]?.onClick.RemoveAllListeners();
            _edgePanelButtons[0]?.onClick.RemoveAllListeners();
            _edgePanelButtons[1]?.onClick.RemoveAllListeners();
            _edgePanelButtons[2]?.onClick.RemoveAllListeners();
        }

        private void GraphControllerStateChangeHandler(GraphControllerStates currentState)
        {
            switch (currentState)
            {
                case GraphControllerStates.SelectionMode:
                    RemoveSelectionPanels();
                    SetNodePanelDefaultTitle();
                    break;
                case GraphControllerStates.NodeSelected:
                    SetNodeSelectedPanel();
                    SetNodePanelDefaultTitle();
                    break;
                case GraphControllerStates.EdgeSelected:
                    SetEdgeSelectedPanel();
                    SetNodePanelDefaultTitle();
                    break;
                case GraphControllerStates.PathFinding:
                    SetNodePanelTitle(Data.ProgrammData.NodePanelPathFindingTitle, Color.red);
                    break;
                case GraphControllerStates.SecondNodeForNewEdgeChoosing:
                    SetNodePanelTitle(Data.ProgrammData.NodePanelEdgeCreatingTitle, Color.red);
                    break;
                default:      
                    break;
            }
        }

        private void SetNodeSelectedPanel()
        {
            _contextModel.EdgePanel.SetActive(false);
            _contextModel.NodePanel.SetActive(true);
        }

        private void SetEdgeSelectedPanel()
        {
            _contextModel.EdgePanel.SetActive(true);
            _contextModel.NodePanel.SetActive(false);
        }

        private void RemoveSelectionPanels()
        {
            _contextModel.EdgePanel.SetActive(false);
            _contextModel.NodePanel.SetActive(false);
        }

        private void SetEdgePanelTitle(string title, Color titleColor)
        {
            _edgePanelText.text = title;
            _edgePanelText.color = titleColor;
        }
        
        private void SetNodePanelTitle(string title, Color titleColor)
        {
            _nodePanelText.text = title;
            _nodePanelText.color = titleColor;
        }

        private void SetNodePanelDefaultTitle()
        {
            SetNodePanelTitle(Data.ProgrammData.NodePanelTitle, Color.black);
        }

        #endregion
    }
}

