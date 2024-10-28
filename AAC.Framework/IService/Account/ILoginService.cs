using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Web.Model.Account;

namespace AAC.Framework.IService.Account
{
    public interface ILoginService
    {
        LoginModel LetMeIn( LoginModel model );
    }
}
