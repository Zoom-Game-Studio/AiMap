using System.Collections.Generic;

namespace KleinEngine
{
    public class BehaviorTree
    {
        protected BTNode rootNode;

        public virtual bool onExecute()
        {
            if (null != rootNode) return rootNode.onExecute();
            return false;
        }

        public void setRootNode(BTNode node)
        {
            rootNode = node;
        }
    }

    public class SpecFunBehaviorTree : BehaviorTree
    {
        BlackboardData data = new BlackboardData();

        public override bool onExecute()
        {
            return base.onExecute();
        }

        public BlackboardData getBlackboardData()
        {
            return data;
        }

        public void onClear()
        {
            data.onClear();
        }
    }

    public class BlackboardData
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        public void setData(string key, object value)
        {
            data.Add(key, value);
        }

        public object getData(string key)
        {
            if (data.ContainsKey(key))
                return data[key];
            return null;
        }

        public void onClear()
        {
            data.Clear();
        }
    }
}
