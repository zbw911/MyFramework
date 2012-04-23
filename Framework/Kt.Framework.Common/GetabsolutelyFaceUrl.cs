using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.Common
{
    class GetabsolutelyFaceUrl
    {
        public string GetFace(decimal userid, string size = "small")
        {
            var uid = userid.ToString();
            uid = (Convert.ToInt32(uid)).ToString("D9");
            var dir1 = uid.Substring(0, 3);  //取左边3位
            var dir2 = uid.Substring(3, 2);  //取4-5位
            var dir3 = uid.Substring(5, 2);  //取6-7位
            var dd = uid.Substring(7, 2);   //取8-9位
            var path = dir1 + "/" + dir2 + "/" + dir3 + "/" + dd + "_avatar_" + size + ".jpg";
            return path;
        }
    }
}
