using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class WbapAction
    {
        List<ClientAction> clientActions = new List<ClientAction>();
        WbapRequest request = new WbapRequest();

        public List<ClientAction> ClientActions
        {
            get { return clientActions; }
            set { clientActions = value; }
        }

        public WbapRequest Request
        {
            get { return request; }
            set { request = value; }
        }
    }
}
