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
        public int StartNodeModelIndex => _startNodeModel.Index;
        public int EndNodeModelIndex => _endNodeModel.Index;

        #endregion


        #region Constructor

        public EdgeModel(GameObject objectOnScene, Vector2 startPosition, NodeModel startNodeModel, 
            NodeModel endNodeModel, int weight) : base (objectOnScene, startPosition)
        {
            _objectType = SceneObjectTypes.GraphEdge;
            _startNodeModel = startNodeModel.Index < endNodeModel.Index ? startNodeModel : endNodeModel;
            _endNodeModel = startNodeModel.Index > endNodeModel.Index ? startNodeModel : endNodeModel;
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

        public void SetLinePosition()
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
            _lineRenderer.material = Weight > 0 ? Data.ProgrammData.PositiveWeightLine : Data.ProgrammData.ZeroWeightLine;
        }

        public void ChangeWeight(int newWeight)
        {
            Weight = Mathf.Clamp(newWeight, 0, Data.ProgrammData.MaximalEdgeWeight);
            SetTextWeight();
            SetLineMaterial();
        }

        public override void Move(Vector2 positionTo)
        {
            base.Move(positionTo);
            SetLinePosition();
        }

        public void SelectForPath()
        {
            _lineRenderer.material = Data.ProgrammData.ChosenPathLine;
        }

        public void UnselectForPath()
        {
            SetLineMaterial();
        }

        #endregion
    }
}

