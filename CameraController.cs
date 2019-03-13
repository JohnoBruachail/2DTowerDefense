using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	#region MapLimits

        public bool  limitMap = true;
        public float limitX = 10f; //x limit of map
        public float limitY = 10f; //y limit of map

	#endregion

	public bool useKeyboardInput = true;
	public bool useScreenEdgeInput = true;

	public bool useFixedUpdate = false; //use FixedUpdate() or Update()

	private Transform m_Transform; //camera tranform

	public float screenEdgeBorder = 25f;
	public float keyboardMovementSpeed = 5f; //speed with keyboard movement
	public float screenEdgeMovementSpeed = 3f; //spee with screen edge movement

	public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

	private void Start(){
		m_Transform = transform;
	}

	private void Update(){
		if (!useFixedUpdate){
			CameraUpdate();
		}
	}

	private void FixedUpdate(){
		if (useFixedUpdate)
			CameraUpdate();
	}

	private void CameraUpdate(){
		//if(FollowingTarget)
			//FollowTarget();
		//else
			Move();

		//HeightCalculation();
		LimitPosition();
	}


	/// <summary>
	/// limit camera position
	/// </summary>
	private void LimitPosition(){
		
		if (!limitMap)
			return;
			
		m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, -limitX, limitX), Mathf.Clamp(m_Transform.position.y, -limitY, limitY), m_Transform.position.z);
	}
	

	private Vector2 KeyboardInput{
        get { return useKeyboardInput ? new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)) : Vector2.zero; }
    }

		/// <summary>
        /// move camera with keyboard or with screen edge
        /// </summary>
        private void Move()
        {
            if (useKeyboardInput)
            {
                Vector3 desiredMove = new Vector3(KeyboardInput.x, KeyboardInput.y, 0);

                desiredMove *= keyboardMovementSpeed;
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                desiredMove = m_Transform.InverseTransformDirection(desiredMove);

                m_Transform.Translate(desiredMove, Space.Self);
            }

            if (useScreenEdgeInput)
            {
                Vector3 desiredMove = new Vector3();

                Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
                Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
                Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
                Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

                desiredMove *= screenEdgeMovementSpeed;
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                desiredMove = m_Transform.InverseTransformDirection(desiredMove);

                m_Transform.Translate(desiredMove, Space.Self);
            }
        }
}
