using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mopsicus.TwinSlider {

	/// <summary>
	/// Twin slider with border limit
	/// </summary>
	public class TwinSlider : MonoBehaviour {

		/// <summary>
		/// Callback on slider change 滑块更改时的回调
		/// </summary>
		public Action<float, float> OnSliderChange;

		/// <summary>
		/// First slider 第一滑块
		/// </summary>
		[SerializeField]
		public Slider SliderOne;

		/// <summary>
		/// Second slider 第二滑块
		/// </summary>
		[SerializeField]
		public Slider SliderTwo;

		/// <summary>
		/// Background image 背景图片
		/// </summary>
		[SerializeField]
		private Image Background;

		/// <summary>
		/// Filler between sliders 滑块之间的填充物
		/// </summary>
		[SerializeField]
		private Image Filler;

		/// <summary>
		/// Filler color
		/// </summary>
		[SerializeField]
		private Color Color;

		/// <summary>
		/// Min sliders value
		/// </summary>
		public float Min = 0f;

		/// <summary>
		/// Max sliders value
		/// </summary>
		public float Max = 1f;

		/// <summary>
		/// Border limit between sliders 滑块之间的边界限制
		/// </summary>
		public float Border = 0.01f;

		/// <summary>
		/// Filler rect cache 填充矩形缓存
		/// </summary>
		private RectTransform _fillerRect;

		/// <summary>
		/// Half of common slider width 普通滑块宽度的一半
		/// </summary>
		public float _width;

		/// <summary>
		/// Constructor
		/// </summary>
		private void Awake () {
			_fillerRect = Filler.GetComponent<RectTransform> ();
			_width = GetComponent<RectTransform> ().sizeDelta.x / 2f;
			SliderOne.minValue = Min;
			SliderOne.maxValue = Max;
			SliderTwo.minValue = Min;
			SliderTwo.maxValue = Max;
			Filler.color = Color;
			if (OnSliderChange == null) {
				OnSliderChange += delegate { };
			}
		}

		/// <summary>
		/// Callback on first slider change 第一次滑块更改时的回调
		/// </summary>
		public void OnCorrectSliderOne (float value) {
			DrawFiller (SliderOne.handleRect.localPosition, SliderTwo.handleRect.localPosition);
			if (value > SliderTwo.value - Border) {
				SliderOne.value = SliderTwo.value - Border;
			} else {
				OnSliderChange.Invoke (SliderOne.value, SliderTwo.value);
			}
		}

		/// <summary>
		/// Callback on second slider change 更改第二个滑块时回调
		/// </summary>
		public void OnCorrectSliderTwo (float value) {
			DrawFiller(SliderOne.handleRect.localPosition, SliderTwo.handleRect.localPosition);
			if (value < SliderOne.value + Border) {
				SliderTwo.value = SliderOne.value + Border;
			} else {
				OnSliderChange.Invoke (SliderOne.value, SliderTwo.value);
			}
		}

		/// <summary>
		/// Draw filler
		/// </summary>
		/// 滑块手柄的坐标
		/// <param name="one">Coords of first slider handle</param>
		/// <param name="two">Coords of second slider handle</param>
		void DrawFiller (Vector3 one, Vector3 two) {
			float left = Mathf.Abs (_width + one.x);
			float right = Mathf.Abs (_width - two.x);
			_fillerRect.offsetMax = new Vector2 (-right, 0f);
			_fillerRect.offsetMin = new Vector2 (left, 0f);
		}
	}
}