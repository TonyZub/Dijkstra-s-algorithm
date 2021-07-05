using UnityEngine;


namespace TestAlgorithm
{
    public sealed class NodeModel : SceneObjectModel
    {
        #region Properties

        public int Index { get; }

        #endregion


        #region Constructor

        public NodeModel(GameObject objectOnScene, Vector2 startPosition, int index) :
            base(objectOnScene, startPosition)
        {
            Index = index;
            TextComponent.text = Index.ToString();
        }

        #endregion
    }
}

