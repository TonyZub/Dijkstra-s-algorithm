//using UnityEditor;
//using UnityEngine;


//namespace TestAlgorithm
//{
//    [CustomEditor(typeof(GraphData))]
//    public class GraphDataEditor : Editor
//    {
//        #region Fields

//        private GraphData _script = null;
//        private SerializedProperty _nodeDatas;
//        private SerializedProperty _edgeDatas;

//        #endregion


//        #region Methods

//        private void OnEnable()
//        {
//            _script = (GraphData)target;
//            _nodeDatas = serializedObject.FindProperty(GraphData.NODE_DATAS_FIELD_NAME);
//            _edgeDatas = serializedObject.FindProperty(GraphData.EDGE_DATAS_FIELD_NAME);
//        }

//        public override void OnInspectorGUI()
//        {

//        }

//        #endregion
//    }
//}

