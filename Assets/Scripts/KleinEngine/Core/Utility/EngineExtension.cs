namespace KleinEngine
{
    public static class EngineExtension
    {
        public static string WithColor(this string srcStr, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, srcStr);
        }
    }
}
