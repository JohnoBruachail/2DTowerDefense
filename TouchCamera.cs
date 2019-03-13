using UnityEngine;

public class TouchCamera : MonoBehaviour {

	#region MapLimits

        public bool  limitMap = true;
        public float limitX = 10f; //x limit of map
        public float limitY = 10f; //y limit of map
		public float limitZ = 5f; //z limit of map

	#endregion

	//toggles rotation on or off.
	public bool enableRotation = false;

	//vector2 touch positions of one or two fingers
	Vector2?[] oldTouchPositions = {
		null,
		null
	};
	Vector2 oldTouchVector;
	float oldTouchDistance;

	void Update() {
		//number of screen touches, one or two fingers
		if (Input.touchCount == 0) {
			oldTouchPositions[0] = null;
			oldTouchPositions[1] = null;
		}
		//if one finger touches the screen
		else if (Input.touchCount == 1) {
			//check if one position is null or if two position is not null and apply the new 
			if (oldTouchPositions[0] == null || oldTouchPositions[1] != null) {
				oldTouchPositions[0] = Input.GetTouch(0).position;
				oldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = Input.GetTouch(0).position;
				
				transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition) * GetComponent<Camera>().orthographicSize / GetComponent<Camera>().pixelHeight * 2f));
				
				oldTouchPositions[0] = newTouchPosition;
			}
		}
		//if two positions are both not null
		else {
			if (oldTouchPositions[1] == null) {
				oldTouchPositions[0] = Input.GetTouch(0).position;
				oldTouchPositions[1] = Input.GetTouch(1).position;
				oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
				oldTouchDistance = oldTouchVector.magnitude;
			}
			else {
				Vector2 screen = new Vector2(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight);
				
				Vector2[] newTouchPositions = {
					Input.GetTouch(0).position,
					Input.GetTouch(1).position
				};
				Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
				float newTouchDistance = newTouchVector.magnitude;

				transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * GetComponent<Camera>().orthographicSize / screen.y));
				
				if(enableRotation == true){
					transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
				}
				
				GetComponent<Camera>().orthographicSize *= oldTouchDistance / newTouchDistance;
				transform.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * GetComponent<Camera>().orthographicSize / screen.y);

				oldTouchPositions[0] = newTouchPositions[0];
				oldTouchPositions[1] = newTouchPositions[1];
				oldTouchVector = newTouchVector;
				oldTouchDistance = newTouchDistance;
			}
		}

		//transform.position = new Vector3(Mathf.Clamp(transform.position.x, -limitX, limitX), Mathf.Clamp(transform.position.y, -limitY, limitY), transform.position.z);
		//transform.position = new Vector3(Mathf.Clamp(transform.position.x, -limitX, limitX), Mathf.Clamp(transform.position.y, -limitY, limitY), Mathf.Clamp(transform.position.z, -limitZ, limitZ));
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -limitX, limitX), Mathf.Clamp(transform.position.y, -limitY, limitY), transform.position.z);

	}
}
