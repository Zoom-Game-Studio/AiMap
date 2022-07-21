namespace KleinEngine
{
    public class Tuple2
    {
        public int v1 { get; private set; }
        public int v2 { get; private set; }

        public void onParseData(string str)
        {
            string[] list = BaseConfigInfo.GetValueList(str);
            if(list.Length >= 2)
            {
                v1 = int.Parse(list[0]);
                v2 = int.Parse(list[1]);
            }
        }
    }

    public class LanguageConfigInfo : BaseConfigInfo
    {
        public string value { get; private set; }
    }
}
