using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Data;
using Kt.GameGroup.Model.ViewModel;
using Kt.GameGroup.Model.Enums;
using Kt.GameWeiBo.Services;
using Kt.GameGroup.Services;
using Kt.UserHome.Services;
using Kt.Framework.Linq;
using Kt.Framework.Repository.Data;
namespace Kt.GameGroup.Services
{
    /// <summary>
    /// 游戏团成功公用方法
    /// </summary>
    public class GroupMeberServices
    {
        private IRepository<group_member> GroupMemberRepository;
        private IRepository<Group_Grade> GroupGradeRepository;
        private MemberServices _MemberServices;
        private GroupServices _GroupServices;
        private PersonalMainPageServices _PersonalMainPageServices;
        private NewsServices newsServices;
        public GroupMeberServices(
                IRepository<group_member> GroupMemberRepository,
                MemberServices _MemberServices, 
                GroupServices _GroupServices,
                PersonalMainPageServices _PersonalMainPageServices,
                NewsServices newsServices,
                IRepository<Group_Grade> GroupGradeRepository
           )
        {
            this.GroupMemberRepository = GroupMemberRepository;
            this._MemberServices = _MemberServices;
            this._GroupServices = _GroupServices;
            this._PersonalMainPageServices = _PersonalMainPageServices;
            this.newsServices = newsServices;
            this.GroupGradeRepository = GroupGradeRepository;
        }

        /// <summary>
        /// 游戏团成员列表
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public GroupMemberData GetGroupMembersByGId(int gId, int PageNo = 1, int PageSize = 20,int utype=0)
        {

            IEnumerable<group_member> groupMemberList = null;

            if (utype == 1)
            {
                groupMemberList = this.GroupMemberRepository.Where(t => t.gId == gId && t.state == true && t.uType == 3);
            }
            else
            {
                groupMemberList = this.GroupMemberRepository.Where(t => t.gId == gId && t.state == true);
            }
            var count = groupMemberList.Count();
            var xxxx = groupMemberList.OrderByDescending(x => x.revTime);
            var datas = xxxx.Skip((PageNo - 1) * PageSize).Take(PageSize);
            /* var predicate = PredicateBuilder.True<group_member>();
             predicate.And(t => t.gId == gId && t.state == true);
             if (utype == 1)
             {
                 predicate.And(x=>x.uType!=1); 
             }

             var count = this.GroupMemberRepository.Get().Where(predicate).Count();

             var xxxx = from groupMemberList in this.GroupMemberRepository.Table
                        orderby groupMemberList.revTime descending
                        select groupMemberList;
             var datas = xxxx.Where(predicate).Skip((PageNo - 1) * PageSize).Take(PageSize);
             */
            IList<ViewGroupMemberData> list = new List<ViewGroupMemberData>();
            Kt.Framework.User.UserOnline online = new Kt.Framework.User.UserOnline();
            foreach (var item in datas)
            {
                
                ViewGroupMemberData vm = new ViewGroupMemberData()
                {
                    uid = Convert.ToDecimal(item.myUid),
                    nickname = this._MemberServices.getNickByUid(Convert.ToDecimal(item.myUid)),
                    face_Small = this._MemberServices.GetAvatarImage(Convert.ToDecimal(item.myUid), DS.Web.UCenter.AvatarSize.Small, DS.Web.UCenter.AvatarType.Virtual),
                    UserType = this._GroupServices.CheckManageRight(gId, Convert.ToDecimal(item.myUid)),
                    IsOnLine = online.IsOnLine(Convert.ToDecimal(item.myUid)),
                    relation = Convert.ToInt32(this._PersonalMainPageServices.GetRelation(Convert.ToDecimal(item.myUid))),
                    GroupUserName = item.GroupUserName,
                    GroupUserImg = item.GroupUserImg,
                    revTime = (DateTime)item.revTime,
                    UserGroupGradeName = GetUserGroupGradeName(gId,item.GradeId)
                };
                list.Add(vm);
            }
            var vd = new GroupMemberData
            {
                count = count,
                ViewGroupMemberData = list,
                PageNo = PageNo,
                PageSize = PageSize
            };
            return vd;
        }
         /// <summary>
        /// 管理游戏团成员（团长，副团长数据）
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="userType">用户身份</param>
        /// <returns></returns>
        public GroupMemberInfo GetGroupMembersByGId(int gId)
        {
            var HeadData = this.GroupMemberRepository.First(x => x.gId == gId && x.uType == 1);
            var DeputyData = this.GroupMemberRepository.Where(x => x.gId == gId && x.uType == 2);
             
            Dictionary<decimal, ViewGroupMember> DeputyGroupMembers = new Dictionary<decimal, ViewGroupMember>();
            foreach (var item in DeputyData)
            {
                //副团长数据模板
                ViewGroupMember vm = new ViewGroupMember()
                 {
                     uid = Convert.ToDecimal(item.myUid),
                     nickname = this._MemberServices.getNickByUid(Convert.ToDecimal(item.myUid)),
                     face_Small = this._MemberServices.GetAvatarImage(Convert.ToDecimal(item.myUid), DS.Web.UCenter.AvatarSize.Small, DS.Web.UCenter.AvatarType.Virtual),
                     GroupUserName = item.GroupUserName,
                     GroupUserImg = item.GroupUserImg,
                     revTime = (DateTime)item.revTime,
                     UserGroupGradeName = GetUserGroupGradeName(gId, item.GradeId)
                 };
                DeputyGroupMembers.Add(Convert.ToDecimal(item.myUid), vm);
            }
            
            GroupMemberInfo gm = new GroupMemberInfo()
            {
                Uid = Convert.ToDecimal(HeadData.myUid),
                Nickname = this._MemberServices.getNickByUid(Convert.ToDecimal(HeadData.myUid)),
                face_Small = this._MemberServices.GetAvatarImage(Convert.ToDecimal(HeadData.myUid), DS.Web.UCenter.AvatarSize.Small, DS.Web.UCenter.AvatarType.Virtual),
                DeputyGroupMembers = DeputyGroupMembers,
                GroupUserName = HeadData.GroupUserName
            };
            return gm;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 获取游戏团内最新加入的成员列表(未审批之前)
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="top">获取成员个数</param>
        /// <returns></returns>
        public GroupMemberData GetNewGroupMembers(int gId)
        {
            var memberlist = this.GroupMemberRepository.Where(t => t.gId == gId && t.state == false);
            var count = memberlist.Count();
            var datas = memberlist.OrderByDescending(x=>x.revTime);
            IList<ViewGroupMemberData> list = new List<ViewGroupMemberData>();
            Kt.Framework.User.UserOnline online = new Kt.Framework.User.UserOnline();
            foreach (var item in datas)
            {
                ViewGroupMemberData vm = new ViewGroupMemberData()
                {
                    uid = Convert.ToDecimal(item.myUid),
                    nickname = this._MemberServices.getNickByUid(Convert.ToDecimal(item.myUid)),
                    face_Small = this._MemberServices.GetAvatarImage(Convert.ToDecimal(item.myUid), DS.Web.UCenter.AvatarSize.Small, DS.Web.UCenter.AvatarType.Virtual),
                    revTime=(DateTime)item.revTime,
                    UserType = this._GroupServices.CheckManageRight(gId, Convert.ToDecimal(item.myUid)),
                    GroupUserName = item.GroupUserName,
                    GroupUserImg = item.GroupUserImg,
                    UserGroupGradeName = GetUserGroupGradeName(gId, item.GradeId)
                };
                list.Add(vm);
            }
            var vd = new GroupMemberData
            {
                ViewGroupMemberData = list,
                count=count,
                UType = this._GroupServices.CheckManageRight(gId, Kt.Framework.User.User.MEMBER_ID)
            };
            return vd;   
        }

        /// <summary>
        /// 获取游戏团内最新加入的成员列表
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="top">获取成员个数</param>
        /// <returns></returns>
        public IEnumerable<group_member> GetNewGroupMembersByGId(int gId, int top = 9)
        {
            //return GroupMemberRepository.Where(t => t.gId == gId && t.state == true).OrderByDescending(t => t.revTime).Take(top);
            var list = GroupMemberRepository.Where(t => t.gId == gId && t.state == true).OrderByDescending(t => t.revTime).Take(top);
            return list;
        }

        /// <summary>
        /// 获取游戏团内最活跃的成员列表
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="top">获取成员个数</param>
        /// <returns></returns>
        public IEnumerable<group_member> GetActiveGroupMembersByGId(int gId, int top = 9)
        {
            //return GroupMemberRepository.Where(t => t.gId == gId && t.state == true).OrderByDescending(t => t.postNum).Take(top);
            var list = GroupMemberRepository.Where(t => t.gId == gId && t.state == true).OrderByDescending(t => t.postNum).Take(top);
            return list;
        }

        /// <summary>
        /// 获取游戏团副管理成员
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <returns>GroupMemberInfo</returns>
        public IEnumerable<group_member> GetGroupMemberCount(int gId)
        {
            //return GroupMemberRepository.Where(t => t.gId == gId && t.uType == 2);
            var list = GroupMemberRepository.Where(t => t.gId == gId && t.uType == 2);
            return list;
        }
        /// <summary>
        /// 处理用户入团申请
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="Uid">用户编号</param>
        /// <param name="operType">处理类型,1==通过,2==忽略</param>
        /// <returns></returns>
        public void DoGroupMemberApply(int gId, decimal Uid,int operType) 
        {
            var MemberApply = this.GroupMemberRepository.First(x => x.gId == gId && x.myUid == Uid);
            switch (operType)
            { 
                case 1:
                    MemberApply.state = true;
                    MemberApply.uType = 3;
                    this.GroupMemberRepository.Update(MemberApply);
                    _GroupServices.ModifyGroupMemberNum(gId, 1, 1);
                    _GroupServices.ChangeGroupPoint(gId, GameGroupPointType.新成员加入);
                    //程钢添加游戏团加入成功通知
                    var gname = _GroupServices.getGroupInfo(gId).gName;
                    newsServices.SendSysMessage(Uid, "您加入" + gname + "游戏团的申请，已被审核通过，可<a href='/GameGroup/group/?gid=" + gId + "'>立即进入游戏团。</a>", 1);
                    break;
                case 2:
                     this.GroupMemberRepository.Delete(MemberApply);
                    _GroupServices.ModifyGroupMemberNum(gId, 1, 2);
                break;
            }
            //group_member表中的state字段修改为int型
            //throw new NotImplementedException();
        }       
        /// <summary>
        /// 管理游戏团成员
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="Uids">用户编号，多个用半角逗号隔开</param>
        /// <param name="operType">1==设置为副团长,2==设置为普通成员,3==踢出该游戏团</param>
        /// <returns></returns>
        public void MangeGroupMember(int gId, decimal uid, int operType)
        {
            var uTypes = this.GroupMemberRepository.First(x => x.gId == gId && x.myUid == uid);
            switch (operType)
            { 
                case 1:
                    uTypes.uType = 2;
                    this.GroupMemberRepository.Update(uTypes);
                    break;
                case 2:
                    uTypes.uType = 3;
                    this.GroupMemberRepository.Update(uTypes);
                    break;
                case 3:
                    this.GroupMemberRepository.Delete(uTypes);
                    _GroupServices.ModifyGroupMemberNum(gId, 1, 2);
                    _GroupServices.ChangeGroupPoint(gId, GameGroupPointType.新成员加入, 1);
                   
                    break;
                    //this.GroupMemberRepository.Update(uTypes);
            }
            
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 指定游戏团副团长数
        /// </summary>
        /// <param name="gId"></param>
        /// <returns></returns>
        public int GroupDeputyNum(int gId)
        {
            var Num = this.GroupMemberRepository.Where(x => x.gId == gId && x.uType == 2).Count();
            return Num;
        }
        /// <summary>
        /// 将该游戏团转让给某人
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="Uid">用户编号,该用户必须为游戏团成员</param>
        /// <returns></returns>
        public bool TransferGameGroup(int gId, decimal Uid)
        {
            throw new NotImplementedException();
        }

        #region 加入/退出游戏团

        /// <summary>
        /// 退出游戏团
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="gId">游戏团ID</param>
        /// <returns>JoinGroup</returns>
        public JoinGroup OutGameGroup(decimal uid, int gId)
        {
            if (uid <= 0) return JoinGroup.未登录;
            var join = GroupMemberRepository.First(t => t.myUid == uid && t.gId == gId);
            if (join == null) return JoinGroup.已退出;
            GroupMemberRepository.Delete(join);
            _GroupServices.ModifyGroupMemberNum(gId, 1, 2);
            _GroupServices.ChangeGroupPoint(gId, GameGroupPointType.新成员加入, 1);
            return JoinGroup.已退出;
        }

        /// <summary>
        /// 加入游戏团
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="gId">团ID</param>
        /// <param name="joinPerm">加入方式</param>
        /// <returns>JoinGroup</returns>
        public JoinGroup JoinGameGroup(decimal uid, int gId, int joinPerm)
        {
            if (uid <= 0) return JoinGroup.未登录;
            var join = GroupMemberRepository.First(t => t.myUid == uid && t.gId == gId);
            if (join != null)
            {
                if (join.state == true) return joinstate(true);
                return joinstate(false);
            }
            bool state = false;
            switch (joinPerm)
            {
                case 0: return JoinGroup.禁止加入;
                    break;
                case 1: state = true;
                    _GroupServices.ChangeGroupPoint(gId, GameGroupPointType.新成员加入);
                    break;
                case 2: state = false;
                    break;
                default: break;
            }
            if (state)
            {
                _GroupServices.ModifyGroupMemberNum(gId, 1, 1);
            }
            var nickname = this._MemberServices.getNickByUid(uid);
            group_member groupMember = new group_member()
            {
                myUid = uid,
                uType = 3,
                gId = gId,
                state = state,
                revTime = DateTime.Now,
                postNum = 0,
                GradeId = 6,     //加入时默认最低级别hjm
                GroupUserName = nickname,
                GroupUserImg = this._MemberServices.GetAvatarImage(uid, DS.Web.UCenter.AvatarSize.Small, DS.Web.UCenter.AvatarType.Virtual),
            };
            GroupMemberRepository.Add(groupMember);
            //程钢添加发送申请站内信
            if (!state)
            {
                var cap = GroupMemberRepository.Where(p => p.gId == gId && p.uType == 1).SingleOrDefault();
                var cap2 = GroupMemberRepository.Where(p => p.gId == gId && p.uType == 2).SingleOrDefault();
                //var nickname = this._MemberServices.getNickByUid(uid);
                var gname = _GroupServices.getGroupInfo(gId).gName;
                newsServices.SendSysMessage(Convert.ToDecimal(cap.myUid), uid + "@" + nickname + "&" + nickname + "申请加入" + gname + "游戏团，请您审核。<a href='/GameGroup/group/?gid=" + gId + "'>进入游戏团</a>", 5);
                if (cap2 != null)
                {
                    newsServices.SendSysMessage(Convert.ToDecimal(cap2.myUid), uid + "@" + nickname + "&" + nickname + "申请加入" + gname + "游戏团，请您审核。<a href='/GameGroup/group/?gid=" + gId + "'>进入游戏团</a>", 5);
                }
            }
            return joinstate(state);
        }

        private JoinGroup joinstate(bool state)
        {
            if (state) return JoinGroup.已加入;
            return JoinGroup.审核中;
        }
        #endregion

        /// <summary>
        /// 修改成员发布帖子数量、最后发布时间
        /// </summary>
        /// <param name="gId"></param>
        /// <param name="uid"></param>
        /// <param name="type">1:添加</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public bool updatePostCount(int gId, decimal uid, int type, int num = 1)
        {
            var _mem = GroupMemberRepository.First(t => t.gId == gId && t.myUid == uid);
            if (_mem == null) return false;
            if (type == 1)
            {
                _mem.postNum += num;
                _mem.LastPostTime = DateTime.Now;
            }
            else
            {
                if (_mem.postNum - num >= 0)
                    _mem.postNum -= num;
                else
                    _mem.postNum = 0;
            }
            GroupMemberRepository.Update(_mem);
            return true;
        }

        /// <summary>
        /// 获取用户在某个游戏团中的成员信息
        /// </summary>
        /// <param name="gId"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Kt.GameGroup.Data.group_member GetGroupMemberInfo(int gId,decimal uid)
        {
            var list = this.GroupMemberRepository.Where(t=>t.gId==gId&&t.myUid==uid);
            if (list != null && list.Count() > 0)
            {
                return list.First();
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public string GetUserGroupGradeName(int gId, int gradeId)
        {
            string gradename = "";
            var list = this.GroupGradeRepository.Where(t=>t.Gid==gId&&t.GradeId==gradeId);
            if (list != null && list.Count() > 0)
            {
                gradename = list.First().GradeName;
            }
            else
            { 
                IList<KeyValuePair<int, string>> glist = Kt.GameGroup.Model.Enums.GroupUserGrade.EnumsHelper<Kt.GameGroup.Model.Enums.GroupUserGrade.游戏团成员级别>.GetList();
                gradename = glist[gradeId - 1].Value;
            }
            return gradename;
        }

        public IList<ViewGroupMemberData> getMemeberByGidGradeId(int gid, int gradeId)
        {
            var list = GroupMemberRepository.Where(p => p.gId == gid && p.GradeId == gradeId).AsEnumerable().Select(p => new ViewGroupMemberData { uid=Convert.ToDecimal(p.myUid), GroupUserName=p.GroupUserName });
            IList<ViewGroupMemberData> list2 = new List<ViewGroupMemberData>();

            foreach (var item in list)
            {
                ViewGroupMemberData vm = new ViewGroupMemberData()
                {
                    uid = Convert.ToDecimal(item.uid),
                    //nickname = this._MemberServices.getNickByUid(Convert.ToDecimal(item.myUid)),
                    nickname = item.GroupUserName == null ? this._MemberServices.getNickByUid(item.uid) : item.GroupUserName,

                    face_Small = this._MemberServices.GetAvatarImage(item.uid, DS.Web.UCenter.AvatarSize.Small, DS.Web.UCenter.AvatarType.Virtual),

                };
                list2.Add(vm);
            }
            return list2.ToList();
        }

        /// <summary>
        /// 设置团员等级
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="fuids"></param>
        /// <param name="gradeid">-1 删除 </param>
        /// <returns></returns>
        public bool setGrade(int gid, string fuids, int gradeid) 
        {
            int index = fuids.LastIndexOf(",") + 1;
            if (index == fuids.Length)
            {
                fuids = fuids.Substring(0, fuids.LastIndexOf(","));
            }
            string[] uid2 = fuids.Split(',');
            if (gradeid != -1)
            {
                
                foreach (var item in uid2)
                {
                    var uid3 = Convert.ToDecimal(item);
                    try
                    {
                        var memeberinfo = GroupMemberRepository.Where(p => p.gId == gid && p.myUid == uid3).SingleOrDefault();
                        memeberinfo.GradeId = gradeid;
                        if (gradeid == 1 || gradeid == 2)
                        {
                            memeberinfo.uType = gradeid;
                        }
                        else 
                        {
                            memeberinfo.uType = 3;
                        }
                        GroupMemberRepository.Update(memeberinfo);
                    }
                    catch
                    {
                        return false;
                    }
                }
                return true;
            }
            else 
            {

                foreach (var item in uid2)
                {
                    var uid3 = Convert.ToDecimal(item);
                    try
                    {
                        var m= GroupMemberRepository.Where(p => p.gId == gid && p.myUid == uid3).SingleOrDefault();
                        this.GroupMemberRepository.Delete(m);
                        _GroupServices.ModifyGroupMemberNum(gid, 1, 2);
                        _GroupServices.ChangeGroupPoint(gid, GameGroupPointType.新成员加入, 1);
                    }
                    catch
                    {
                        return false;
                    }
                   
                }
                return true;
            }
        }

        public int SaveGroupUserName(decimal uid,int gid,string groupusername)
        {
            int rst = 1;
            var list = this.GroupMemberRepository.Where(t=>t.myUid==uid &&t.gId==gid);
            var info = new Kt.GameGroup.Data.group_member();
            if (list != null && list.Count() > 0)
            {
                info = list.First();
                info.GroupUserName = groupusername;
                this.GroupMemberRepository.Update(info);
            }
            else
            {
                rst = 0;
            }
            return rst;
        }

        public string GetGroupUserGradeName(int gid,int gradeid)
        {
            string gradename = "";
            var info = this.GroupMemberRepository.Where(t=>t.gId==gid&&t.GradeId== gradeid);
            if (info != null && info.Count() > 0)
            {
                gradename = info.First().GroupUserName;
            }
            return gradename;
        }
        /// <summary>
        /// 判断团人数是否满足人数
        /// </summary>
        /// <returns></returns>
        public int CheckTuanMembersNO(int gid,int num) 
        {
            var nums=GroupMemberRepository.Where(t=>t.gId==gid).Count();
            if (nums>=num)
            {
                return 1;
            }
            return 0;
        }
        public int CheckTuanUser(int gid, string uid)
        {
            if (gid>0&&uid!=null&&uid!="")
            {
                var u = Convert.ToDecimal(uid);
                var nums = GroupMemberRepository.First(t => t.gId == gid && t.myUid == u).GradeId;
                if (nums == 1)
                {
                    return 1;
                }
            }
            
            return 0;
        }
    }
}
