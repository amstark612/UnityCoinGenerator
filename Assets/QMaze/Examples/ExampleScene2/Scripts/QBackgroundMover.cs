using UnityEngine;
using System.Collections;

namespace qtools.qmaze.example2
{
	public class QBackgroundMover : MonoBehaviour 
	{	
		public Vector3 offset = new Vector3(0.0f, -50.0f, 0.0f);

		private Transform targetTransform;
		
		void Start()
		{
			targetTransform = Camera.main.transform;		
			transform.position = targetTransform.position + offset;
		}
		
		void LateUpdate()
		{
			if (targetTransform == null) return;
			transform.position = targetTransform.position + offset;
		}
	}
}