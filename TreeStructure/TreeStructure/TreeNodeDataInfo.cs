using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeStructure
{

    #region 树结构基类
    /// <summary>
    /// 树数据结构
    /// </summary>
    public class Tree
    {
        public List<Tree> Children { get; set; }

        public Tree()
        {
            this.Children = new List<Tree>();
        }
        public string NodeCode { get; set; }
        public string NodeText { get; set; }
        public string ParentCode { get; set; }
        /// <summary>
        /// 是否满足某种条件(是否拥有权限即是否勾选、是否为项目(地区、项目))
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 以当前节点为根的子树叶子节点并且IsChecked=true的叶子节点数
        /// </summary>
        public int CurrentTreeLeafNodesIsCheckedCount { get; set; }
        /// <summary>
        /// 当前节点孩子节点数
        /// </summary>
        public int ChildrenCount { get; set; }
        /// <summary>
        /// 以当前节点为根的子树叶子节点数
        /// </summary>
        public int CurrentTreeLeafNodesCount { get; set; }
        /// <summary>
        /// 以当前节点为根的子树节点数(包括当前节点即包括根节点)
        /// </summary>
        public int CurrentTreeNodesCount { get; set; }
    }

    #endregion

    /// <summary>
    /// 地区项目树
    /// </summary>
    public class RegionProjectTree : Tree
    {
        public RegionProjectTree()
        {
            //A继承B，A:B，但是List<A>、List<B>的类型为泛型List<T>，List<A>、List<B>并无继承关系
            this.Children = new List<RegionProjectTree>().Cast<Tree>().ToList();//1、其实是复制操作
        }

        //public RegionProjectTree()
        //{
        //    this.Children = new List<RegionProjectTree>();//0、类型无法转换
        //}

        //public List<RegionProjectTree> Children { get; set; }//2、覆盖

        public decimal? Longtitude { get; set; }
        public decimal? Latitude { get; set; }


    }
    public class TreeNodeDataInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [BusinessMethod]
        public List<RegionProjectTree> getTreesNodeDataInfo()
        {
            var rootNodes = managements.Where(p => p.ParentCode == null).Select(p => new RegionProjectTree() { NodeCode = p.MCode, NodeText = p.MName, ParentCode = p.ParentCode }).ToList();
            var childNodes = managements.Where(p => p.ParentCode != null).Select(p => new RegionProjectTree() { NodeCode = p.MCode, NodeText = p.MName, ParentCode = p.ParentCode }).ToList();
            var projectNodes = projects.AsEnumerable().Select(p => new RegionProjectTree() { NodeCode = p.ProjectCode.ToString(), NodeText = p.ProjectName, ParentCode = p.MCode, Longtitude = p.Longitude, Latitude = p.Latitude }).ToList();
            childNodes.AddRange(projectNodes);
            var checkNodes = projectNodes;//是否为项目
            var trees = this.GetTrees(rootNodes, childNodes, checkNodes);
            return tress;
        }

        #region 直接使用类--发现使用模板方法过程
        #region
        /// <summary>
        /// 构建树--从根节点开始深度优先遍历(先序遍历)节点递归构建(树的生成顺序是后序遍历(递归栈返回，栈先进后出即后调用先返回))(先序、后序说法把树的右子树视为空)各棵树即森林
        /// </summary>
        /// <param name="rootNodes"></param>
        /// <param name="childNodes"></param>
        /// <param name="checkNodes"></param>
        /// <returns></returns>
        public List<RegionProjectTree> GetTrees(List<RegionProjectTree> rootNodes, List<RegionProjectTree> childNodes, List<RegionProjectTree> checkNodes)
        {
            List<RegionProjectTree> trees = new List<RegionProjectTree>();
            foreach (var rootNode in rootNodes)
            {
                var isCheacked = checkNodes.Where(p => p.NodeCode == rootNode.NodeCode).FirstOrDefault() != null;
                //Tree root = new RegionProjectTree() {  NodeCode = rootNode.NodeCode, NodeText = rootNode.NodeText, IsChecked = isCheacked};//rootNodes在方法外构造并传入，这里可以直接用
                rootNode.IsChecked = isCheacked;//rootNodes在方法外构造并传入，这里可以直接用
                //根节点
                trees.Add(rootNode);
                //子树
                this.AddChildren(rootNode, childNodes, checkNodes);
                #region
                ////树构建完成后,获取树根的叶子节点以及子树节点数
                //rootNode.CurrentTreeLeafNodesCount = rootNode.Children.Sum(p=>p.CurrentTreeLeafNodesCount);//由于构建树过程为先序遍历，即先根遍历，树根的叶子节点以及子树节点数无法在建树时获取(先根节点后子节点)//在构建完树之后再获取树根的叶子节点以及子树节点数
                //上述有误
                //树的生成顺序是后序遍历
                #endregion
            }
            return trees;
        }

        /// <summary>
        /// 构建树--从根节点开始深度优先遍历(先序遍历)节点递归构建树(树的生成顺序是后序遍历(递归栈返回，栈先进后出即后调用先返回))(先序、后序说法把树的右子树视为空)
        /// </summary>
        /// <param name="currentRootNode"></param>
        /// <param name="childNodes"></param>
        /// <param name="checkNodes"></param>
        private void AddChildren(Tree currentRootNode, List<RegionProjectTree> childNodes, List<RegionProjectTree> checkNodes)
        {
            var currentRootChildrenNodes = childNodes.Where(p => p.ParentCode == currentRootNode.NodeCode).ToList();
            foreach (var node in currentRootChildrenNodes)
            {
                var isCheacked = checkNodes.Where(p => p.NodeCode == node.NodeCode).FirstOrDefault() != null;
                node.IsChecked = isCheacked;
                //当前根节点currentRootNode节点的孩子节点node节点作为子树根节点递归构建子树
                this.AddChildren(node, childNodes, checkNodes);//直到叶子节点递归返回
                //当前根节点currentRootNode节点的孩子节点node节点作为子树根节点递归返回后(此node的子树生成完毕)
                currentRootNode.Children.Add(node);
                //currentRootNode.ChildrenCount += 1;//1、ChildrenCount
                #region
                //currentRootNode.CurrentTreeLeafNodesCount = //由于构建树过程为先序遍历，即先根遍历，树根的叶子节点以及子树节点数无法在建树时获取(先根节点后子节点)//在构建完树之后再获取树根的叶子节点以及子树节点数
                //上述有误
                //树的生成顺序是后序遍历
                #endregion
            }
            currentRootNode.ChildrenCount = currentRootNode.Children.Count;//2、ChildrenCount
            //currentRootNode.ChildrenCount = currentRootChildrenNodes.Count;//3、ChildrenCount
            #region
            //currentRootNode.CurrentTreeLeafNodesCount = //由于构建树过程为先序遍历，即先根遍历，树根的叶子节点以及子树节点数无法在建树时获取(先根节点后子节点)//在构建完树之后再获取树根的叶子节点以及子树节点数
            //上述有误
            //树的生成顺序是后序遍历
            #endregion
            currentRootNode.CurrentTreeLeafNodesCount = currentRootNode.ChildrenCount > 0 ? currentRootNode.Children.Sum(p => p.CurrentTreeLeafNodesCount) : 1;//当前节点叶子节点(ChildrenCount、currentRootChildrenNodes均可判断)时为1，否则为孩子节点的叶子节点数相加
            currentRootNode.CurrentTreeLeafNodesIsCheckedCount = currentRootNode.ChildrenCount > 0 ? currentRootNode.IsChecked ? currentRootNode.Children.Sum(p => p.CurrentTreeLeafNodesIsCheckedCount) + 1 ://有孩子并且IsChecked==true情况
                currentRootNode.Children.Sum(p => p.CurrentTreeLeafNodesIsCheckedCount) : currentRootNode.IsChecked ? 1 : 0;//当前节点叶子节点并且IsChecked==true的叶子节点
            currentRootNode.CurrentTreeNodesCount = currentRootNode.Children.Sum(p => p.CurrentTreeNodesCount) + 1;//子树节点数+根节点1
        }
        #endregion
        #endregion

        #region 使用模板方法--AOP初始化数据上下文时，不能有泛型--单独拿出去新建一个类应该可以
        //#region
        ///// <summary>
        ///// 构建树--从根节点开始先序遍历节点递归构建(树的生成顺序是后序遍历(递归栈返回，栈先进后出即后调用先返回))(先序、后序说法把树的右子树视为空)各棵树即森林
        ///// </summary>
        ///// <param name="rootNodes"></param>
        ///// <param name="childNodes"></param>
        ///// <param name="checkNodes"></param>
        ///// <returns></returns>
        //public List<T> GetTrees<T>(List<T> rootNodes, List<T> childNodes, List<T> checkNodes) where T:Tree
        //{
        //    List<T> trees = new List<T>();
        //    foreach (var rootNode in rootNodes)
        //    {
        //        var isCheacked = checkNodes.Where(p => p.NodeCode == rootNode.NodeCode).FirstOrDefault() != null;
        //        //Tree root = new RegionProjectTree() {  NodeCode = rootNode.NodeCode, NodeText = rootNode.NodeText, IsChecked = isCheacked};//rootNodes在方法外构造并传入，这里可以直接用
        //        rootNode.IsChecked = isCheacked;//rootNodes在方法外构造并传入，这里可以直接用
        //        //根节点
        //        trees.Add(rootNode);
        //        //子树
        //        this.AddChildren(rootNode, childNodes, checkNodes);
        //        #region
        //        ////树构建完成后,获取树根的叶子节点以及子树节点数
        //        //rootNode.CurrentTreeLeafNodesCount = rootNode.Children.Sum(p=>p.CurrentTreeLeafNodesCount);//由于构建树过程为先序遍历，即先根遍历，树根的叶子节点以及子树节点数无法在建树时获取(先根节点后子节点)//在构建完树之后再获取树根的叶子节点以及子树节点数
        //        //上述有误
        //        //树的生成顺序是后序遍历
        //        #endregion
        //    }
        //    return trees;
        //}

        ///// <summary>
        ///// 构建树--从根节点开始先序遍历节点递归构建树(树的生成顺序是后序遍历(递归栈返回，栈先进后出即后调用先返回))(先序、后序说法把树的右子树视为空)
        ///// </summary>
        ///// <param name="currentRootNode"></param>
        ///// <param name="childNodes"></param>
        ///// <param name="checkNodes"></param>
        //private void AddChildren<T>(Tree currentRootNode, List<T> childNodes, List<T> checkNodes) where T:Tree
        //{
        //    var currentRootChildrenNodes = childNodes.Where(p => p.ParentCode == currentRootNode.NodeCode).ToList();
        //    foreach (var node in currentRootChildrenNodes)
        //    {
        //        var isCheacked = checkNodes.Where(p => p.NodeCode == node.NodeCode).FirstOrDefault() != null;
        //        node.IsChecked = isCheacked;
        //        //当前根节点currentRootNode节点的孩子节点node节点作为子树根节点递归构建子树
        //        this.AddChildren(node, childNodes, checkNodes);//直到叶子节点递归返回
        //        //当前根节点currentRootNode节点的孩子节点node节点作为子树根节点递归返回后(此node的子树生成完毕)
        //        currentRootNode.Children.Add(node);
        //        //currentRootNode.ChildrenCount += 1;//ChildrenCount
        //        #region
        //        //currentRootNode.CurrentTreeLeafNodesCount = //由于构建树过程为先序遍历，即先根遍历，树根的叶子节点以及子树节点数无法在建树时获取(先根节点后子节点)//在构建完树之后再获取树根的叶子节点以及子树节点数
        //        //上述有误
        //        //树的生成顺序是后序遍历
        //        #endregion
        //    }
        //    currentRootNode.ChildrenCount = currentRootNode.Children.Count;//ChildrenCount
        //    //currentRootNode.ChildrenCount = currentRootChildrenNodes.Count;//ChildrenCount
        //    #region
        //    //currentRootNode.CurrentTreeLeafNodesCount = //由于构建树过程为先序遍历，即先根遍历，树根的叶子节点以及子树节点数无法在建树时获取(先根节点后子节点)//在构建完树之后再获取树根的叶子节点以及子树节点数
        //    //上述有误
        //    //树的生成顺序是后序遍历
        //    #endregion
        //    currentRootNode.CurrentTreeLeafNodesCount = currentRootNode.ChildrenCount > 0 ? currentRootNode.Children.Sum(p => p.CurrentTreeLeafNodesCount) : 1;//当前节点叶子节点(ChildrenCount、currentRootChildrenNodes均可判断)时为1，否则为孩子节点的叶子节点数相加
        //    currentRootNode.CurrentTreeNodesCount = currentRootNode.Children.Sum(p => p.CurrentTreeNodesCount) + 1;//子树节点数+根节点1
        //}
        //#endregion
        #endregion


        /// <summary>
        /// 总能耗及费用实体
        /// </summary>
        public class ProjectEnergy
        {
            public int ProjectCode { get; set; }
            public decimal DifData { get; set; }
            public decimal DifFee { get; set; }
        }

       
    }

}
