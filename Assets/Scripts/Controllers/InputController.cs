using UnityEngine;
using System;


namespace TestAlgorithm
{
    public sealed class InputController : MonoBehaviour
    {
        #region Constants

        private const string MOUSE_X_AXIS_NAME = "Mouse X";
        private const string MOUSE_Y_AXIS_NAME = "Mouse Y";

        #endregion


        #region Fields

        private ContextModel _contextModel;
        private Action<bool> _onMousePressed;
        private Vector2 _mousePosition;
        private bool _isMousePressed;

        #endregion


        #region Properties

        private bool IsMousePressed
        {
            get
            {
                return _isMousePressed;
            }
            set
            {
                if(value != _isMousePressed)
                {
                    _onMousePressed?.Invoke(value);
                    _isMousePressed = value;
                }
            }
        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _contextModel = GetComponent<ContextModel>();
            _contextModel.InitInputModel(new InputModel(ref _onMousePressed));
            _isMousePressed = false;
        }

        private void Update()
        {
            GetMouseInput();
        }

        #endregion


        #region Methods

        private void GetMouseInput()
        {
            IsMousePressed = Input.GetMouseButton(0);
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _contextModel.InputModel.MousePosition = _mousePosition;
        }

        #endregion
    }
}

