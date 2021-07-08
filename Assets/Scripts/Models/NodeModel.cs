using UnityEngine;


namespace TestAlgorithm
{
    public sealed class NodeModel : SceneObjectModel
    {
        #region Fields

        private int _index;

        #endregion


        #region Properties

        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                SetTextIndex();
            }
        }

        #endregion


        #region Constructor

        public NodeModel(GameObject objectOnScene, Vector2 startPosition, int index) :
            base(objectOnScene, startPosition)
        {
            _objectType = SceneObjectTypes.GraphNode;
            Index = index;
        }

        #endregion


        #region Methods

        private void SetTextIndex()
        {
            TextComponent.text = Index.ToString();
            ObjectOnScene.name = Index.ToString();
        }

        #endregion
    }
}

