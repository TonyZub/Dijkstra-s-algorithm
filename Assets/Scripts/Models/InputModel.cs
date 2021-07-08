using System;
using UnityEngine;


namespace TestAlgorithm
{
    public sealed class InputModel
    {
        #region Fields

        public event Action<bool> OnMousePressed;

        #endregion


        #region Properties

        public Vector2 MousePosition { get; set; }
        public bool IsMousePressed { get; private set; }

        #endregion


        #region Constructor

        public InputModel(ref Action<bool> onMousePress)
        {
            onMousePress += (isPressed) => GetMousePressed(isPressed);
        }

        #endregion


        #region Methods

        private void GetMousePressed(bool isPressed)
        {
            IsMousePressed = isPressed;
            OnMousePressed?.Invoke(IsMousePressed);
        }

        #endregion
    }
}

