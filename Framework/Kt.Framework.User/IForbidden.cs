///////////////////////////////////////////////////////////
//  IForbidden.cs
//  Implementation of the Interface IForbidden
//  Generated by Enterprise Architect
//  Created on:      05-七月-2011 12:32:27
//  Original author: 张保维
///////////////////////////////////////////////////////////




using Kt.Framework.User.Forbidden;
namespace Kt.Framework.User
{
    public interface IForbidden
    {

        /// <summary>
        /// 从列表中排除
        /// </summary>
        /// <param name="UserForbiddenModel">从队列中排除</param>
        void DeList(UserForbiddenModel UserForbiddenModel);

        /// <summary>
        /// 加入列表
        /// </summary>
        /// <param name="UserForbiddenModel"></param>
        void EnList(UserForbiddenModel UserForbiddenModel);

        /// <summary>
        /// 是否被拒绝
        /// </summary>
        /// <param name="Uid"></param>

        bool IsForbidden(decimal Uid);

        void ClearForbiddenList(decimal uid);
    }//end IForbidden

}//end namespace Forbidden