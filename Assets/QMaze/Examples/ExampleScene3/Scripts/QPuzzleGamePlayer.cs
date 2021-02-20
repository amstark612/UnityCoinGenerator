using UnityEngine;
using System.Collections;

namespace qtools.qmaze.example3
{
	public class QPuzzleGamePlayer : MonoBehaviour 
	{
		public delegate void GoalHandler();
		private GoalHandler goalHandler;

		public Transform playerGeometryTransform;
		public float lerp = 8f;

		private Vector3 goalPosition;
		
		void Start () 
		{
			playerGeometryTransform.parent = null;
		}

		void Update () 
		{
			playerGeometryTransform.position = Vector3.Lerp(playerGeometryTransform.position, transform.position, lerp * Time.deltaTime);

			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
			{
				if (!Physics.Raycast(new Ray(transform.position, -transform.right), 1))
				{
					transform.position += -transform.right;
				}
			}
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) 
			{
				if (!Physics.Raycast(new Ray(transform.position, transform.right), 1))
				{
					transform.position += transform.right;
				}
			}
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
			{
				if (!Physics.Raycast(new Ray(transform.position, transform.forward), 1))
				{
					transform.position += transform.forward;
				}
			}
			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) 
			{
				if (!Physics.Raycast(new Ray(transform.position, -transform.forward), 1))
				{
					transform.position += -transform.forward;
				}
			}

			if (goalHandler != null && Vector3.SqrMagnitude(goalPosition - playerGeometryTransform.transform.position) < 0.5f)
			{
				goalHandler();
				goalHandler = null;
			}
		}

		public void setPosition(Vector3 position)
		{
			playerGeometryTransform.transform.position = transform.position = new Vector3(position.x, 2.75f, position.z);
		}

		public void setGoal(Vector3 goalPosition, GoalHandler handler)
		{
			this.goalHandler = handler;
			this.goalPosition = goalPosition;
			this.goalPosition.y = 2.75f;
		}
	}
}