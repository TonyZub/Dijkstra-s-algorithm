using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace TestAlgorithm
{
    public abstract class SceneObjectModel
    {
        #region Properties

        public GameObject ObjectOnScene { get; }
        public Transform ObjectTransform { get; }
        public TMP_Text TextComponent { get; }
        public Image SelectionImage { get; }
        public float ObjectId { get; }

        #endregion


        #region Constructor

        public SceneObjectModel(GameObject objectOnScene, Vector2 startPosition)
        {
            ObjectOnScene = objectOnScene;
            ObjectTransform = ObjectOnScene.transform;
            ObjectId = ObjectOnScene.GetComponentInChildren<Collider>().gameObject.GetInstanceID();
            ObjectTransform.position = startPosition;
            TextComponent = ObjectOnScene.GetComponentInChildren<TMP_Text>();
            SelectionImage = ObjectOnScene.GetComponentInChildren<Image>();
        }

        #endregion


        #region Methods

        public void Select()
        {
            SelectionImage.enabled = true;
        }

        public void Unselect()
        {
            SelectionImage.enabled = false;
        }

        public virtual void Move(Vector2 movement)
        {
            float screenWidth = Data.ProgrammData.ActiveScreenSize.x;
            float screenHeight = Data.ProgrammData.ActiveScreenSize.y;
            float newPositionX = Mathf.Clamp(ObjectTransform.position.x + movement.x, -screenWidth / 2, screenWidth / 2);
            float newPositionY = Mathf.Clamp(ObjectTransform.position.y + movement.y, -screenHeight / 2, screenHeight / 2);
            ObjectTransform.position = new Vector3(newPositionX, newPositionY, 0);
        }

        #endregion
    }
}

