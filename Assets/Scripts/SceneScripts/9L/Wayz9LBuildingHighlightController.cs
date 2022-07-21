using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;


namespace AppLogic
{
	[RequireComponent(typeof(BoxCollider))]
	public class Wayz9LBuildingHighlightController : MonoBehaviour
	{
		public float speed = 200f;

		private readonly int period = 1530;
		private float counter = 0f;

		protected HighlightableObject ho;

		private bool isSelcted = false;

		private Color startColor = Color.red;
		private Color endColor = new Color(Color.red.r, Color.red.g, Color.red.b, 0);

		private MovingUpAndDown movingSc;

		void Awake()
		{
			ho = gameObject.AddComponent<HighlightableObject>();
			AppFacade.GetInstance().addEvent(ModuleEventType.HIGHLIGHT_OBJ_ON_TRIGGER, HandleOnHighlightObjOnTrigger);

		}
		private void Start()
		{
			ho.Off();
			movingSc = GetComponent<MovingUpAndDown>();
			if (movingSc == null) return;
			movingSc.enabled = false;
		}

		void Update()
		{
			// Fade in/out constant highlighting with 'Tab' button
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				ho.ConstantSwitch();
			}
			// Turn on/off constant highlighting with 'Q' button
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				ho.ConstantSwitchImmediate();
			}

			// Turn off all highlighting modes with 'Z' button
			if (Input.GetKeyDown(KeyCode.Z))
			{
				ho.Off();
			}
			if (isSelcted) return;
			AfterUpdate();
		}


		private void HandleOnHighlightObjOnTrigger(EventObject ev)
		{
			isSelcted = !isSelcted;
			Debug.Log("HandleOnHighlightObjOnTrigger...");
			if (isSelcted)
			{
				Transform obj = ev.param as Transform;
				Debug.Log(obj.name);
				if (obj == null) return;
				if (transform.Equals(obj))
				{
					Debug.Log("HandleOnHighlightObjOnTrigger..." + obj.name);
					//ho.ConstantOnImmediate(Color.red);
					ho.FlashingOn(startColor, endColor, 2f);
					if (movingSc == null) return;
					movingSc.enabled = true;
				}
			}
			else
			{
				ho.FlashingOff();
				if (movingSc == null) return;
				movingSc.enabled = false;
			}
		}

		protected void AfterUpdate()
		{

			int val = (int)counter;
			Color col = new Color(GetColorValue(1020, val), GetColorValue(0, val), GetColorValue(510, val), 1f);

			ho.ConstantOnImmediate(col);

			counter += Time.deltaTime * speed;
			counter %= period;
		}


		float GetColorValue(int offset, int x)
		{
			int o = 0;
			x = (x - offset) % period;
			if (x < 0)
			{
				x += period;
			}

			if (x < 255)
			{
				o = x;
			}

			if (x >= 255 && x < 765)
			{
				o = 255;
			}

			if (x >= 765 && x < 1020)
			{
				o = 1020 - x;
			}

			return (float)o / 255f;
		}
	}

}