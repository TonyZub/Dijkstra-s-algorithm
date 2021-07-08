using UnityEngine;
using UnityEditor;


namespace TestAlgorithm
{
    public sealed class ShowPopup : EditorWindow
    {
#if UNITY_EDITOR

        #region Fields

        public string MainText;

        #endregion


        #region Methods

        public static void Init(string mainText, float width = 250, float height = 150)
        {
            ShowPopup window = ScriptableObject.CreateInstance<ShowPopup>();
            window.MainText = mainText;
            window.position = new Rect(Screen.width / 2, Screen.height / 2, width, height);
            window.ShowPopup();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField(MainText, EditorStyles.wordWrappedLabel);
            GUILayout.Space(70);
            if (GUILayout.Button("Ok")) Close();
        }

        #endregion

#endif
    }
}

