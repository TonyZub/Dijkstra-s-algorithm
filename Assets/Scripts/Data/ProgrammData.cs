using UnityEngine;


namespace TestAlgorithm
{
    [CreateAssetMenu(fileName = "ProgrammData", menuName = "CreateData/ProgrammData")]
    public sealed class ProgrammData : ScriptableObject
    {
        #region Fields

        [SerializeField] private Vector2 _activeScreenSize;
        [SerializeField] private Material _negativeWegithLine;
        [SerializeField] private Material _positiveWeightLine;
        [SerializeField] private Material _zeroWeightLine;
        [SerializeField] private Material _chosenPathLine;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private GameObject _edgePrefab;
        [SerializeField] private string _mainCameraTag;
        [SerializeField] private float _cameraRaycastLength;

        #endregion


        #region Properties

        public Vector2 ActiveScreenSize => _activeScreenSize;
        public Material NegativeWeightLine => _negativeWegithLine;
        public Material PositiveWeightLine => _positiveWeightLine;
        public Material ZeroWeightLine => _zeroWeightLine;
        public Material ChosenPathLine => _chosenPathLine;
        public GameObject NodePrefab => _nodePrefab;
        public GameObject EdgePrefab => _edgePrefab;

        public string MainCameraTag => _mainCameraTag;
        public float CameraRaycastLength => _cameraRaycastLength;

        #endregion
    }
}

