using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeStructure
{

    public class CommentUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PortraitUrl { get; set; }
    }

    /// <summary>
    /// 评论、回复树结构
    /// </summary>
    public class Comments
    {
        /// <summary>
        /// 构造方法实例化Children字段，在使用pNode.Children.Add(node)之前，省去new的操作;
        /// </summary>
        public Comments()
        {
            this.Children = new List<Comments>();
        }
        public int ID { get; set; }
        public CommentUser CommentUser { get; set; }
        /// <summary>
        /// 评论时为null、回复时为回复用户//并且前端依此判断是评论还是回复
        /// </summary>
        public CommentUser ToReplyUser { get; set; }

        public string Comment { get; set; }
        public string CreateTime { get; set; }
        public List<Comments> Children { get; set; }
        public int? ParentID { get; set; }

    }

    public class CommentBLL //: BaseBusiness
    {
        //Acc_VipBriefCommentDao vipBriefCommentDao { get; set; }

        //VIPBriefProjectCommentBLL bll_prj = new VIPBriefProjectCommentBLL();

        /// <summary>
        /// 新增评论、回复
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="fileKey"></param>
        /// <param name="parentID"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [BusinessMethod]
        public bool addComment(int userID, string fileKey, int? parentID, string comment)
        {
            var model = new Acc_VipBriefComment();
            model.UserID = userID;
            model.FileKey = fileKey;
            model.ParentID = parentID;
            model.Comment = comment;
            model.CreateTime = DateTime.Now;
            vipBriefCommentDao.Add(model);
            return true;
        }

        /// <summary>
        /// 删除自己的评论、回复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [BusinessMethod]
        public bool delete(int id, int userID)
        {
            //防止误删//前端当前用户点击与评论、回复用户相同的用户时，弹出是否删除评论、回复
            if (vipBriefCommentDao.Entity.Where(p => p.ID == id && p.UserID == userID).Count() > 0)
            {
                vipBriefCommentDao.Delete(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取评论及其回复
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        [BusinessMethod]
        public IList getComment(string fileKey, out int count)
        {
            string masterUrl = ConfigurationManager.AppSettings["PassportUri"];
            var userInfos = this.MasterRedisConfig.Acc_UserInfo;
            //评论回复数据//评论回复同表回复也保存帖子id更高效//即便是评论回复分表，回复表也最好保存帖子id，否则对于一个评论，要去回复表所有回复中根据ParentID找到该评论的回复以及其下回复的回复，效率低
            var commentsreplys = vipBriefCommentDao.Entity.Where(p => p.FileKey == fileKey).ToList().Select(p => new Comments
            {
                ID = p.ID,
                CommentUser = new CommentUser
                {
                    UserID = p.UserID,
                    UserName = userInfos.Where(pp => pp.UserID == p.UserID).Select(pp => pp.UserName).FirstOrDefault(),
                    PortraitUrl = userInfos.Where(pp => pp.UserID == p.UserID && pp.HeadPortrait != null && pp.HeadPortrait != "").Select(pp => masterUrl + pp.HeadPortrait.Replace("~", "")).FirstOrDefault()
                },
                Comment = p.Comment,
                CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ParentID = p.ParentID
            }).ToList();


            //获取树
            var trees = CommentBLL.getCommentTress(commentsreplys);
            //遍历树
            var rs = CommentBLL.getCommentTressPreOrderTravel(trees, out count);

            return rs;
        }

        /// <summary>
        /// 遍历树
        /// </summary>
        /// <param name="trees"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IList getCommentTressPreOrderTravel(List<Comments> trees, out int count)
        {
            //树形结构展示则直接返回trees即可，这里评论回复按照qq按列表有序显示，故遍历保存到数组后返回，前端不用判断，也可以直接返回trees，前端判断从属关系及其位置，要不要树形显示均可//以下遍历前后端做均可(前端循环，后端涉及到多叉树的深度优先遍历(先序遍历))
            //将森林所有节点按照深度优先遍历(先序遍历)有序保存到数组
            List<Comments> allNodes = new List<Comments>();
            //遍历森林
            foreach (var tree in trees)
            {
                //评论回复列表--多叉树的深度优先遍历(前序遍历)
                CommentBLL.preOrderTraver(tree, allNodes);
            }
            //return allNodes;

            var rs = allNodes.Select(p => new
            {
                p.ID,
                CommentUserID = p.CommentUser.UserID,
                CommentUserName = p.CommentUser.UserName,
                CommentUserPortrait = p.CommentUser.PortraitUrl,
                ToReplyUserID = p.ToReplyUser == null ? new Nullable<int>() : p.ToReplyUser.UserID,
                ToReplyUserName = p.ToReplyUser == null ? null : p.ToReplyUser.UserName,
                ToReplyUserNamePortrait = p.ToReplyUser == null ? null : p.ToReplyUser.PortraitUrl,
                p.Comment,
                p.CreateTime
            }).ToList();
            //评论数量（不计回复）
            count = rs.Where(p => !p.ToReplyUserID.HasValue).Count();
            return rs;
        }

        /// <summary>
        /// 获取评论及其回复树结构
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public static List<Comments> getCommentTress(List<Comments> commentsreplys)
        {
            //评论(ParentID==null)
            var comments = commentsreplys.Where(p => !p.ParentID.HasValue).OrderBy(p => Convert.ToDateTime(p.CreateTime)).ToList();
            //回复(ParentID!=null)
            var replys = commentsreplys.Where(p => p.ParentID.HasValue).ToList();

            //评论回复森林
            List<Comments> trees = new List<Comments>();
            //由于评论与回复表同一表中，因此评论作为评论回复树的根节点(评论作为评论回复树根节点)//并且评论的评论ParentID、评论的回复ParentID编号也避免了ParentID不明确冲突的可能
            comments.ForEach(p =>
            {

                //深度遍历(前序遍历)递归调用建树
                trees.Add(p);
                CommentBLL.addChildren(p, replys);

            });
            return trees;
        }

        /// <summary>
        /// 深度遍历(先序遍历)递归调用建树-->节点访问顺序<===>节点生成(实例化、赋值)顺序-->后序遍历
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="replys"></param>
        public static void addChildren(Comments pNode, List<Comments> replys)
        {
            List<Comments> nodes = replys.Where(p => p.ParentID == pNode.ID).OrderBy(p => Convert.ToDateTime(p.CreateTime)).ToList();
            foreach (var p in nodes)
            {
                p.ToReplyUser = p.CommentUser;
                CommentBLL.addChildren(p, replys);
                pNode.Children.Add(p);
            }

        }

        /// <summary>
        /// 多叉树(深度遍历)先序遍历，将各节点有序保存到数组（拼接JSON字符串）
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="allSingleNodes"></param>
        public static void preOrderTraver(Comments tree, List<Comments> allSingleNodes)
        {
            //String result = "";//"{" + "id : '" + id + "'" + ", text : '" + text + "'";
            var node = new Comments()
            {
                ID = tree.ID,
                CommentUser = tree.CommentUser,
                ToReplyUser = tree.ToReplyUser,
                Comment = tree.Comment,
                CreateTime = tree.CreateTime,
                Children = null,
            };
            allSingleNodes.Add(node);
            if (tree.Children.Count != 0)
            {

                for (int i = 0; i < tree.Children.Count; i++)
                {
                    //result += ((Node)children.get(i)).toString() + ",";
                    CommentBLL.preOrderTraver(tree.Children[i], allSingleNodes);
                }
                //result = result.substring(0, result.length() - 1);
                //result += "]";
            }
            //else
            //{
            //    result += ", leaf : true";
            //}
            //return result + "}";
            return;
        }

    }
}
