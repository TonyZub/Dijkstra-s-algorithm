using UnityEngine;
using System.Linq;


namespace TestAlgorithm
{
    public sealed class GraphController : MonoBehaviour
    {
        #region Fields

        private ContextModel _contextModel;
        private SceneObjectModel _chosenObjectModel;
        private int _chosenObjectId;
        private bool _isMouseButtonDown;

        #endregion


        #region Properties


        #endregion


        #region UnityMethods

        private void Start()
        {
            _contextModel = GetComponent<ContextModel>();
        }

        private void Update()
        {
            GetInput();
        }

        #endregion


        #region Methods

        private void GetInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseButtonDown();
            }
            else if(Input.GetMouseButtonUp(0))
            {
                OnMouseButtonUp();
            }
        }

        private void OnMouseButtonDown()
        {
            _isMouseButtonDown = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Data.ProgrammData.CameraRaycastLength))
            {
                _chosenObjectId = hit.collider.gameObject.GetInstanceID();
            }
            else
            {
                _chosenObjectId = 0;
            }
        }

        private void OnMouseButtonUp()
        {
            _isMouseButtonDown = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Data.ProgrammData.CameraRaycastLength))
            {
                int stayedObjectId = hit.collider.gameObject.GetInstanceID();
                if(stayedObjectId == _chosenObjectId)
                {
                    NodeModel nodeModel = _contextModel.NodeModels.Find(x => x.ObjectId == stayedObjectId);
                    if(nodeModel != null)
                    {
                        if (_chosenObjectModel != null) _chosenObjectModel.Unselect();
                        _chosenObjectModel = nodeModel;
                        nodeModel.Select();
                    }
                    else
                    {
                        EdgeModel edgeModel = _contextModel.EdgeModels.Find(x => x.ObjectId == stayedObjectId);
                        if (_chosenObjectModel != null) _chosenObjectModel.Unselect();
                        _chosenObjectModel = edgeModel;
                        edgeModel.Select();
                    }
                }
            }
            else
            {
                if (_chosenObjectModel != null) _chosenObjectModel.Unselect();
                _chosenObjectModel = null;
            }
        }

        #endregion
    }
}

