using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.ShortMessage
{
    public class MemShortMessage : IShortMessage
    {
        private static List<ShortMessageModel> list = new List<ShortMessageModel>();
        private static object lockobj = new object();
        public MemShortMessage()
        { }

        internal IEnumerable<ShortMessageModel> MessageList
        {
            get
            {
                lock (lockobj)
                {
                    return list;
                }
            }
        }

        public void DeList(ShortMessageModel ShortMessageModel)
        {
            list.Remove(ShortMessageModel);
        }

        public void EnList(ShortMessageModel ShortMessageModel)
        {
            list.Add(ShortMessageModel);
        }

        public void FreshList(ShortMessageModel ShortMessageModel)
        {
            var message = list.Where(x => x.Mobile == ShortMessageModel.Mobile).FirstOrDefault();
            if (list.Remove(message))
            {
                this.EnList(ShortMessageModel);
            }
        }

        private void SortList()
        {
            if (MessageList.Count() != 0)
            {
                list = MessageList.OrderByDescending(x => x.LastTime).ToList();
            }
        }

        public void ClearShortMessage(string Mobile)
        {
            var message = list.Where(x => x.Mobile == Mobile).FirstOrDefault();
            this.DeList(message);
            //以下，如果另外一个人在调用此方法，循环删除的时候，将另外的人的记录删除了，会出现问题
            //foreach (var m in MessageList)
            //{
            //    if (DateTime.Now >= m.LastTime.AddSeconds(ShortMessageConfig.TimeInterval))
            //    {
            //        this.DeList(m);
            //    }
            //}
        }

        /// <summary>
        /// 判断这个手机号是否是频繁发布的
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns>true:表示频繁发布，不应该继续发送  false:非频繁发布，可以发送短信</returns>
        public bool IsSendToMany(string Mobile)
        {
            var message = MessageList.FirstOrDefault(x => x.Mobile == Mobile);
            if (message != null)
            {
                if (DateTime.Now >= message.LastTime.AddSeconds(ShortMessageConfig.TimeInterval))
                {
                    FreshList(new ShortMessageModel { Mobile = message.Mobile, LastTime = DateTime.Now, SendTimes = message.SendTimes + 1 });
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                this.EnList(new ShortMessageModel { Mobile = Mobile, LastTime = DateTime.Now, SendTimes = 1 });
                return false;
            }

            /*首先判断该手机号码是否在短信发送列表中
             * 如果是：判断当前时间是否已经超过LastTime+TimeInterval
             *      如果是：列表中更新这个记录，返回false
             *          否：直接返回true
             *  否：在列表中插入这个记录，返回false
             *  
             * */
        }

        /// <summary>
        /// 根据手机号获取此手机发送过多少次短信
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public int GetSendTimes(string Mobile)
        {
            var message = MessageList.FirstOrDefault(x => x.Mobile == Mobile);
            if (message != null)
            {
                return message.SendTimes;
            }
            else
            {
                return 0;
            }
        }
    }
}
