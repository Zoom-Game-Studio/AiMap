using System.Collections.Generic;

namespace KleinEngine
{
    public abstract class BTNode
    {
        public virtual bool onExecute()
        {
            return true;
        }
    }

    public class ActionNode : BTNode
    {

    }

    public class ConditionNode : BTNode
    {

    }

    public class CompositeNode : BTNode
    {
        protected List<BTNode> childNodeList = new List<BTNode>();

        public void addNode(BTNode node)
        {
            childNodeList.Add(node);
        }

    }

    /**
     * 说明：并行复合节点
     */

    public class ParallelNode : CompositeNode
    {
        public override bool onExecute()
        {
            bool result = false;
            foreach (BTNode node in childNodeList)
            {
                if (node.onExecute()) result = true;
            }
            return result;
        }
    }

    /**
     * 说明：选择复合节点
     */

    public class SelectorNode : CompositeNode
    {
        public override bool onExecute()
        {
            int len = childNodeList.Count;
            for (int i = 0; i < len; i++)
            {
                BTNode node = childNodeList[i];
                if (null == node) continue;
                if (node.onExecute()) return true;
            }
            return false;
        }
    }

    /**
     * 说明：顺序复合节点
     */

    public class SequenceNode : CompositeNode
    {
        public override bool onExecute()
        {
            int len = childNodeList.Count;
            for (int i = 0; i < len; i++)
            {
                BTNode node = childNodeList[i];
                if (null == node) continue;
                if (!node.onExecute()) return false;
            }
            return true;
        }
    }
}
