using UnityEngine;
using System.Collections;

namespace qtools.qmaze.example3
{
	public class QMazeMover : MonoBehaviour 
	{
		public delegate void CompleteHandler();
		private CompleteHandler completeHandler;

		private Vector3 targetPosition;
		private float startTime;

		public void show(float delay = 0, CompleteHandler completeHandler = null)
		{
			this.completeHandler = completeHandler;
			enabled = true;
			startTime = Time.time + delay;
			targetPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
		}

		public void hide(float delay = 0, CompleteHandler completeHandler = null)
		{
			this.completeHandler = completeHandler;
			enabled = true;
			startTime = Time.time + delay;
			targetPosition = new Vector3(transform.localPosition.x, -3, transform.localPosition.z);
		}

		void Update () 
		{
			if (Time.time > startTime)
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 100 * Mathf.SmoothStep(0.0f, 1.0f, 0.1f) * Time.deltaTime);
				if (Vector3.SqrMagnitude(transform.position - targetPosition) < 0.01f)
				{
					enabled = false;
					if (completeHandler != null) completeHandler();
				}
			}
		}
	}
}
