using System.Diagnostics;

namespace Wonk
{
    // ports that are monitored for remote connections
    enum RemotePorts
    {
        SSH = 22,
        RDP = 3389,
        VNC = 5900,
        WINRM_HTTP = 5985,
        WINRM_HTTPS = 5986,
    }

    // actions that garner attention
    enum ImportantAction
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
}
