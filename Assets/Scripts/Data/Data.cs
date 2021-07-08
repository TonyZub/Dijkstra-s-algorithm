using System.IO;
using UnityEngine;


namespace TestAlgorithm
{
    [CreateAssetMenu(fileName = "Data", menuName = "CreateData/Data")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _programmDataPath;
        [SerializeField] private string _graphDataPath;

        private static Data _instance;
        private static ProgrammData _programmData;
        private static GraphData _graphData;

        #endregion


        #region Properties

        public static Data Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<Data>("Data/" + typeof(Data).Name);
                }
                return _instance;
            }
        }

        public static ProgrammData ProgrammData
        {
            get
            {
                if (_programmData == null)
                {
                    _programmData = Resources.Load<ProgrammData>("Data/" + Instance._programmDataPath);
                }
                return _programmData;
            }
        }

        public static GraphData GraphData
        {
            get
            {
                if (_graphData == null)
                {
                    _graphData = Resources.Load<GraphData>("Data/" + Instance._graphDataPath);
                }
                return _graphData;
            }
        }

        #endregion


        #region Methods

        private static T Load<T>(string resourcesPath) where T : UnityEngine.Object =>
            Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));

        #endregion
    }
}