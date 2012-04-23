using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Model.ViewModel;
using Kt.GameGroup.Model.Enums;
using Kt.GameGroup.Data;
using Kt.Framework.Linq;
using Kt.Framework.Repository.Data;

namespace Kt.GameGroup.Services
{
    public class GroupBBsServices
    {
        private IRepository<group_bbs> GroupbbsRepository;
        private IRepository<group_bbsSort> GroupbbsSortRepository;
        private IRepository<group_bbsReply> GroupbbsReplyRepository;
        private GroupServices GroupServices;
        private GroupMeberServices GroupMeberServices;
        public GroupBBsServices(GroupServices GroupServices, IRepository<group_bbs> GroupbbsRepository, IRepository<group_bbsSort> GroupbbsSortRepository, IRepository<group_bbsReply> GroupbbsReplyRepository, GroupMeberServices GroupMeberServices)
        {
            this.GroupbbsRepository = GroupbbsRepository;
            this.GroupbbsSortRepository = GroupbbsSortRepository;
            this.GroupbbsReplyRepository = GroupbbsReplyRepository;
            this.GroupServices = GroupServices;
            this.GroupMeberServices = GroupMeberServices;
        }
        /// <summary>
        /// 获取某一游戏团内主题帖列表
        /// </summary>
        /// <param name="gid">游戏团编号</param>
        /// <param name="publishid">团主自定义板块编号,如查询全部则为-1</param>
        /// <param name="postSortId">帖子系统分类编号,如查询全部则为-1</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public ViewThreadInfo GroupBBSInfos(int gid, int publishid = -1, int postSortId = -1, int pageNo = 1, int pageSize = 20)
        {

            var predicate = PredicateBuilder.True<group_bbs>();

            predicate = predicate.And(x => x.gId == gid);

            if (publishid != -1)
            {
                predicate = predicate.And(x => x.publishId == publishid);
            }

            if (postSortId != -1)
            {
                predicate = predicate.And(x => x.postSortId == postSortId);
            }


            var count = this.GroupbbsRepository.Get().Where(predicate).Count();

            //var datas = this.GroupbbsRepository.Get().Where(predicate).OrderByDescending(x => x.bbsId).OrderByDescending(x => x.isTop).Skip((pageNo - 1) * pageSize).Take(pageSize);

            //var dataaaa = from this.GroupbbsRepository.Table  in BBS
            //     where 

            var xxxx = from BBS in this.GroupbbsRepository.Get()
                       orderby BBS.isTop descending, BBS.bbsId descending
                       select BBS;


            var datas = xxxx.Where(predicate).Skip((pageNo - 1) * pageSize).Take(pageSize);

            var data = new ViewThreadInfo
            {
                count = count,
                PageNo = pageNo,
                PageSize = pageSize,
                List = datas
            };

            return data;
        }




        /// <summary>
        /// 发表主题
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="uId">发布者编号</param>
        /// <param name="nickName">发布者昵称</param>
        /// <param name="title">发布标题</param>
        /// <param name="content">发布内容</param>
        /// <param name="postSort">帖子系统分类编号</param>
        /// <param name="publishId">团主自定义板块编号</param>
        /// <returns></returns>
        public int CreateGroupBBSInfo(int gId, decimal uId, string nickName, string title, string content, PublishType publishType, int sortId = 0)
        {
            var ut = this.GroupServices.CheckManageRight(gId, uId);

            if (ut == UserType.游客 || ut == UserType.申请中成员) return -1;

            var retdata = this.GroupbbsRepository.Add(new group_bbs
            {
                clickNum = 0,
                content = content,
                gId = gId,
                isLight = false,
                isTop = false,
                isPith = false,
                lastNickName = nickName,
                lastTime = System.DateTime.Now,
                lastUid = uId,
                nickName = nickName,
                postSortId = sortId,
                replyNum = 0,
                publishId = (int)publishType,
                sendTime = System.DateTime.Now,
                title = title,
                uId = uId
            });

            this.GroupServices.ChangeGroupPoint((int)gId, GameGroupPointType.发新帖子);
            this.GroupServices.ModifyGroupPostNum((int)gId, 1, 1);
            this.GroupMeberServices.updatePostCount((int)gId, uId, 1);
            return retdata.bbsId;

        }

        /// <summary>
        ///修改主题帖
        /// </summary>
        /// <param name="bbsId">帖子编号</param>
        /// <param name="title">帖子标题</param>
        /// <param name="content">帖子内容</param>
        /// <param name="publishId">团主自定义板块编号</param>
        /// <returns></returns>
        public int ModifyGroupBBSInfo(int bbsId, string title, string content, int publishId, decimal uid)
        {
            var bbsdata = this.GroupbbsRepository.Get(bbsId);
            if (bbsdata == null) return -1;

            var gid = (int)bbsdata.gId;
            var ut = this.GroupServices.CheckManageRight((int)bbsdata.gId, uid);

            if (ut != UserType.副团长 && ut != UserType.团长) return -1;

            bbsdata.title = title;
            bbsdata.content = content;
            bbsdata.publishId = publishId;
            this.GroupbbsRepository.Update(bbsdata);

            return gid;
        }

        /// <summary>
        /// 按主题帖编号删除主题帖
        /// </summary>
        /// <param name="bbsId">帖子编号</param>
        /// <returns></returns>
        public int DeleteGroupBBSInfo(int bbsId, decimal uid)
        {
            var bbsdata = this.GroupbbsRepository.Get(bbsId);
            if (bbsdata == null) return -1;
            var gid = bbsdata.gId;
            var ut = this.GroupServices.CheckManageRight((int)bbsdata.gId, uid);

            if (ut != UserType.副团长 && ut != UserType.团长) return -1;

            var replaylist = this.GroupbbsReplyRepository.Where(x => x.bbsId == bbsId);

            this.GroupbbsReplyRepository.Delete(replaylist);
            GroupServices.ChangeGroupPoint((int)gid, GameGroupPointType.发表回复, 1, replaylist.Count());

            this.GroupbbsRepository.Delete(bbsdata);
            GroupServices.ChangeGroupPoint((int)gid, GameGroupPointType.发新帖子, 1);

            this.GroupServices.ModifyGroupPostNum((int)gid, 1, 2);
            this.GroupMeberServices.updatePostCount((int)gid, uid, 2);

            return (int)gid;

        }

        /// <summary>
        /// 获取某一帖子的回复列表
        /// </summary>
        /// <param name="bbsId">帖子编号</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public ViewThreadModel GetGroupBBSReplysById(int bbsId, int pageNo = 1, int pageSize = 20, bool addClickNum = false)
        {
            var bbsdata = this.GroupbbsRepository.Get(bbsId);

            if (bbsdata == null) return null;

            var count = this.GroupbbsReplyRepository.Get().Where(x => x.bbsId == bbsId).Count();
            var replaydata = this.GroupbbsReplyRepository.Get().Where(x => x.bbsId == bbsId).OrderBy(x => x.replyId).Skip(pageSize * (pageNo - 1)).Take(pageSize);

            var model = new ViewThreadModel { BBSInfo = bbsdata, count = count, PageNo = pageNo, PageSize = pageSize, ReplyList = replaydata };


            if (addClickNum)
            {
                bbsdata.clickNum++;
                this.GroupbbsRepository.Update(bbsdata);
            }

            return model;

        }

        /// <summary>
        /// 添加对某一帖子的回复内容
        /// </summary>
        /// <param name="bbsId">帖子编号</param>
        /// <param name="uid">回复人编号</param>
        /// <param name="nickName">回复人昵称</param>
        /// <param name="content">回复内容</param>
        /// <returns></returns>
        public bool CreateGroupBBSReplyInfo(int bbsId, decimal uid, string nickName, string content)
        {
            var data = this.GroupbbsRepository.Get(bbsId);

            if (data == null) return false;

            var ut = this.GroupServices.CheckManageRight((int)data.gId, uid);

            if (ut == UserType.游客 || ut == UserType.申请中成员) return false;


            this.GroupbbsReplyRepository.Add(new group_bbsReply { bbsId = bbsId, content = content, repTime = System.DateTime.Now, nickName = nickName, uId = uid });
            data.lastNickName = nickName;
            data.lastTime = System.DateTime.Now;
            data.lastUid = uid;
            data.replyNum++;

            this.GroupbbsRepository.Update(data);

            this.GroupServices.ChangeGroupPoint((int)data.gId, GameGroupPointType.发表回复);

            return true;

        }
        /// <summary>
        /// 对回复内容的修改
        /// </summary>
        /// <param name="replyId">回复编号</param>
        /// <param name="content">回复内容</param>
        /// <returns></returns>
        public bool ModifyGroupBBSReplyInfo(int replyId, string content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除某一回复内容
        /// </summary>
        /// <param name="replyId">回复编号</param>
        /// <returns></returns>
        public bool DeleteGroupBBSReplyInfo(int replyId, decimal uid)
        {
            //throw new NotImplementedException();

            var replay = this.GroupbbsReplyRepository.Get(replyId);

            if (replay == null) return false;

            var data = this.GroupbbsRepository.Get(replay.bbsId);

            if (data == null) return false;

            var ut = this.GroupServices.CheckManageRight((int)data.gId, uid);

            if (ut == UserType.游客 || replay.uId != uid) return false;

            this.GroupbbsReplyRepository.Delete(replay);
            GroupServices.ChangeGroupPoint((int)data.gId, GameGroupPointType.发表回复, 1);

            data.replyNum--;

            this.GroupbbsRepository.Update(data);

            return true;
        }

        /// <summary>
        /// 给游戏团添加自定义分组
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="showXh">显示序号,越大越靠前</param>
        /// <param name="sortName">分类名称</param>
        /// <returns></returns>
        public bool CreateGroupbbsSortInfo(int gId, int showXh, string sortName)
        {
            this.GroupbbsSortRepository.Add(new group_bbsSort { gId = gId, showXh = showXh, sortName = sortName });
            return true;
        }
        /// <summary>
        /// 修改游戏团的自定义分类
        /// </summary>
        /// <param name="sortId">分类编号</param>
        /// <param name="showXh">显示序号</param>
        /// <param name="sortName">分类名称</param>
        /// <returns></returns>
        public bool ModifyGroupbbsSortInfo(int sortId, int showXh, string sortName)
        {

            var item = this.GroupbbsSortRepository.Get(sortId);
            if (item == null) return false;

            item.showXh = showXh;
            item.sortName = sortName;

            this.GroupbbsSortRepository.Update(item);

            return true;

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取游戏团自定义分类
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        public IEnumerable<group_bbsSort> GetGroupbbsSortInfosByGroupId(int gId)
        {
            return this.GroupbbsSortRepository.Where(x => x.gId == gId);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="deleteable"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public bool BatDeletebbsSort(int[] deleteable, int gid)
        {
            foreach (var deleteid in deleteable)
            {
                var gbsinfo = this.GroupbbsSortRepository.Get(deleteid);

                if (gbsinfo == null) continue;

                if (gbsinfo.gId != gid) continue;

                this.GroupbbsSortRepository.Delete(gbsinfo);
            }

            return true;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="sortids"></param>
        /// <param name="displayorders"></param>
        /// <param name="names"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public bool BatUpdateBbsSort(int[] sortids, string[] displayorders, string[] names, int gid)
        {
            //忽略
            if (sortids == null || displayorders == null || names == null) return true;
            //忽略
            if (sortids.Length != displayorders.Length || sortids.Length != names.Length) return true;

            for (var i = 0; i < sortids.Length; i++)
            {
                var item = this.GroupbbsSortRepository.Get(sortids[i]);
                if (item == null) continue;

                if (item.gId != gid) continue;

                if (string.IsNullOrEmpty(names[i])) continue;

                int order;
                int.TryParse(displayorders[i], out order);

                item.showXh = order;
                item.sortName = names[i];

                this.GroupbbsSortRepository.Update(item);
            }

            return true;
        }

        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="newdisplayorder"></param>
        /// <param name="newname"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public bool BatAddBbsSort(string[] newdisplayorder, string[] newname, int gid)
        {
            //忽略
            if (newdisplayorder == null || newname == null) return true;
            //忽略
            if (newdisplayorder.Length != newname.Length) return true;

            for (var i = 0; i < newdisplayorder.Length; i++)
            {
                if (string.IsNullOrEmpty(newname[i])) continue;
                int order;
                int.TryParse(newdisplayorder[i], out order);
                this.GroupbbsSortRepository.Add(new group_bbsSort { gId = gid, showXh = order, sortName = newname[i] });

            }

            return true;
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="bbsid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool Bbsding(int bbsid, decimal uid)
        {
            var data = this.GroupbbsRepository.Get(bbsid);

            if (data == null) return false;

            var ut = this.GroupServices.CheckManageRight((int)data.gId, uid);

            if (ut != UserType.副团长 && ut != UserType.团长) return false;

            if (data.isTop == true)
            {
                data.isTop = false;
            }
            else
            {
                data.isTop = true;
            }

            this.GroupbbsRepository.Update(data);

            return true;
        }

        /// <summary>
        /// 高亮
        /// </summary>
        /// <param name="bbsid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool GaoLiang(int bbsid, decimal uid)
        {
            var data = this.GroupbbsRepository.Get(bbsid);

            if (data == null) return false;

            var ut = this.GroupServices.CheckManageRight((int)data.gId, uid);

            if (ut != UserType.副团长 && ut != UserType.团长) return false;

            if (data.isLight == true)
            {
                data.isLight = false;
            }
            else
            {
                data.isLight = true;
            }

            this.GroupbbsRepository.Update(data);

            return true;
        }

        /// <summary>
        /// 加精
        /// </summary>
        /// <param name="bbsid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool Jinghua(int bbsid, decimal uid)
        {
            var data = this.GroupbbsRepository.Get(bbsid);

            if (data == null) return false;

            var ut = this.GroupServices.CheckManageRight((int)data.gId, uid);

            if (ut != UserType.副团长 && ut != UserType.团长) return false;

            if (data.isPith == true)
            {
                data.isPith = false;
            }
            else
            {
                data.isPith = true;
            }

            this.GroupbbsRepository.Update(data);

            return true;
        }

        public group_bbs GetBBsInfo(int bbsid)
        {
            return this.GroupbbsRepository.Get(bbsid);
        }
    }
}
