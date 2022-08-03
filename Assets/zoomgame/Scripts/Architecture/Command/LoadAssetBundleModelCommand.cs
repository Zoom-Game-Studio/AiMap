using QFramework;

namespace Architecture.Command
{
    public class LoadAssetBundleModelCommand : AbstractCommand
    {
        private readonly string _tileName;

        public LoadAssetBundleModelCommand(string tileName)
        {
            this._tileName = tileName;
        }


        protected override void OnExecute()
        {
        }
    }
}