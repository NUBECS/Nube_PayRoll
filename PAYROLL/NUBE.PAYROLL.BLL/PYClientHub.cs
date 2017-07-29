using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUBE.PAYROLL.BLL
{
    public static class PYClientHub
    {
        #region Field

        private static HubConnection _hubCon;
        private static IHubProxy _PYHub;

        #endregion

        #region Property

        public static HubConnection Hubcon
        {
            get
            {
                if (_hubCon == null) HubConnect();
                return _hubCon;
            }
            set
            {
                _hubCon = value;
            }
        }

        public static IHubProxy PYHub
        {
            get
            {
                if (_PYHub == null) HubConnect();
                if (Hubcon.State != ConnectionState.Connected) HubConnect();
                return _PYHub;
            }
            set
            {
                _PYHub = value;
            }
        }

        #endregion

        #region Method

        public static void HubConnect()
        {
            //            _hubCon = new HubConnection("http://110.4.40.46/fmcgsl/SignalR");
            _hubCon = new HubConnection("http://localhost:51068/SignalR");
            _PYHub = _hubCon.CreateHubProxy("PYServerHub");
            _hubCon.Start(new LongPollingTransport()).Wait();
        }

        public static void HubDisconnect()
        {
            _hubCon.Stop();
        }

        #endregion

    }
}
