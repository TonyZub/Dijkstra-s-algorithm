using UnityEngine;


namespace TestAlgorithm
{
    public sealed class EdgeModel : SceneObjectModel
    {
        #region Fields

        private readonly NodeModel _startNodeModel;
        private readonly NodeModel _endNodeModel;
        private readonly LineRenderer _lineRenderer;

        #endregion


        #region Properties

        public int Weight { get; private set; }

        #endregion


        #region Constructor

        public EdgeModel(GameObject objectOnScene, Vector2 startPosition, NodeModel startNodeModel, NodeModel endNodeModel, int weight) : 
            base (objectOnScene, startPosition)
        {
            _startNodeModel = startNodeModel;
            _endNodeModel = endNodeModel;
            _lineRenderer = ObjectOnScene.GetComponentInChildren<LineRenderer>();   
            Weight = weight;

            SetTextWeight();
            SetLinePosition();
            SetLineMaterial();
        }

        #endregion


        #region Methods

        private void SetTextWeight()
        {
            TextComponent.text = Weight.ToString();
        }

        private void SetLinePosition()
        {
            Vector3[] linePointsPositions = new[] 
            {
                _startNodeModel.ObjectTransform.position,
                this.ObjectTransform.position,
                _endNodeModel.ObjectTransform.position
            };
            _lineRenderer.SetPositions(linePointsPositions);
        }

        private void SetLineMaterial()
        {
            _lineRenderer.material = Weight > 0 ? Data.ProgrammData.PositiveWeightLine : Weight < 0 ?
                Data.ProgrammData.NegativeWeightLine : Data.ProgrammData.ZeroWeightLine;
        }

        public void ChangeWeight(int newWeight)
        {
            Weight = newWeight;
            SetTextWeight();
            SetLineMaterial();
        }

        public override void Move(Vector2 positionTo)
        {
            base.Move(positionTo);
            ObjectTransform.position = positionTo;
        }

        #endregion
    }
}

