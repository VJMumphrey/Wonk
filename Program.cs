﻿using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Wonk
{
    class Program
    {
        public static async Task Main()
        {
            Banner.PrintBanner();

            if (IsAdministrator())
            {
                IDictionary<string, string> RemoteConnections = GatherConns();

                List<Utils.UserProc> usernames = GatherProcs();

                var eventIDs = await EventTracing();

                // TODO Wonk the user
                await Wonked(eventIDs, RemoteConnections);
            }
            else
            {
                Console.WriteLine("Make sure you have the right privs noob");
            }
        }

        // used to make sure that the account is the built-in Administrator
        private static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // TODO Change to Use events to keep track of this. global value?
        private static IDictionary<string, string> GatherConns()
        {
            IDictionary<string, string> RemoteConnections = new Dictionary<string, string>();

            // supposed to show all active TCP connections
            // from the docs,
            // https://learn.microsoft.com/en-us/dotnet/api/system.net.networkinformation.ipglobalproperties.getactivetcpconnections?view=net-7.0#system-net-networkinformation-ipglobalproperties-getactivetcpconnections
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnections = properties.GetActiveTcpConnections();

            // strip the port number out of the local endpoint
            foreach (TcpConnectionInformation connection in tcpConnections)
            {
                if (Enum.TryParse(connection.LocalEndPoint.ToString(), out Utils.RemotePorts _))
                {
                    RemoteConnections.Add(connection.LocalEndPoint.ToString(), connection.LocalEndPoint.Port.ToString());
                }
                else
                {
                    continue;
                }
            }
            return RemoteConnections;
        }

        // gather the processes on the system with the username along with it
        private static List<Utils.UserProc> GatherProcs()
        {
            // The call to InvokeMethod below will fail if the Handle property is not retrieved
            string[] propertiesToSelect = new[] { "Handle", "ProcessId" };
            SelectQuery processQuery = new SelectQuery("Win32_Process", "Name = 'taskhost.exe'", propertiesToSelect);
            List<Utils.UserProc> users = new List<Utils.UserProc>();

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
            using (ManagementObjectCollection processes = searcher.Get())
                foreach (ManagementObject process in processes)
                {
                    object[] outParameters = new object[2];
                    uint result = (uint)process.InvokeMethod("GetOwner", outParameters);

                    if (result == 0)
                    {
                        string username = (string)outParameters[0];
                        // string domain = (string)outParameters[1];
                        uint processId = (uint)process["ProcessId"];

                        // set the class objects and store in a list
                        users.Add(new Utils.UserProc(username, processId));
                    }
                    else
                    {
                        // TODO handle failure...
                    }
                }
            return users;
        }

        private static async Task<List<int>> EventTracing()
        {
            List<Tuple<string, int>> providersAndEvents = Utils.GenProvidersEvents();

            var eventIDs = new List<int>();

            TraceEventSession session = null;
            using (session = new TraceEventSession("WonkWatch"))
            {
                // enable provider and event ID in the list.
                foreach (var providerAndEvent in providersAndEvents)
                {
                    string providerName = providerAndEvent.Item1;
                    int eventID = providerAndEvent.Item2;
                    session.EnableProvider(providerName, TraceEventLevel.Verbose, (ulong)eventID);
                }

                var taskCompletionSource = new TaskCompletionSource<bool>();

                session.Source.Dynamic.All += delegate (TraceEvent data)
                {
                    foreach (var providerAndEvent in providersAndEvents)
                    {
                        string providerName = providerAndEvent.Item1;
                        int eventID = providerAndEvent.Item2;

                        if (data.ProviderName == providerName && (int)data.ID == eventID)
                        {
                            taskCompletionSource.SetResult(true);
                            ProcessEventIDs(data, eventIDs);
                        }
                    }
                };

                session.Source.Clr.All += delegate (TraceEvent data)
                {
                    // TODO figure out how to fix/cleanup and shutdown on errors
                    // close the session and cleanup
                    session.Dispose();
                };

                // start listening for events
                await Task.Run(() => session.Source.Process());
            }

            return eventIDs;
        }

        // just to keep things async
        private static void ProcessEventIDs(TraceEvent data, List<int> eventIDs)
        {
            eventIDs.Add((int)data.ID);
        }

        // Wonks the intended user for committing an offensive action
        private static async Task<int> Wonked(List<int> eventIDs, IDictionary<string, string> remote_connections)
        {
            // kill the main process of the users connection
            List<Utils.UserProc> users = GatherProcs();

            foreach (Utils.UserProc process in users)
            {
                process.Kill();
            }

            return 0;
        }
    }
}
