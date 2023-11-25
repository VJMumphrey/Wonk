using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Wonk
{
    public class Utils
    {
        // ports that are monitored for remote connections
        public enum RemotePorts
        {
            SSH = 22,
            RDP = 3389,
            VNC = 5900,
            WINRM_HTTP = 5985,
            WINRM_HTTPS = 5986,
        }

        // actions that garner attention
        public enum ImportantAction
        {
            SuccessfulLogon = 4624,
            ResetPassword = 4724,
            DomainPolicyChange = 4739,
            LogonWExpCreds = 4648,
            SpecialPrivLogon = 4672,
            InstallService = 4697,
            CreateSchTask = 4698,
            NewProcess = 4688,
            NewUserCreated = 4720,
            AttemptChangePasswd = 4723,
            AccessPasswordHash = 4782,
            TryToKillWonk = 4905,           // remove security event source
            FirewallExcpListModified = 4947,
            FirewallExcpListDeleted = 4948,
            FirewallSettingChanged = 4950,
            FirewallBlkIncomingToApp = 5031,
            ADDSModiefied = 5136,           // A directory service object was modified.
            HandleToObj = 4656,
            OppOnPrivObj = 4674,

        }

        public class UserProc : Process
        {
            public UserProc(string user, uint processId)
            {
                User = user;
                ProcessId = processId;
            }

            public string User { get; set; }

            public uint ProcessId { get; set; }

        }

        // setup the providers
        public static List<Tuple<string, int>> GenProvidersEvents()
        {
            List<Tuple<string, int>> providersAndEvents = new List<Tuple<string, int>>
            {
            Tuple.Create("Microsoft-Windows-Security-Auditing", 4624),      // successful logins
            Tuple.Create("Microsoft-Windows-SMBSever", 3000),               // access SMBv1 server
            Tuple.Create("Microsoft-Windows-WinINet", 0),                   // network traffic
            Tuple.Create("Microsoft-Windows-DNS-Client", 0),                // dns queries
            Tuple.Create("Microsoft-Windows-SMBClient", 0),                 // smb info
            Tuple.Create("Microsoft-Windows-Kernel-Process", 0),            // process info
            Tuple.Create("Microsoft-Windows-Kernel-File", 0),               // file and directory
            Tuple.Create("Microsoft-Windows-Kernel-Audit-API-Calls",  0),   // remote process termination
            Tuple.Create("Microsoft-Windows-PowerShell", 0),                // powerShell
            Tuple.Create("Microsoft-Windows-WinRM", 0)                      // WinRM

            // add more as needed
            };

            return providersAndEvents;
        }
    }
}
