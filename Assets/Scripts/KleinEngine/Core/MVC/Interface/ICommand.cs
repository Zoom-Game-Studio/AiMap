using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleinEngine
{
    public interface ICommand
    {
        void onExecute(object param);
    }
}
