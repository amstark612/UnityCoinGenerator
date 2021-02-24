using UnityEngine;
using System.Collections;

namespace qtools.qmaze.example2
{
	[RequireComponent(typeof(Camera))]
	public class QTPSCamera : MonoBehaviour 
	{
		public Transform targetTransform;
		public Vector3 offset = new Vector3(1.0f, 5.0f, 1.0f);
		public float lerp = 0.4f;

		private Vector3 targetPosition;

		void Start()
		{
			targetPosition = targetTransform.position;

			transform.position = targetTransform.position + offset;
			transform.rotation = Quaternion.LookRotation(targetTransform.position - transform.position);
		}

		void LateUpdate()
		{
			if (targetTransform == null) return;

			targetPosition = Vector3.Lerp(targetPosition, targetTransform.position, lerp * Time.deltaTime);

			transform.position = targetPosition + offset;

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetTransform.position - transform.position), lerp * Time.deltaTime);
		}
	}
}