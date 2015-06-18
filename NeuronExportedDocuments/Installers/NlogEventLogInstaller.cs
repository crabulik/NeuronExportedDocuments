using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.ComponentModel;


namespace NeuronExportedDocuments.Installers
{
    [RunInstaller(true)]
    public class NlogEventLogInstaller: Installer
    {
        private EventLogInstaller myEventLogInstaller;

        public NlogEventLogInstaller() 
        {
            // Create an instance of an EventLogInstaller.
            myEventLogInstaller = new EventLogInstaller();

            // Set the source name of the event log.
            myEventLogInstaller.Source = "NeuronExportedDocuments";

            // Set the event log that the source writes entries to.
            myEventLogInstaller.Log = "Application";

            // Add myEventLogInstaller to the Installer collection.
            Installers.Add(myEventLogInstaller);   
        }
    }
}