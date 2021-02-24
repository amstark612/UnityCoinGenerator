using UnityEngine;
using System.Collections;
using qtools.qmaze;

namespace qtools.qmaze.example3
{
	public class QPuzzleMazeCamera : MonoBehaviour 
	{	
		public float panLerp = 0.2f;
		public float zoomLerp = 4.0f;
		public float targetZoom = 5;
		public Transform targetTransform;

		private bool followMode = false;

		public void Update()
		{
			if (followMode)
			{
				transform.position = Vector3.Lerp(transform.position, targetTransform.position - Camera.main.transform.forward * 50, panLerp * Time.deltaTime);
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomLerp * Time.unscaledDeltaTime);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, new Vector3(17, 12, -17), panLerp * Time.deltaTime);
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10, zoomLerp * Time.unscaledDeltaTime);
			}
		}

		public void setFollowMode(bool value)
		{
			followMode = value;
		}
	}
}