using KleinEngine;
using System.IO;
using UnityEngine;

namespace AppLogic
{
    public class ShutdownCommand : BaseCommand
    {
        public override void onExecute(object param)
        {
            //Singleton<NetworkManager>.GetInstance().onDestroy();
        }
    }
}
