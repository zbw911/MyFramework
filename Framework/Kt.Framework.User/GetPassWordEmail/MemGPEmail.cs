using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.GetPassWordEmail
{
    public class MemGPEmail : IGPEmail
    {
        private static List<GPEmailModel> list = new List<GPEmailModel>();
        private static object lockobj = new object();
        public MemGPEmail()
        { }

        internal IEnumerable<GPEmailModel> MessageList
        {
            get
            {
                lock (lockobj)
                {
                    return list;
                }
            }
        }

        /// <summary>
        /// 从列表中删除
        /// </summary>
        /// <param name="ShortMessageModel"></param>
        public void DeList(GPEmailModel GPEmailModel)
        {
            list.Remove(GPEmailModel);
        }

        /// <summary>
        /// 加入列表
        /// </summary>
        /// <param name="ShortMessageModel"></param>
        public void EnList(GPEmailModel GPEmailModel)
        {
            list.Add(GPEmailModel);
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="ShortMessageModel"></param>
        public void FreshList(GPEmailModel GPEmailModel)
        {
            var message = list.Where(x => x.email == GPEmailModel.email).FirstOrDefault();
            if (list.Remove(message))
            {
                this.EnList(GPEmailModel);
            }
        }

        private void SortList()
        {
            if (MessageList.Count() != 0)
            {
                list = MessageList.OrderByDescending(x => x.dateline).ToList();
            }
        }


        public void ClearShortMessage(string email)
        {
            var message = list.Where(x => x.email == email).FirstOrDefault();
            this.DeList(message);
        }

        /// <summary>
        /// 是否频繁发送，true表示频繁发送（禁止），false表示正常发送（允许）
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool IsCorrectValidCode(GPEmailModel GPEmailModel)
        {
            //这里先要对authstr.id进行验证，要保证这个值和用户从邮箱中获取的值是一样的，
            //首先在发送邮件的时候，要记录下发送给用户的值是什么，可以采用服务器内存或者数据库来记录（安全考虑不采用cookie来记录）
            //这里我们采用服务器内存来记录
            //首先是记录下要验证的邮箱，然后记录下对应的ID值以及时间
            //在此处验证的时候，首先判断内存中是否有这个邮箱发送找回密码邮件的记录
            //如果有，进一步判断时间是否超过
            //如果没超过，则判断此处的值和数据库中记录的值是否相同
            //如果不同则不能满足此条件，不能显示下一页
            if (GPEmailModel == null)
            {
                return false;
            }

            var message = MessageList.FirstOrDefault(x => x.email == GPEmailModel.email);
            if (message == null)
            {
                return false;
            }
            else
            {
                if (DateTime.Now >= message.dateline.AddMinutes(GPEmailConfig.TimeInterval))//表示已经超时
                {
                    return false;
                }
                else
                {
                    if (GPEmailModel.valid == message.valid)
                    {
                        return true;//验证通过
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }
    }
}
