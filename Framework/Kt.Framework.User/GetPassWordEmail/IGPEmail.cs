using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.GetPassWordEmail
{
    public interface IGPEmail
    {
        /// <summary>
        /// 从列表中删除
        /// </summary>
        /// <param name="ShortMessageModel"></param>
        void DeList(GPEmailModel GPEmailModel);

        /// <summary>
        /// 加入列表
        /// </summary>
        /// <param name="ShortMessageModel"></param>
        void EnList(GPEmailModel GPEmailModel);

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="ShortMessageModel"></param>
        void FreshList(GPEmailModel GPEmailModel);

        /// <summary>
        /// 是否频繁发送，true表示频繁发送（禁止），false表示正常发送（允许）
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        bool IsCorrectValidCode(GPEmailModel GPEmailModel);
    }
}
