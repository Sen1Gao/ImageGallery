using Castle.Windsor;
using Frontend.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Frontend
{
    public class ContainerHelper : IContainerHelper
    {
        private IWindsorContainer? container;

        public ContainerHelper()
        {
        }

        public IWindsorContainer Container
        {
            get
            {
                if (container != null)
                {
                    return container;
                }
                else
                {
                    throw new NullReferenceException("The variable 'container' must be assigned before get!");
                }
            }
            set => container = value;
        }
    }
}
