using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using Kt.GameGroup.Data;
using Kt.GameGroup.Model.ViewModel;
using System.IO;
using Kt.GameGroup.Model.Enums;
using Kt.GameNav.Data;
using Kt.GameGroup.Model.TransModel;
using System.Web;
using Kt.Framework.Repository.Data;

using Kt.Framework.Linq;
using Kt.Framework.Core;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web.Services;

namespace Kt.GameGroup.Services
{
    /// <summary>
    /// 游戏团业务方法
    /// </summary>
    public class GroupServices
    {
        private IRepository<group_Infor> GroupInforRepository;
        private IRepository<group_gameStat> GroupGameStatRepository;
        private IRepository<group_member> GroupMemberRepository;
        private IRepository<group_bbsSort> GroupBbsRepository;
        private IRepository<kt_games> kt_gamesRepository;
        private IRepository<kt_game_role> kt_game_roleRepository;
        private IRepository<kt_service_platform> kt_service_platform;
        private IRepository<kt_service_game> kt_service_game;
        private IRepository<kt_game_server> kt_game_server;
        private IRepository<Group_Grade> group_GradeReRepository;
        public GroupServices(IRepository<group_Infor> GroupInforRepository, IRepository<group_gameStat> GroupGameStatRepository,
            IRepository<group_member> GroupMemberRepository,
            IRepository<group_bbsSort> GroupBbsRepository,
            IRepository<kt_games> kt_gamesRepository,
            IRepository<kt_game_role> kt_game_roleRepository,
            IRepository<kt_service_platform> kt_service_platform,
            IRepository<kt_service_game> kt_service_game,
            IRepository<kt_game_server> kt_game_server,
            IRepository<Group_Grade> group_GradeReRepository
            )
        {
            this.GroupInforRepository = GroupInforRepository;
            this.GroupGameStatRepository = GroupGameStatRepository;
            this.GroupBbsRepository = GroupBbsRepository;
            this.GroupMemberRepository = GroupMemberRepository;
            this.kt_gamesRepository = kt_gamesRepository;
            this.kt_game_roleRepository = kt_game_roleRepository;
            this.kt_service_platform = kt_service_platform;
            this.kt_service_game = kt_service_game;
            this.kt_game_server = kt_game_server;
            this.group_GradeReRepository = group_GradeReRepository;
        }
        /// <summary>
        /// 创建游戏团
        /// </summary>
        /// <returns></returns>
        public bool CreateGroup(decimal uId, int gameId, int platFormId, int gameserverid, string gName, string descript, int joinPerm, bool viewPerm)
        {
            Kt.GameGroup.Data.group_Infor groupInfor = new Kt.GameGroup.Data.group_Infor()
            //this.GroupInforRepository.Add(new group_Infor
            {
                createDate = System.DateTime.Now,
                gameId = gameId,
                memberNum = 1,
                account = descript,
                gName = gName,
                postNum = 0,
                platFormId = platFormId,
                gameserverid = gameserverid,
                uId = uId,
                joinPerm = joinPerm,
                viewPerm = viewPerm,
                points = 0,
                maxNum = 200,
                postPerm = true,
                recommend = false,
                isClass = false,
                isOpenSort = false,
                isPrefix = false

            };

            //add by lxzh 2011-5-10 对group_member表 增加记录
            var addGroupInfo = this.GroupInforRepository.Add(groupInfor);
            this.GroupMemberRepository.Add(new group_member
            {
                myUid = uId,
                uType = 1,
                gId = addGroupInfo.gId,
                state = true,
                revTime = System.DateTime.Now,
                GradeId = 1

            });
            int rstgrade = SetIniGroupGrade(addGroupInfo.gId);
            //更新统计表中数据，不存在则insert , 否则 +1
            var gamestate = this.GroupGameStatRepository.First(x => x.gameId == gameId);

            if (gamestate == null)
            {
                this.GroupGameStatRepository.Add(new group_gameStat { gameId = gameId, groupNum = 1 });
            }
            else
            {
                gamestate.groupNum++;

                this.GroupGameStatRepository.Update(gamestate);
            }


            return true;
        }

        public bool CreateGroup(CreateGroupModel CreateGroupModel, HttpPostedFileBase gPic, out int gid)
        {
            gid = 0;
            int rst = 1;
            string smallimg = "";
            #region 上传图片到服务器
            if (gPic != null && gPic.ContentLength != 0)
               rst = UploadGroupICO_Simple(gPic.InputStream, gPic.FileName, out smallimg);
            if(rst!=1)
            {
                return false;
            }
            #endregion
            Kt.GameGroup.Data.group_Infor groupInfor = new Kt.GameGroup.Data.group_Infor()
            //this.GroupInforRepository.Add(new group_Infor
            {
                createDate = System.DateTime.Now,
                gameId = CreateGroupModel.gameId,
                memberNum = 1,
                account = CreateGroupModel.account,
                gName = CreateGroupModel.gName,
                postNum = 0,
                platFormId = CreateGroupModel.platFormId,
                gameserverid = CreateGroupModel.gameserverid,
                uId = CreateGroupModel.uId,
                joinPerm = CreateGroupModel.joinPerm,
                viewPerm = CreateGroupModel.viewPerm,
                points = 0,
                maxNum = 200,
                postPerm = true,
                recommend= false,
                isClass = false,
                isOpenSort = false,
                isPrefix = false,
                gPic = smallimg     //hjm创建游戏团时先插入图片，避免图片报错但数据库已插入记录的bug
                
            };

            //add by lxzh 2011-5-10 对group_member表 增加记录
            var addGroupInfo = this.GroupInforRepository.Add(groupInfor);
            this.GroupMemberRepository.Add(new group_member
            {
                myUid = addGroupInfo.uId,
                uType = 1,
                gId = addGroupInfo.gId,
                state = true,
                revTime = System.DateTime.Now,
                GradeId=1
            });
            int rstgrade = SetIniGroupGrade(addGroupInfo.gId);
            //更新统计表中数据，不存在则insert , 否则 +1
            var gamestate = this.GroupGameStatRepository.First(x => x.gameId == addGroupInfo.gameId);

            if (gamestate == null)
            {
                this.GroupGameStatRepository.Add(new group_gameStat { gameId = (int)addGroupInfo.gameId, groupNum = 1 });
            }
            else
            {
                gamestate.groupNum++;

                this.GroupGameStatRepository.Update(gamestate);
            }

            gid = addGroupInfo.gId;            
            return true;
        }
        /// <summary>
        /// 修改游戏团 
        /// </summary>
        /// <returns></returns>
        public bool ModifyGroup(Kt.GameGroup.Model.ViewModel.ModifyGroupInfo GroupInfo, HttpPostedFileBase imagefile)
        {
            Kt.GameGroup.Data.group_Infor groupInfos = this.GroupInforRepository.Where(m => m.gId == GroupInfo.gId).First();
            groupInfos.gName = GroupInfo.gName;
            //groupInfos.gPic = GroupInfo.gPic;
            groupInfos.account = GroupInfo.account;
            groupInfos.notice = GroupInfo.notice;
            groupInfos.gameId = GroupInfo.gameId;
            groupInfos.gameserverid = GroupInfo.gameserverid;
            groupInfos.platFormId = GroupInfo.platFormId;
            groupInfos.joinPerm = GroupInfo.joinPerm;
            groupInfos.viewPerm = GroupInfo.viewPerm;
            if (this.GroupInforRepository.Update(groupInfos) == null)
            {
                return false;
            }
            else
            {
                //调用上传图片函数，更新图片
                #region 上传图片到服务器
                if (imagefile != null && imagefile.ContentLength != 0)
                    UploadGroupICO(imagefile.InputStream, imagefile.FileName, GroupInfo.gId);
                #endregion

                return true;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 修改游戏团帖子数量
        /// </summary>
        /// <param name="gId">游戏团编号</param>
        /// <param name="num">要修改的数量</param>
        /// <param name="type">1、增加； 2、减少</param>
        /// <returns></returns>
        public bool ModifyGroupPostNum(int gId, int num, int type)
        {
            if (gId <= 0) return false;
            var _group = GroupInforRepository.Get(gId);
            if (_group == null) return false;
            if (_group.postNum == null) _group.postNum = 0;
            if (type == 1)
            {
                _group.postNum += num;
            }
            else
            {
                if (_group.postNum - num <= 0)
                    _group.postNum = 0;
                else
                    _group.postNum -= num;
            }
            GroupInforRepository.Update(_group);
            return true;
        }

        /// <summary>
        /// 修改游戏团人数
        /// </summary>
        /// <param name="gId">团编号</param>
        /// <param name="num">操作数量</param>
        /// <param name="type">1、添加  2、删除</param>
        public GroupNum ModifyGroupMemberNum(int gId, int num, int type)
        {
            if (gId <= 0) return GroupNum.该团已不存在;
            var _group = GroupInforRepository.Get(gId);
            if (_group == null) return GroupNum.该团已不存在;
            if (type == 1)
            {
                return AddGroupMemberNum(_group, num);
            }
            return DeleteGroupMemberNum(_group, num);
        }

        public GroupNum AddGroupMemberNum(group_Infor _group, int num)
        {
            if (_group.memberNum + num > _group.maxNum)
            {
                return GroupNum.超过人数上线;
            }
            _group.memberNum += num;
            GroupInforRepository.Update(_group);
            return GroupNum.操作成功;
        }

        public GroupNum DeleteGroupMemberNum(group_Infor _group, int num)
        {
            if (_group.memberNum - num <= 0)
                _group.memberNum = 1;
            else
                _group.memberNum -= num;
            GroupInforRepository.Update(_group);
            return GroupNum.操作成功;
        }

        /// <summary>
        /// 修改游戏团Logo
        /// </summary>
        /// <param name="gId"></param>
        /// <param name="gPic"></param>
        /// <returns></returns>
        public bool ModifyGroup(int gId, string gPic)
        {
            Kt.GameGroup.Data.group_Infor groupInfos = this.GroupInforRepository.Where(m => m.gId == gId).First();
            groupInfos.gPic = gPic;
            if (this.GroupInforRepository.Update(groupInfos) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 热门游戏群组， 注意在实现的时候个数别写死
        /// </summary>
        /// <param name="topgame">显示游戏数量</param>
        /// <param name="hotgroup">显示群组数量</param>
        /// <returns></returns>
        public HotGameGroupInfo HotGroup(int topgame = 6, int tophotgroup = 20)
        {
            Dictionary<int, string> GameList = new Dictionary<int, string>();

            Dictionary<int, int?> GroupCount = new Dictionary<int, int?>();
            Dictionary<int, IEnumerable<group_Infor>> GroupList = new Dictionary<int, IEnumerable<group_Infor>>();
            var list = GroupGameStatRepository.Get().OrderByDescending(t => t.groupNum).Take(topgame).ToList();
            foreach (var item in list)
            {
                GameList.Add(item.gameId, kt_gamesRepository.First(t => t.gameId == item.gameId).game_name);
                GroupCount.Add(item.gameId, item.groupNum);

                var game_group = this.GroupInforRepository.Where(t => t.gameId == item.gameId).OrderByDescending(t => t.points).Take(tophotgroup).ToList();
                GroupList.Add(item.gameId, game_group);
            }
            return new HotGameGroupInfo
            {
                GameList = GameList,
                GroupCount = GroupCount,
                GroupList = GroupList
            };
        }

        /// <summary>
        /// 游戏团排行榜
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public GroupInfo GroupRankingList(int top = 10)
        {
            return new GroupInfo()
            {
                Group_Infor = GroupInforRepository.Get().OrderByDescending(t => t.points).Take(top)
            };
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 推荐的群组
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public GroupInfo RecomendGroupList(int top = 8)
        {
            return new GroupInfo()
            {
                Group_Infor = GroupInforRepository.Where(t => t.recommend == true).OrderByDescending(t => t.points).Take(top)
            };
        }

        /// <summary>
        /// 我的游戏团
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public GroupInfo MyGroupList(decimal uid, int top = 5)
        {
            return new GroupInfo()
            {
                Group_Infor = (from a in GroupMemberRepository.Where(t => t.myUid == uid && t.state == true).OrderBy(t => t.uType)
                               join b in GroupInforRepository.Get() on a.gId equals b.gId
                               select b).OrderByDescending(t => t.points).ThenByDescending(t => t.createDate).Take(top)
            };
        }
        public GroupInfo MyGroupListMore(decimal uid)
        {
            return new GroupInfo()
            {
                Group_Infor = (from a in GroupMemberRepository.Where(t => t.myUid == uid && t.state == true).OrderBy(t => t.uType)
                               join b in GroupInforRepository.Get() on a.gId equals b.gId
                               select b).OrderByDescending(t => t.points).ThenByDescending(t => t.createDate)
            };
        }

        /// <summary>
        /// 程钢添加获取游戏团和MyGroupList方法排序条件不一样固重写
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public GroupInfo MyGroupListorderbytype(decimal uid, int top = 5)
        {
            return new GroupInfo()
            {
                Group_Infor = (from a in GroupMemberRepository.Where(t => t.myUid == uid && t.state == true).OrderBy(t => t.uType)
                               join b in GroupInforRepository.Get() on a.gId equals b.gId
                               select b).AsEnumerable().OrderByDescending(p => p.uId.ToString().IndexOf(uid.ToString())).ThenByDescending(t => t.points).ThenByDescending(t => t.createDate).Take(top)
            };
        }

        

        /// <summary>
        /// 搜索群组
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="platFormId"></param>
        /// <param name="gameserverid"></param>
        /// <param name="gName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GroupInfo SearchGroup(int gameId = 0, int platFormId = 0, int gameserverid = 0, string gName = "", int pageNo = 1, int pageSize = 20)
        {
            var list = this.GroupInforRepository.Where(p => p.gameId == gameId);

            if (platFormId != 0)
            {
                list = list.Where(p => p.platFormId == platFormId);
            }

            if (gameserverid != 0)
            {
                list = list.Where(p => p.gameserverid == gameserverid);
            }

            if (!string.IsNullOrEmpty(gName))
            {
                list = list.Where(p => p.gName.IndexOf(gName) >= 0);
            }

            var count = list.Count();
            list = list.OrderByDescending(t => t.points).Skip((pageNo - 1) * pageSize).Take(pageSize);

            GroupInfo _GroupInfo = new GroupInfo();
            _GroupInfo.Group_Infor = list;

            _GroupInfo.count = count;
            _GroupInfo.pageNo = pageNo;
            _GroupInfo.pageSize = pageSize;

            return _GroupInfo;
        }


        /// <summary>
        /// 上传游戏团图标，
        /// </summary>
        /// <param name="InputStream"></param>
        /// <param name="FileName"></param>
        /// <param name="uid"></param>
        /// <param name="groupId"></param>
        /// <returns>返回图片路径 </returns>
        public string UploadGroupICO(Stream InputStream, string FileName, int groupId)
        {
            // 参考 Kt.UserHome.Services.PhotoService.UploadImage 方法 
            //var __is_image = false;

            int thumbwidth = 90;
            int thumbheight = 90;
            
            var fileKey = "";

            if (InputStream != null)
            {
                //调用图片上传接口
                var fileserverImage = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<Kt.Framework.FileServer.IImageFile>();
                //上传图片至图片服务器，保存原图同时并生成缩略图，返回的是原图的 “图片KEY”，
                fileKey = fileserverImage.SaveImageFile(InputStream, FileName, thumbwidth, thumbheight);//, PhotoImageSmall.thumbwidth, PhotoImageSmall.thumbheight

                //var image_size = Kt.Framework.Common.ImageHelper.GetImageSize(InputStream);

                //photoData.size = (int)(InputStream.Length / 8);
                //photoData.title = FileName;
                //photoData.filepath = fileKey;
                //this.photopicRepository.Update(photoData);

                //__is_image = true;
            }

            //图片地址解析方法 
            var key = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<Kt.Framework.FileServer.IKey>();
            //返回缩略图地址
            var smallurl = key.GetFileUrl(fileKey, thumbwidth, thumbheight);
            //model.smallPic = smallurl;

            Kt.GameGroup.Data.group_Infor groupInfos = this.GroupInforRepository.Where(m => m.gId == groupId).First();
            groupInfos.gPic = smallurl;

            if (this.GroupInforRepository.Update(groupInfos) == null)
            {
                //更新失败，提示错误并退出
                return "-1";
            }

            return smallurl;
            //throw new NotImplementedException();

        }

        /// <summary>
        /// 上传游戏团图标hjm
        /// </summary>
        /// <param name="InputStream"></param>
        /// <param name="FileName"></param>
        /// <param name="uid"></param>
        /// <param name="groupId"></param>
        /// <returns>返回图片路径 </returns>
        public int UploadGroupICO_Simple(Stream InputStream, string FileName, out string smallurl)
        {
            // 参考 Kt.UserHome.Services.PhotoService.UploadImage 方法 
            int rst = 1;
            smallurl = "";
            int thumbwidth = 90;
            int thumbheight = 90;

            var fileKey = "";
            try
            {
                if (InputStream != null)
                {
                    //调用图片上传接口
                    var fileserverImage = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<Kt.Framework.FileServer.IImageFile>();
                    //上传图片至图片服务器，保存原图同时并生成缩略图，返回的是原图的 “图片KEY”，
                    fileKey = fileserverImage.SaveImageFile(InputStream, FileName, thumbwidth, thumbheight);//, PhotoImageSmall.thumbwidth, PhotoImageSmall.thumbheight               
                }

                //图片地址解析方法 
                var key = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<Kt.Framework.FileServer.IKey>();
                //返回缩略图地址
                smallurl = key.GetFileUrl(fileKey, thumbwidth, thumbheight);
            }
            catch
            {
                rst = 0;
            }

            return rst;
             
        }

        /// <summary>
        /// 转让群组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        public int GroupTransfer(int groupId, decimal fromId, decimal toId)
        {
            Kt.GameGroup.Data.group_Infor ginfo = this.GroupInforRepository.Get(groupId);
            //1,判断是否有转让的权限
            if (ginfo != null && (decimal)ginfo.uId == fromId)
            {
                Kt.GameGroup.Data.group_member fminfo = this.GroupMemberRepository.Get().Where(t => t.myUid == fromId && t.gId == groupId).First();
                Kt.GameGroup.Data.group_member tminfo = this.GroupMemberRepository.Get().Where(t => t.myUid == toId && t.gId == groupId).First();
                if (fminfo == null || tminfo == null)
                {
                    return -3;
                }
                //判断是否有接收的权限
                var query = this.GroupMemberRepository.Where(t => t.gId == groupId && t.uType == 2 && t.myUid == toId);
                if (query == null)
                {
                    //没有接收的权限
                    return -2;
                }

                /**********************实行转让的操作**********************/
                //2。1更新该游戏团的创建人
                ginfo.uId = toId;
                this.GroupInforRepository.Update(ginfo);
                //2。2撤消原正团长为副团

                fminfo.uType = 2;
                fminfo.GradeId = 2;
                this.GroupMemberRepository.Update(fminfo);
                //2。3将接收人改为正团长                
                tminfo.uType = 1;
                tminfo.GradeId = 1;
                this.GroupMemberRepository.Update(tminfo);
                return 1;
            }
            else
            {
                return -1;
                //没有转让的权限
            }

        }
        /// <summary>
        /// 可能感兴趣的游戏
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GameInfoModel> MayBeInterestedGame(decimal uid)
        {
            //规则 ： 
            // 杨兆昆(杨兆昆) 17:11:42
            //可能感兴趣的 调用用户建立的游戏角色所属游戏
            //去游戏导航中调用本用户创建的角色
            var roles = this.kt_game_roleRepository.Where(m => m.uid == uid);
            var games = this.kt_gamesRepository.Where(t => (roles.Select(m => m.gameId).Distinct()).Contains(t.gameId)
                ).Distinct();
            IList<GameInfoModel> list = new List<GameInfoModel>();
            foreach (var item in games)
            {
                GameInfoModel ginfo = new GameInfoModel();
                ginfo.GameId = item.gameId;
                ginfo.GameName = item.game_name;
                ginfo.gamePic = item.game_logo;
                list.Add(ginfo);
            }
            return list;
            //throw new NotImplementedException();
        }

        public GroupInfo getGroupInfo(int gid)
        {
            if (gid <= 0) return null;
            var info = GroupInforRepository.Get(gid);            
            if (info == null) return null;
            int Ranking = GroupInforRepository.Where(t => t.points > info.points).Count() + 1;
            
            var iGameId = 0;
            if (info.gameId.HasValue)
                iGameId = (int)info.gameId;
            //var game_name = kt_gamesRepository.Where(t => t.gameId == (info.gameId.HasValue==true?0:1)).First();
            var game_name = kt_gamesRepository.First(t => t.gameId == iGameId);

            var iflatId = 0;
            if (info.platFormId.HasValue)
                iflatId = (int)info.platFormId;
            var plat_name = kt_service_platform.First(t => t.platform_id == iflatId);
            //var platserver_name = kt_service_game.First(t => t.service_game_id == iflatId);
            //var plat_name = kt_service_platform.First(t => t.platform_id == platserver_name.platform_id);

            var iserviceId = 0;
            if (info.gameserverid.HasValue)
                iserviceId = (int)info.gameserverid;
            var server_name = kt_game_server.First(t => t.game_server_id == iserviceId);
            var minfo = GroupMemberRepository.Where(x => x.gId == gid && x.uType == 1);
            string groupusername = "";
            if (minfo != null)
            {
                groupusername = minfo.First().GroupUserName;
            }
            var GroupInfo = new GroupInfo()
            {
                gId = gid,
                uId = info.uId,
                gName = info.gName,
                memberNum = info.memberNum,
                postNum = info.postNum,
                points = info.points,
                account = info.account,
                notice = info.notice,
                gPic = info.gPic,
                gameId = info.gameId,
                platFormId = info.platFormId,
                gameserverid = info.gameserverid,
                viewPerm = info.viewPerm,
                createDate = info.createDate,
                vistUrl = info.vistUrl,
                ranking = Ranking,
                joinPerm = info.joinPerm,
                GroupUserName = groupusername,
                isChatRoom=info.isChatRoom
            };
            if (game_name != null)
                GroupInfo.game_name = game_name.game_name;
            if (plat_name != null)
                GroupInfo.plat_name = plat_name.platform_name;
            if (server_name != null)
                GroupInfo.gameserver_name = server_name.server_name;
            
            
            return GroupInfo;
        }
        public Kt.GameGroup.Data.group_Infor GetGroupInfoByGid(int gid)
        {
            if (gid <= 0) return null;
            
            var info=GroupInforRepository.First(t=>t.gId==gid);
            if (info == null) return null;
            return info;
        }
        /// <summary>
        /// 分类数据属性
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public GroupThreadTypeModel GetGroupThreadType(int gid)
        {
            var model = new GroupThreadTypeModel();

            var group = GroupInforRepository.Get(gid);
            if (group != null)
            {

                //            Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
                //启用主题分类	isOpenSort	bit			FALSE	FALSE	FALSE
                //发帖必须归类	isClass	bit			FALSE	FALSE	FALSE
                //显示类别前缀	isPrefix	bit			FALSE	FALSE	FALSE
                model.jointype = (group.isOpenSort ?? false) ? 0 : 1;
                model.prefix = (group.isPrefix ?? false) ? 0 : 1;
                model.required = (group.isClass ?? false) ? 0 : 1;

                model.sortList = this.GroupBbsRepository.Where(x => x.gId == gid);

                if (model.sortList != null)
                    model.sortList = model.sortList.OrderBy(x => x.showXh);
                //model.sortList = model.sortList.OrderByDescending(x => x.showXh);            
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新主题类型
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="jointype"></param>
        /// <param name="prefix"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public bool UpdateThreadType(int gid, int jointype, int prefix, int required)
        {
            //throw new NotImplementedException();

            var ginfo = this.GroupInforRepository.Get(gid);

            if (ginfo == null) return false;

            ginfo.isOpenSort = (jointype == 0);

            ginfo.isPrefix = (prefix == 0);
            ginfo.isClass = (required == 0);

            this.GroupInforRepository.Update(ginfo);

            return true;
        }

        /// <summary>
        /// 判断用户是不是有管理权限
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UserType CheckManageRight(int gid, decimal uid)
        {
            if (gid == 0 || uid == 0) return UserType.游客;


            var member = this.GroupMemberRepository.First(x => x.gId == gid && x.myUid == uid);

            if (member == null)
                return UserType.游客;

            if (member.state != true)
                return UserType.申请中成员;
            if (member.uType == 1)
                return UserType.团长;
            if (member.uType == 2)
                return UserType.副团长;
            if (member.uType == 3)
                return UserType.普通成员;

            return UserType.游客;
        }

        /// <summary>
        /// 更新群组的积分
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="GameGroupPointType"></param>
        /// <returns></returns>
        public bool ChangeGroupPoint(int gid, GameGroupPointType GameGroupPointType, int type = 0, int point = 0)
        {
            var groupdata = this.GroupInforRepository.Get(gid);

            if (groupdata == null) return false;


            //      //游戏团积分规则：
            //新成员加入,//  +2
            //发起组团,// +2
            //发新帖子,//+2
            //发表回复    //+1

            if (groupdata.points == null) groupdata.points = 0;
            var addpoint = 0;
            switch (GameGroupPointType)
            {
                case Model.Enums.GameGroupPointType.新成员加入:
                    addpoint = 2;
                    break;
                case Model.Enums.GameGroupPointType.发起组团:
                    addpoint = 2;
                    break;
                case Model.Enums.GameGroupPointType.发新帖子:
                    addpoint = 2;
                    break;
                case Model.Enums.GameGroupPointType.发表回复:
                    addpoint = 1;
                    break;
            }
            if (type == 0)
            {
                groupdata.points += addpoint;
            }
            else
            {
                if (point != 0)
                    addpoint = point;
                if (groupdata.points - addpoint >= 0)
                    groupdata.points -= addpoint;
                else
                    groupdata.points = 0;
            }
            this.GroupInforRepository.Update(groupdata);

            return true;
        }

        /// <summary>
        /// 获取用户可能感兴趣的游戏团列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public GroupInfo UserLikeGroupList(decimal uid, int top = 9)
        {
            //return new GroupInfo()
            //{
            //    Group_Infor = (from  b in GroupInforRepository.Get()
            //                   join a in this.kt_game_roleRepository.Where(t => t.uid == uid ) on b.gameserverid equals a.game_server_id
            //                   select b).Distinct().OrderByDescending(t => t.points).ThenByDescending(t => t.createDate).Take(top)
            //    //Group_Infor = this.GroupInforRepository.Get().OrderByDescending(t => t.points).ThenByDescending(t => t.createDate).Take(top)

            //};
            var RoleList = (from a in this.kt_game_roleRepository.Where(x => x.uid== uid)
                            select a.game_server_id).Distinct();
            IList<group_Infor> list = new List<group_Infor>();
            var  glist = (  from m in this.GroupMemberRepository.Where(t=>t.myUid==uid && t.state==true)
                            select m.gId).Distinct();
            foreach (var item in RoleList)
            {
                var GroupList =  this.GroupInforRepository.Where(t=>t.gameserverid==item && t.joinPerm!=0 && ! glist.Contains(t.gId));                
                if (GroupList != null && GroupList.Count() > 0)
                {
                    foreach (var i in GroupList)
                    {
                        list.Add(i);
                    }
                }                
            }
           
            return new GroupInfo()
            {
                Group_Infor = list.OrderByDescending(t => t.points).Take(top)
            };
        }

        /// <summary>
        /// 获取用户可能感兴趣的游戏团列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<Kt.GameGroup.Model.ViewModel.GroupRankInfo> UserLikeGroupRankList(decimal uid, int top = 9)
        {
            //该用户游戏角色涉及的区服编号
            var RoleList = (from a in this.kt_game_roleRepository.Where(x => x.uid == uid)
                            select a.game_server_id).Distinct();
            //该用户已经成功加入或创建的游戏团
            IList<group_Infor> list = new List<group_Infor>();
            var glist = (from m in this.GroupMemberRepository.Where(t => t.myUid == uid && t.state == true)
                         select m.gId).Distinct();

            //获取和用户角色中区服一致，并且允许加入，且不包括用户自己已经成功加入或创建的游戏团 的游戏团列表
            foreach (var item in RoleList)
            {
                var GroupList = this.GroupInforRepository.Where(t => t.gameserverid == item && t.joinPerm != 0 && !glist.Contains(t.gId));
                if (GroupList != null && GroupList.Count() > 0)
                {
                    foreach (var i in GroupList)
                    {
                        list.Add(i);
                    }
                }
            }
            if (list.Count() == 0)
            {
                return null;
            }
            //获取这些游戏团的在所有游戏团总排行中的排名（该需求有点变态）
            var allgroup = GroupInforRepository.Get().OrderByDescending(t => t.points);
            IList<Kt.GameGroup.Model.ViewModel.GroupRankInfo> allrankgroup = new List<Kt.GameGroup.Model.ViewModel.GroupRankInfo>();
            
            int j = 1;
            //foreach (var item in allgroup)
            //{
            //    Kt.GameGroup.Model.ViewModel.GroupRankInfo rankmodel = new GroupRankInfo();
            //    rankmodel.Group_Infor = item;
            //    rankmodel.OrderId = j;
            //    j = j + 1;
            //    allrankgroup.Add(rankmodel);
            //}
            IList<Kt.GameGroup.Model.ViewModel.GroupRankInfo> myrankgroup = new List<Kt.GameGroup.Model.ViewModel.GroupRankInfo>();
            foreach(var item in list)
            {
                int gid = item.gId;
                //var oo = allrankgroup.Where(t=>t.Group_Infor.gId==gid);
                //if (oo != null && oo.Count() > 0)
                //{
                //    myrankgroup.Add(oo.First());
                //}
                int curpoints = (int)item.points;
                int Ranking = GroupInforRepository.Where(t => t.points > curpoints).Count() + 1;
                Kt.GameGroup.Model.ViewModel.GroupRankInfo rankmodel = new GroupRankInfo();
                rankmodel.Group_Infor = item;
                rankmodel.OrderId = Ranking;
                myrankgroup.Add(rankmodel);
            }

            return myrankgroup.OrderBy(t=>t.OrderId).ToList();
        }

        /// <summary>
        /// 查询不是本等级的所有等级信息
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="gradeid"></param>
        /// <returns></returns>
        public IList<Group_Grade> getGradebyGid(int gid, int gradeid) 
        {
            var list = group_GradeReRepository.Where(p => p.Gid==gid && p.GradeId != gradeid).ToList();
            return list;
        }

        public decimal getHeadUid(int gid) 
        {
            var uid = GroupInforRepository.Where(p => p.gId == gid).SingleOrDefault().uId;
            return Convert.ToDecimal(uid);
            
        }
        public IList<Kt.GameGroup.Model.ViewModel.GroupGradeInforData> getAllGrade(int gid)
        {
            var list = group_GradeReRepository.Where(t => t.Gid == gid).Where(t=>t.GradeName!="团长").OrderBy(t=>t.GradeId);
            var lists = new List<Kt.GameGroup.Model.ViewModel.GroupGradeInforData>();
            foreach (var item in list)
            {
                Kt.GameGroup.Model.ViewModel.GroupGradeInforData gd = new Kt.GameGroup.Model.ViewModel.GroupGradeInforData();
                gd.AutoId = item.AutoId;
                gd.Gid = item.Gid;
                gd.GradeId = item.GradeId;
                gd.GradeName = item.GradeName;
                gd.GradeNum = GroupMemberRepository.Where(t => t.gId == gid).Where(t => t.GradeId == item.GradeId).Count();
                lists.Add(gd);
            }
            return lists;
        }

        /// <summary>
        /// 插入游戏团初始化成员级别
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public int SetIniGroupGrade(int gid)
        {
            int rst = 1;
            IList<KeyValuePair<int, string>> glist = Kt.GameGroup.Model.Enums.GroupUserGrade.EnumsHelper<Kt.GameGroup.Model.Enums.GroupUserGrade.游戏团成员级别>.GetList();
            foreach (KeyValuePair<int, string> kv in glist)
            {
                Kt.GameGroup.Data.Group_Grade info = new Kt.GameGroup.Data.Group_Grade();
                info.Gid = gid;
                info.GradeId = kv.Key + 1;
                info.GradeName = kv.Value;
                this.group_GradeReRepository.Add(info);
            }
            return rst;
        }
        /// <summary>
        /// 判断团积分
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public int CheckTuanPoints(int gid,int point) 
        {
            int num=GroupInforRepository.Where(t=>t.gId==gid&&t.points>=point).Count();
            if (num>0)
            {
                return 1;
            }
            return 0;
        }

        public void UpdateTuanChatRoom(int gid)
        {
            var tuan = GroupInforRepository.First(t=>t.gId==gid);
            tuan.isChatRoom = 1;
            GroupInforRepository.Update(tuan);
        }
        public int AddChatRoom(string gid, string fid, string rname, string rpsw, string rmaxnum, string rtype, string rorder, string rmode)
        {
            try
            {
                ChatService.gameServiceSoapClient sc = new ChatService.gameServiceSoapClient();
                int ok = sc.addRoom(gid, fid, rname, rpsw, rmaxnum, rtype, rorder, rmode);
                return ok;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
    }
}
