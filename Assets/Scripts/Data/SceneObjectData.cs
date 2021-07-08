using UnityEngine;


namespace TestAlgorithm
{
    [System.Serializable]
    public abstract class SceneObjectData
    {
        #region Fields

        [SerializeField] private Vector2 _position;

        #endregion


        #region Properties

        public Vector2 Position => _position;

        #endregion


        #region Constructor

        public SceneObjectData(Vector2 position)
        {
            _position = position;
        }

        #endregion


        #region Methods

        public virtual void OnValidate()
        {
            _position.x = Mathf.Clamp(_position.x, Data.ProgrammData.ScreenEdgeLeft, Data.ProgrammData.ScreenEdgeRight);
            _position.y = Mathf.Clamp(_position.y, Data.ProgrammData.ScreenEdgeDown, Data.ProgrammData.ScreenEdgeUp);
        }

        #endregion
    }
}

