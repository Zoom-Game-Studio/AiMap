using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;

namespace AppLogic
{
    [RequireComponent(typeof(Camera))]
    public class WayzCameraTargeting : MonoBehaviour
    {
		// Which layers targeting ray must hit (-1 = everything)
		public LayerMask targetingLayerMask = -1;

		// Targeting ray length
		private float targetingRayLength = Mathf.Infinity;

		// Camera component reference
		private Camera cam;

		void Awake()
		{
			cam = GetComponent<Camera>();
		}

		void Update()
		{
			TargetingRaycast();
		}

		public void TargetingRaycast()
		{
			// Current mouse position on screen
			Vector3 mp = Input.mousePosition;

			// Current target object transform component
			Transform targetTransform = null;

			// If camera component is available
			if (cam != null)
			{
				if(Input.GetMouseButtonUp(0))
                {
					RaycastHit hitInfo;

					// Create a ray from mouse coords
					Ray ray = cam.ScreenPointToRay(new Vector3(mp.x, mp.y, 0f));

					// Targeting raycast
					if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, targetingRayLength, targetingLayerMask.value))
					{
						// Cache what we've hit
						targetTransform = hitInfo.collider.transform;
						AppFacade.GetInstance().dispatchEvent(ModuleEventType.HIGHLIGHT_OBJ_ON_TRIGGER, targetTransform);

						AppFacade.GetInstance().dispatchEvent(ModuleEventType.SWITCH_OBJ_ON_TRIGGER,targetTransform);

					}
				}
				
			}
		}
	}
}
