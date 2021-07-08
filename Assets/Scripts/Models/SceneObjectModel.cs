using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace TestAlgorithm
{
    public abstract class SceneObjectModel
    {
        #region Fields

        protected SceneObjectTypes _objectType;

        #endregion


        #region Properties

        public SceneObjectTypes ObjectType => _objectType;
        public GameObject ObjectOnScene { get; }
        public Transform ObjectTransform { get; }
        public TMP_Text TextComponent { get; }
        public Image SelectionImage { get; }
        public float ObjectId { get; }

        #endregion


        #region Constructor

        public SceneObjectModel(GameObject objectOnScene, Vector2 startPosition)
        {
            _objectType = SceneObjectTypes.None;
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

        public virtual void Move(Vector2 newPosition)
        {
            float newPositionX = Mathf.Clamp(newPosition.x, Data.ProgrammData.ScreenEdgeLeft, 
                Data.ProgrammData.ScreenEdgeRight);
            float newPositionY = Mathf.Clamp(newPosition.y, Data.ProgrammData.ScreenEdgeDown, 
                Data.ProgrammData.ScreenEdgeUp);
            ObjectTransform.position = new Vector3(newPositionX, newPositionY, 0);
        }

        public void UpdateName(int index)
        {
            ObjectOnScene.name = index.ToString();
        }

        public void DestroyObjectOnScene()
        {
            Object.Destroy(ObjectOnScene);
        }

        #endregion
    }
}

