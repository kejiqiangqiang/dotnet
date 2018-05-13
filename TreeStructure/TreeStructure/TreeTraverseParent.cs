using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeStructure
{

    public class TreeTraverseParent
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childNodeCode"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public string GetParentLocationsStr(string childNodeCode, List<Flow_Location> nodes)
        {
            string parentLocationsStr = null;
            Flow_Location childNode = nodes.Where(p => p.LocationCode == childNodeCode).FirstOrDefault();
            var parentNodes = this.GetParentLocations(childNode, nodes);
            var locationNames = parentNodes.Select(p => p.Location).ToList();
            parentLocationsStr = string.Join("-", locationNames);
            return parentLocationsStr;
        }

        /// <summary>
        /// 获取位置节点所有父节点
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="childNode"></param>
        /// <returns></returns>
        public List<Flow_Location> GetParentLocations(Flow_Location childNode, List<Flow_Location> nodes)
        {
            List<Flow_Location> parentNodes = new List<Flow_Location>();
            if (childNode != null)
            {
                //由树子节点到根节点(自下而上)顺序，则不能先加当前节点，应在加入所有父节点之后再加当前节点
                //parentNodes.Add(childNode);
                this.GetParentNodes(childNode, parentNodes, nodes);
                //由树根节点到子节点(自上而下)顺序，则不能先加当前节点，应在加入所有父节点之后再加当前节点
                parentNodes.Add(childNode);
            }
            return parentNodes;
        }

        /// <summary>
        /// 获取节点所有父节点
        /// </summary>
        /// <param name="childNode"></param>
        /// <param name="parentNodes"></param>
        /// <param name="nodes"></param>
        public void GetParentNodes(Flow_Location childNode, List<Flow_Location> parentNodes, List<Flow_Location> nodes)
        {
            if (childNode == null)
            {
                return;
            }
            var parentNode = nodes.Where(p => p.LocationCode == childNode.ParentCode).FirstOrDefault();
            if (parentNode != null)
            {
                //防止出现环导致死循环
                var pnodes = parentNodes.Select(p => p.LocationCode).ToList();
                if (pnodes.Contains(parentNode.LocationCode))
                {
                    return;
                }
                //由树子节点到根节点(自下而上)
                //parentNodes.Add(parentNode);
                this.GetParentNodes(parentNode, parentNodes, nodes);
                //由树根节点到子节点(自上而下)
                parentNodes.Add(parentNode);
            }
        }

    }
}
