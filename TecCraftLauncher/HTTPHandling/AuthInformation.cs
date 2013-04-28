using System;
using System.Collections.Generic;
using System.Text;

namespace TecCraftLauncher
{
    class AuthInformation
    {
        public String currentGameVersion;
        public String caseCorrectUsername;
        public String sessionId;
        public AuthInformation(String _currentGameVersion, String _caseCorrectUsername, String _sessionId)
        {
            currentGameVersion = _currentGameVersion;
            caseCorrectUsername = _caseCorrectUsername;
            sessionId = _sessionId;
        }
    }

}
