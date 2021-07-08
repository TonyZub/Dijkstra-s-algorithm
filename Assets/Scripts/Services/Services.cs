namespace TestAlgorithm
{
    public sealed class Services
    {
        #region Fields

        public static readonly Services SharedInstance = new Services();

        #endregion


        #region Properties

        public PhysicsService PhysicsService { get; private set; }
        public MainCanvasService MainCanvasService { get; private set; }
        public DijkstrasAlgorithmService DijkstrasAlgorithmService { get; private set; }

        #endregion


        #region Methods

        public void InitializeServices(ContextModel context)
        {
            PhysicsService = new PhysicsService();
            MainCanvasService = new MainCanvasService(context);
            DijkstrasAlgorithmService = new DijkstrasAlgorithmService(context);
        }

        #endregion
    }
}

