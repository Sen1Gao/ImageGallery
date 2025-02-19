using BasicArgs;
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
    }
}
