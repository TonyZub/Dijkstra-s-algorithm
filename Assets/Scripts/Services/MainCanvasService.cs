using UnityEngine.EventSystems;


namespace TestAlgorithm
{
    public sealed class MainCanvasService
    {
        #region Fields

        private readonly ContextModel _context;
        private EventSystem _eventSystem;

        #endregion


        #region Constructor

        public MainCanvasService(ContextModel context)
        {
            _context = context;
            _eventSystem = _context.CanvasOverlay.GetComponentInChildren<EventSystem>();
        }

        #endregion


        #region Methods

        public bool CreateRayCastCanvas()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        #endregion
    }
}

