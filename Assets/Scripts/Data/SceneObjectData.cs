using UnityEngine;


namespace TestAlgorithm
{
    [System.Serializable]
    public class SceneObjectData
    {
        #region Fields

        [SerializeField] private Vector2 _position;

        #endregion


        #region Properties

        public Vector2 Position => _position;

        #endregion
    }
}

