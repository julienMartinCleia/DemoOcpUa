/******************************************************************************
**
** <auto-generated>
**     This code was generated by a tool: UaModeler
**     Runtime Version: 1.6.11, using .NET Server 3.4.0 template (version 0)
**
**     This is a template file that was generated for your convenience.
**     This file will not be overwritten when generating code again.
**     ADD YOUR IMPLEMTATION HERE!
** </auto-generated>
**
** Copyright (c) 2006-2024 Unified Automation GmbH All rights reserved.
**
** Software License Agreement ("SLA") Version 2.8
**
** Unless explicitly acquired and licensed from Licensor under another
** license, the contents of this file are subject to the Software License
** Agreement ("SLA") Version 2.8, or subsequent versions
** as allowed by the SLA, and You may not copy or use this file in either
** source code or executable form, except in compliance with the terms and
** conditions of the SLA.
**
** All software distributed under the SLA is provided strictly on an
** "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED,
** AND LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
** LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
** PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the SLA for specific
** language governing rights and limitations under the SLA.
**
** Project: .NET OPC UA SDK information model for namespace http://yourorganisation.org/DemoOpcUa/
**
** Description: OPC Unified Architecture Software Development Kit.
**
** The complete license agreement can be found here:
** http://unifiedautomation.com/License/SLA/2.8/
**
** Created: 20.03.2024
**
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnifiedAutomation.UaBase;
using UnifiedAutomation.UaServer;
using UnifiedAutomation.UaSchema;

namespace cleia.DemoOpcUa
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // There is no license file configured in UaModeler.
                // After you have added a license file to the project you can add the license with the following
                // line of code.
                // ApplicationLicenseManager.AddProcessLicenses(System.Reflection.Assembly.GetExecutingAssembly(), "License.lic");

                // Start the server.
                ServerManager server = new TestServerManager();
#if WINDOWS
                ApplicationInstance.Default.AutoCreateCertificate = true;
                ApplicationInstance.Default.Start(server, null, server);
#else
                ApplicationInstanceBase.Default.AutoCreateCertificate = true;
                ApplicationInstanceBase.Default.SecurityProvider = new BouncyCastleSecurityProvider();
                ApplicationInstanceBase.Default.Start(server, null, server);
#endif
                Console.WriteLine("Endpoint URL: opc.tcp://localhost:48030");
                // Block until the server exits.
                Console.WriteLine("Press <enter> to exit the program.");
                Console.ReadLine();
                // Stop the server.
                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                Console.WriteLine("Press <enter> to exit the program.");
                Console.ReadLine();
            }
        }

        static void ConfigureOpcUaApplicationFromCode()
        {
            // fill in the application settings in code
            // The configuration settings are typically provided by another module
            // of the application or loaded from a data base. In this example the
            // settings are hardcoded
            var application = new ConfigurationInMemory();

            // ***********************************************************************
            // standard configuration options

            // general application identification settings
            application.ApplicationName = "DemoOpcUa";
            application.ApplicationUri = "urn:localhost:cleia:DemoOpcUa";
            application.ApplicationType = UnifiedAutomation.UaSchema.ApplicationType.Server_0;
            application.ProductName = "DemoOpcUa";

            // configure certificate stores
            application.ApplicationCertificate = new UnifiedAutomation.UaSchema.CertificateIdentifier();
            application.ApplicationCertificate.StoreType = "Directory";
            application.ApplicationCertificate.StorePath = @"%CommonApplicationData%\cleia\DemoOpcUa\pki\own";
            application.ApplicationCertificate.SubjectName = "CN=GettingStartedServer/O=cleia/DC=localhost";

            application.TrustedCertificateStore = new UnifiedAutomation.UaSchema.CertificateStoreIdentifier();
            application.TrustedCertificateStore.StoreType = "Directory";
            application.TrustedCertificateStore.StorePath = @"%CommonApplicationData%\cleia\DemoOpcUa\pki\trusted";

            application.IssuerCertificateStore = new UnifiedAutomation.UaSchema.CertificateStoreIdentifier();
            application.IssuerCertificateStore.StoreType = "Directory";
            application.IssuerCertificateStore.StorePath = @"%CommonApplicationData%\cleia\DemoOpcUa\pki\issuers";

            application.RejectedCertificatesStore = new UnifiedAutomation.UaSchema.CertificateStoreIdentifier();
            application.RejectedCertificatesStore.StoreType = "Directory";
            application.RejectedCertificatesStore.StorePath = @"%CommonApplicationData%\cleia\DemoOpcUa\pki\rejected";

            // configure endpoints
            application.BaseAddresses = new UnifiedAutomation.UaSchema.ListOfBaseAddresses();
            application.BaseAddresses.Add("opc.tcp://localhost:48030");

            application.SecurityProfiles = new ListOfSecurityProfiles();
            application.SecurityProfiles.Add(new SecurityProfile() { ProfileUri = SecurityProfiles.Basic256Sha256, Enabled = true });
            application.SecurityProfiles.Add(new SecurityProfile() { ProfileUri = SecurityProfiles.Aes128Sha256RsaOaep, Enabled = true });
            application.SecurityProfiles.Add(new SecurityProfile() { ProfileUri = SecurityProfiles.Aes256Sha256RsaPss, Enabled = true });
            application.SecurityProfiles.Add(new SecurityProfile() { ProfileUri = SecurityProfiles.None, Enabled = true });
            // ***********************************************************************

            // ***********************************************************************
            // extended configuration options

            // trace settings
            TraceSettings trace = new TraceSettings();

            trace.MasterTraceEnabled = false;
            trace.DefaultTraceLevel = UnifiedAutomation.UaSchema.TraceLevel.Info;
            trace.TraceFile = @"%CommonApplicationData%\cleia\logs\DemoOpcUa.log.txt";
            trace.MaxLogFileBackups = 3;

            trace.ModuleSettings = new ModuleTraceSettings[]
                {
                    new ModuleTraceSettings() { ModuleName = "UnifiedAutomation.Stack", TraceEnabled = true },
                    new ModuleTraceSettings() { ModuleName = "UnifiedAutomation.Server", TraceEnabled = true },
                };

            application.Set<TraceSettings>(trace);

            // Installation settings
            InstallationSettings installation = new InstallationSettings();

            installation.GenerateCertificateIfNone = true;
            installation.DeleteCertificateOnUninstall = true;

            application.Set<InstallationSettings>(installation);

            application.ServerSettings = new UnifiedAutomation.UaSchema.ServerSettings()
            {
                ProductName = "DemoOpcUa",
                DiscoveryRegistration = new DiscoveryRegistrationSettings()
                {
                    Enabled = false
                },
                UserIdentity = new UserIdentitySettings()
                {
                    EnableAnonymous = true,
                    EnableUserName = true
                }
            };
            // ***********************************************************************

            // set the configuration for the application (must be called before start to have any effect).
            // these settings are discarded if the /configFile flag is specified on the command line.
            ApplicationInstanceBase.Default.SetApplicationSettings(application);
        }
    }
}

