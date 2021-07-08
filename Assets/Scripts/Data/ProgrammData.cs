using UnityEngine;


namespace TestAlgorithm
{
    [CreateAssetMenu(fileName = "ProgrammData", menuName = "CreateData/ProgrammData")]
    public sealed class ProgrammData : ScriptableObject
    {
        #region Fields

        [SerializeField] private Vector2 _activeScreenSize;
        [SerializeField] private Material _positiveWegithLine;
        [SerializeField] private Material _zeroWeightLine;
        [SerializeField] private Material _chosenPathLine;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private GameObject _edgePrefab;
        [SerializeField] private GameObject _nodePanelPrefab;
        [SerializeField] private GameObject _edgePanelPrefab;
        [SerializeField] private GameObject _mainPanelPrefab;

        [SerializeField] private string _mainCameraTag;
        [SerializeField] private string _nodePanelTitle;
        [SerializeField] private string _nodePanelPathFindingTitle;
        [SerializeField] private string _nodePanelEdgeCreatingTitle;
        [SerializeField] private string _edgePanelTitle;
        [SerializeField] private float _cameraRaycastLength;
        [SerializeField] private int _maximalEdgeWeight;
        [SerializeField] private bool _saveProgressAfterPlayMode;

        #endregion


        #region Properties

        public Vector2 ActiveScreenSize => _activeScreenSize;
        public Material PositiveWeightLine => _positiveWegithLine;
        public Material ZeroWeightLine => _zeroWeightLine;
        public Material ChosenPathLine => _chosenPathLine;
        public GameObject NodePrefab => _nodePrefab;
        public GameObject EdgePrefab => _edgePrefab;
        public GameObject NodePanelPrefab => _nodePanelPrefab;
        public GameObject EdgePanelPrefab => _edgePanelPrefab;
        public GameObject MainPanelPrefab => _mainPanelPrefab;

        public string MainCameraTag => _mainCameraTag;
        public string NodePanelTitle => _nodePanelTitle;
        public string NodePanelPathFindingTitle => _nodePanelPathFindingTitle;
        public string NodePanelEdgeCreatingTitle => _nodePanelEdgeCreatingTitle;
        public string EdgePanelTitle => _edgePanelTitle;
        public float CameraRaycastLength => _cameraRaycastLength;
        public float ScreenEdgeRight => _activeScreenSize.x / 2;
        public float ScreenEdgeLeft => -_activeScreenSize.x / 2;
        public float ScreenEdgeUp => _activeScreenSize.y / 2;
        public float ScreenEdgeDown => -_activeScreenSize.y / 2;
        public int MaximalEdgeWeight => _maximalEdgeWeight;
        public bool SaveProgressAfterPlayMode => _saveProgressAfterPlayMode;

        #endregion
    }
}

