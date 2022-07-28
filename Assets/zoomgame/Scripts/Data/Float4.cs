namespace HttpData
{
    public struct Float4
    {
        public float Number0;
        public float Number1;
        public float Number2;
        public float Number3;

        /// <summary>
        /// 测试示例的相机物理数据
        /// </summary>
        public static Float4 intrinsic => new Float4(494.4375f, 498.5292f, 319.8089f, 241.4202f);
        
        public Float4(float number0 = 0,float number1 = 0,float number2 = 0,float number3 = 0)
        {
            Number0 = number0;
            Number1 = number1;
            Number2 = number2;
            Number3 = number3;
        }

        public override string ToString()
        {
            var arr = new float[]{Number0,Number1,Number2,Number3};
            return Newtonsoft.Json.JsonConvert.SerializeObject(arr);
        }
    }
}