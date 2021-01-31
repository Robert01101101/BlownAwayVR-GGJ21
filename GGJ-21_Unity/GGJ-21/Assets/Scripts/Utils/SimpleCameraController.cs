using UnityEngine;

namespace Utils
{
    public class SimpleCameraController : MonoBehaviour
    {
        private Vector3 prevMouseDirection;

        private Vector3 prevMousePosition;
        
        [SerializeField]
        private bool allowMovement = default;
    
        [SerializeField]
        private float movementSpeed = default;
    
        [SerializeField]
        private float xAxisTurnSpeed = default;
    
        [SerializeField]
        private float yAxisTurnSpeed = default;

        private bool firstUpdate = true;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Vector3 mousePositionDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
            float yAxisTurn = mousePositionDelta.x;
            float xAxisTurn = -mousePositionDelta.y;
            Vector3 angle = Camera.main.transform.rotation.eulerAngles + new Vector3(xAxisTurn*xAxisTurnSpeed, yAxisTurn*yAxisTurnSpeed, 0) * Time.deltaTime;
            angle.z = 0f;
            Camera.main.transform.rotation = Quaternion.Euler(angle);

            if (allowMovement)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Camera.main.transform.Translate(Camera.main.transform.forward*movementSpeed*Time.deltaTime, Space.World);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    Camera.main.transform.Translate(-Camera.main.transform.forward*movementSpeed*Time.deltaTime, Space.World);
                }
            }
        }
    }
}