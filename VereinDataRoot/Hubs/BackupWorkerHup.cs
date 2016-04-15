using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Microsoft.AspNet.SignalR;
using Models;
using Models.Backup;
using Repository.Context;

namespace VereinDataRoot
{
    public class BackupWorkerHub : Hub
    {
        public void Start(string id)
        {
            Clients.Caller.addNewMessageToPage("Backup wird gestartet", 0);
            NewBackup(id);
        }

        private void NewBackup(string id)
        {
            Clients.Caller.addNewMessageToPage("Dateien ermitteln von MandantId: " + id, 10);
            VereinBackup modelBackup = new VereinBackup();
            modelBackup.MandantId = int.Parse(id);
            BackupMitglieder(modelBackup);
        }

        private void BackupMitglieder(VereinBackup modelBackup)
        {
            modelBackup.Mitglieder = new List<MitgliedBackup>();

            MitgliedRequest request = new MitgliedRequest();
            request.MandantId = modelBackup.MandantId;
            request.Skip = 0;
            request.Take = int.MaxValue;
            request.Filters = new List<TableFilter>();
            request.Sorting = new List<TableSort>();

            Mitglieder mitglieder = new Mitglieder();
            Clients.Caller.addNewMessageToPage("Mitglieder werden ermittelt: ", 20);
            List<MitgliedModel> l = mitglieder.GetMitglieder(request).Mitglieder;
            Clients.Caller.addNewMessageToPage(l.Count() + " Mitglieder gefunden: ", 30);
            List<MitgliedBackup> backupMitglieder = new List<MitgliedBackup>();

            foreach (MitgliedModel item in l)
            {
                MitgliedBackup m = new MitgliedBackup();
                m.MitgliedId = item.MitgliedId;
                m.MandantId = item.MandantId;
                m.Vorname = item.Vorname;
                m.Nachname = item.Name;

                backupMitglieder.Add(m);
            }

            modelBackup.Mitglieder = backupMitglieder;

            CreateFile(modelBackup);
        }

        private void CreateFile(VereinBackup modelBackup)
        {
            string fileName = DateTime.Now.ToString() + "_myFileName";
            string pathFile = @"C:\VsProjekte\www\VereinDataRoot\VereinDataRoot\Backup\" + fileName + ".xml";
            string pathZipFile = @"C:\VsProjekte\www\VereinDataRoot\VereinDataRoot\Backup\" + fileName + ".zip";

            // Insert code to set properties and fields of the object.
            XmlSerializer mySerializer = new XmlSerializer(typeof(VereinBackup));
            // To write to a file, create a StreamWriter object.
            using (StreamWriter myWriter = new StreamWriter(pathFile))
            {
                mySerializer.Serialize(myWriter, modelBackup);
                myWriter.Close();
            }

            ZipFile.CreateFromDirectory(pathFile, pathZipFile, CompressionLevel.Optimal, true);


            Clients.Caller.addNewMessageToPage("Vereinsbackup wurde erstellt: ", 30);
            Clients.Caller.addNewMessageToPage("Datei für den Download stet bereit: <a href='http://localhost:50994/backup/" + fileName + ".zip'>DownloadLink</a> ", 30);
        }
    }
}