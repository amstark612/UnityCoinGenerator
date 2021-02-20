using UnityEngine;
using System.Collections;

namespace qtools.qmaze.example2
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(SphereCollider))]
	public class QBallController : MonoBehaviour 
	{
		public float moveScaleX = 1.0f;
		public float moveScaleY = 1.0f;
		public float moveLerp = 20f;
		public GameObject ballGeometry;
		
		private Rigidbody rigidBody;
		private float sphereRadius;
		
		void Start () 
		{
			ballGeometry.transform.SetParent(null);
			rigidBody = GetComponent<Rigidbody>();
			sphereRadius = GetComponent<SphereCollider>().radius;
		}
		
		void FixedUpdate () 
		{
			if (Physics.SphereCast(new Ray(transform.position, Vector3.down), sphereRadius * 0.95f, 0.1f))
			{
				Vector3 force = new Vector3(Input.GetAxis("Horizontal") * moveScaleX, 0, Input.GetAxis("Vertical"  ) * moveScaleY);
				rigidBody.AddForce(force);
			}

			ballGeometry.transform.position = Vector3.Lerp(ballGeometry.transform.position, transform.position, moveLerp * Time.deltaTime);
			ballGeometry.transform.rotation = Quaternion.Lerp(ballGeometry.transform.rotation, transform.rotation, moveLerp * Time.deltaTime);
		}
	}
}