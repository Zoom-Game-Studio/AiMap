using NRKernal;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using WeiXiang;

namespace Game
{
    public class OffSetPointer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private Transform offsetPoint;

        private Vector3ReactiveProperty offset = new Vector3ReactiveProperty();
        private Vector3 originPos = new Vector3();
        private MeshRenderer _meshRender;
        private BoolReactiveProperty _isIn = new BoolReactiveProperty();
        
        private void Awake()
        {
            originPos = offsetPoint.position;
            offset.Subscribe(v =>
            {
                LocalizationConvert.Origin.position = offset.Value;
                _textMeshPro.text = v.ToString();
            }).AddTo(this);
        }

        private void Start()
        {
            _meshRender = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            offset.Value = offsetPoint.position - originPos;
            if (_isIn)
            {
                if (NRInput.Hands.IsRunning)
                {
                    var gesture = NRInput.Hands.GetHandState(HandEnum.RightHand).currentGesture;
                    if (gesture == HandGesture.Grab)
                    {
                        offsetPoint.position = NRInput.GetPosition(ControllerHandEnum.Right);
                    }
                }
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            _meshRender.material.color = Color.green;
            _isIn.Value = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _meshRender.material.color = Color.white;
            _isIn.Value = false;
        }
    }
}