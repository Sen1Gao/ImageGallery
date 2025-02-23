using BasicArgs;
using Frontend.Arguments;
using Frontend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class StatementManager : IStatementManager
    {
        public SigninType SigninType { get; set; } = SigninType.None;
        public ImageCard CurrentImageCard { get; set; } = new ImageCard();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
