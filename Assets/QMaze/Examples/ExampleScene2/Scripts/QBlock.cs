using UnityEngine;
using System.Collections;

namespace qtools.qmaze.example2
{
	public class QBlock : MonoBehaviour
	{	
		public delegate void QBlockTriggerHandler();
		
		public event QBlockTriggerHandler triggerHandlerEvent;

		public GameObject colliderGameObject;
		public GameObject blockGameObject;
		public Material blockMaterial;

		private float currentBlockMaterialAlpha = 0;

		void Awake () 
		{
			blockGameObject.SetActive(false);
		}

		void OnTriggerEnter(Collider other) 
		{
			if (other.tag == "Player")
			{
				blockGameObject.SetActive(true);
				colliderGameObject.SetActive(false);
				this.enabled = true;

				currentBlockMaterialAlpha = 0;

				if (triggerHandlerEvent != null)				
					triggerHandlerEvent();
			}
		}

		void Update()
		{
			currentBlockMaterialAlpha = Mathf.Lerp(currentBlockMaterialAlpha, 1, Time.deltaTime);
			blockMaterial.SetFloat("_Alpha", currentBlockMaterialAlpha);
			if (currentBlockMaterialAlpha >= 0.99) this.enabled = false;
		}
	}
}