using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace NUBE.PAYROLL.SL.Hubs
{
    public partial class PYServerHub : Hub
    {
        #region Constant

        enum LogDetailType
        {
            INSERT,
            UPDATE,
            DELETE
        }

        #endregion

        #region Field

        private static DAL.PayrollEntities DB = new DAL.PayrollEntities();        

        #endregion

        public void Hello()
        {
            Clients.All.hello();
        }
    }
}