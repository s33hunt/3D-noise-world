using UnityEngine;
using System.Collections;

namespace FPVR
{
	public class Player : MonoBehaviour 
	{
		public Texture2D crosshair;
		public int raycastFrameRate = 15;
		public float 
			walkSpeed = 15f,
			turnSensitivity = 1,
			armsReach = 12;

		public static Player instance;
		[HideInInspector] public Quaternion camLerpTarget = Quaternion.identity;
		[HideInInspector] public Camera camera;
		[HideInInspector] public Vector3 bodyPosition;
		[HideInInspector] public Transform head, body, eye;
		[HideInInspector] public RaycastHit 
			visualFocus = new RaycastHit(), 
			emptyFocus = new RaycastHit();
		private float raycastFrameTime;
		private Networking.Observer observer;
		private Quaternion fix_rotation(Quaternion q){return new Quaternion(q.x, q.y, -q.z, -q.w);}
		private Rigidbody bodyRigidbody;
		private Rect crosshairRect, optionsRect;
		private InteractiveObject ioFocus;

		void Start()
		{
			if(Application.platform == RuntimePlatform.Android){crosshairRect = new Rect ((Screen.width * 0.5f) - 16, (Screen.height * 0.5f) - 16, 32, 32);}
			if(Application.platform == RuntimePlatform.Android){optionsRect = new Rect ((Screen.width * 0.5f) + 32, (Screen.height * 0.5f) - 16, Screen.width*0.5f, Screen.height*0.5f);}
			instance = this;
			raycastFrameTime = 1f /(float)raycastFrameRate;
			observer = GetComponent<Networking.Observer> ();
			camera = transform.GetComponentInChildren<Camera> ();
			body = transform.Find ("body");
			head = transform.Find ("head");
			eye = camera.transform.Find ("eye");
			bodyPosition = body.position;
			bodyRigidbody = body.GetComponent<Rigidbody> ();
			bodyRigidbody.freezeRotation = true;

			if (Application.platform == RuntimePlatform.Android) {
				Input.gyro.enabled = true;
				StartCoroutine("RayCasting");
			}
		}

		private IEnumerator RayCasting()
		{
			while(true){
				//visual focus
				ioFocus = null;
				RaycastHit hit;
				if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, armsReach, ~(1<<GAME.Layers.Player))){
					visualFocus = hit;
					if(visualFocus.collider.gameObject.layer == GAME.Layers.Interactive){
						ioFocus = visualFocus.collider.GetComponent<InteractiveObject>();
					}
				}else{
					visualFocus = emptyFocus;
				}
				//Debug.Log (visualFocus.collider == null ? "no target" : visualFocus.collider.name);
				yield return new WaitForSeconds(raycastFrameTime);
			}
		}

		void OnGUI()
		{
			//if(Application.platform == RuntimePlatform.Android){
				GUI.Label (crosshairRect, crosshair);//}
			//if(Application.platform == RuntimePlatform.Android){
				GUI.Label (optionsRect, ioFocus != null ? ioFocus.hoverMessage : "");//}
		}

		void Update()
		{
			//VERY SPECIFIC ORDER OF EVENTS REQUIRED TO:
			// - use controller to move relative to look dir
			// - update look dir with EITHER right stick OR gyroscope

			float 
				rightHorizontal = (Input.GetKey(KeyCode.Q) ? -1:0) + (Input.GetKey(KeyCode.E) ? 1:0), 
				walkInput = (Input.GetKey(KeyCode.W) ? 1:0) + (Input.GetKey(KeyCode.S) ? -1:0),
				strafeInput = (Input.GetKey(KeyCode.A) ? -1:0) + (Input.GetKey(KeyCode.D) ? 1:0);

			//phone client
			if (Application.platform == RuntimePlatform.Android) {
				//set camera orientation by gyro
				camera.transform.localRotation = fix_rotation (Input.gyro.attitude);

				//ps3 input
				rightHorizontal = Input.GetAxis("right_stick_horizontal");
				walkInput = Input.GetAxis("left_stick_vertical");
				strafeInput = Input.GetAxis("left_stick_horizontal");

				ButtonAction(PS3Controller.Axes.button_cross);
				ButtonAction(PS3Controller.Axes.button_square);
				ButtonAction(PS3Controller.Axes.button_triangle);
				ButtonAction(PS3Controller.Axes.button_circle);

			//not phone
			} else {
				camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, camLerpTarget, 0.5f);
				if(Networking.Manager.online){
					body.position = Vector3.Lerp(body.position, bodyPosition, 0.5f);
				}
			}

			if(!Networking.Manager.online || Application.platform == RuntimePlatform.Android){
				//turn body & head
				if(rightHorizontal != 0){
					float turnAmt = rightHorizontal * turnSensitivity * Time.deltaTime;
					body.Rotate(0,turnAmt,0);
					head.Rotate(0,turnAmt,0);
				}
				
				//move bod relative to self
				if(walkInput != 0){body.Translate(0,0,walkInput * walkSpeed * Time.deltaTime, Space.Self);}
				if(strafeInput != 0){body.Translate(strafeInput * walkSpeed * Time.deltaTime,0,0, Space.Self);}
				
				//have bod look at eye (y-axis only)
				body.LookAt(new Vector3(eye.position.x, body.position.y, eye.position.z));
			}

			//set head pos to bod pos
			head.localPosition = body.localPosition;
		}

		private void ButtonAction(string button_axis_name)
		{
			if(PS3Controller.GetButtonDown(button_axis_name)){
				if(ioFocus != null){
					if(button_axis_name == PS3Controller.Axes.button_cross){ioFocus.CrossButton();}
					if(button_axis_name == PS3Controller.Axes.button_square){ioFocus.SquareButton();}
					if(button_axis_name == PS3Controller.Axes.button_triangle){ioFocus.TriangleButton();}
					if(button_axis_name == PS3Controller.Axes.button_circle){ioFocus.CircleButton();}
				}
			}
		}
	}
}