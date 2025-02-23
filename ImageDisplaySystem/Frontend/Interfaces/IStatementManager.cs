using BasicArgs;
using Frontend.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Interfaces
{
    public interface IStatementManager
    {
        SigninType SigninType { get; set; }

        ImageCard CurrentImageCard { get; set; }

        int Page { get; set; }
        int PageSize { get; set; }
    }
}
