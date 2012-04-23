using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.Enums
{
    public class GroupUserGrade
    {
        public enum 游戏团成员级别
        {
            团长, 副团长, 核心会员, 高级会员, 普通会员, 初来乍到
        }

        public class EnumsHelper<T>
        {
            public static IList<KeyValuePair<int, string>> GetList()
            {
                IList<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
                foreach (int values in Enum.GetValues(typeof(T)))
                {
                    list.Add(new KeyValuePair<int, string>(values, Enum.GetName(typeof(T), values)));
                }
                return list;
            }

        }
    }
}
