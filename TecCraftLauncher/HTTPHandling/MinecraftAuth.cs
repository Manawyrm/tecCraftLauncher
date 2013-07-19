using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace TecCraftLauncher
{
    /*
     * TecCraftLauncher
     * Geschrieben 2011-2013 von Tobias Mädel (t.maedel@alfeld.de)
     * http://tbspace.de
     */
    delegate void _LoginSuceeded(AuthInformation user);
    delegate void _LoginFailed(String reason);

    class MinecraftAuth
    {
        public event _LoginSuceeded LoginSuceeded;
        public event _LoginFailed LoginFailed;

        public void Login (String username, String password)
        {
            _username = username;
            _password = password;
            Thread auththread = new Thread(LoginSync);
            auththread.Start();
        }
        void LoginSync()
        {
            HttpPost authrequest = new HttpPost("http://login.minecraft.net");
            PostParamCollection authparams = new PostParamCollection();

            authparams.Add(new PostParam("user", _username));
            authparams.Add(new PostParam("password", _password));
            authparams.Add(new PostParam("version", "13"));

            authrequest.Proxy = null;
            authrequest.Language = "en-US";
            authrequest.PostComplete += new TecCraftLauncher._PostComplete(authrequest_PostComplete);

            authrequest.doPost(authparams);
        }
        String _username;
        String _password; 

        void authrequest_PostComplete(String data)
        {
            if(data.Contains(":"))
            {
                //Login erfolgreich!
                //Die zurückgegebenen Daten werden durch Doppelpunkte getrennt:
                //1343825972000:deprecated:SirCmpwn:7ae9007b9909de05ea58e94199a33b30c310c69c:dba0c48e1c584963b9e93a038a66bb98
                String[] logindata = data.Trim().Split(':');
                if (logindata[0].Contains("error.System.Net.Sockets.SocketException"))
                {
                    LoginFailed(data.Trim());
                    return;
                }
                try
                {
                    AuthInformation authdata = new AuthInformation(logindata[0].Trim(), logindata[2].Trim(), logindata[3].Trim());
                    LoginSuceeded(authdata);
                }
                catch (Exception)
                {
                    LoginFailed(data.Trim());
                }
             
            }
            else
            {
                LoginFailed(data.Trim());
            }
        }

   }
}
