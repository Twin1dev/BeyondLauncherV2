using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeyondLauncherV2.Utilities
{
    internal class HwidBanning
    {
        public bool CheckForBan()
        {
            using (WebClient wc = new())
            {
                wc.Headers.Add("authkey", SimpleUtils.TimeStampEncryption());
                string res = wc.DownloadString("http://backend.beyondfn.xyz:8990/backend/hwid/fafafa/isBannedSmokearr");

                MessageBox.Show(res);
            }

            return true;
        }
    }
}
