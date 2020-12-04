using UnityEngine;
using System.Collections;

namespace qtools.qmaze.example3
{
	public class QMaterialAlpha : MonoBehaviour 
	{
		public Material material;
		public float lerp = 0.2f;

		private float currentAlpha;
		private float targetAlpha;

		void Awake()
		{
			currentAlpha = 0;
			targetAlpha = 0;
			material.SetFloat("_Alpha", currentAlpha);
			enabled = false;
		}

		public void setAlpha(float value)
		{
			targetAlpha = value;
			enabled = true;			 
		}

		void Update () 
		{
			currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, lerp * Time.deltaTime * 100);
			material.SetFloat("_Alpha", currentAlpha);
			if (Mathf.Abs(currentAlpha - targetAlpha) < 0.01f) enabled = false;
		}
	}
}