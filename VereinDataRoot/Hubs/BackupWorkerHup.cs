namespace VereinDataRoot
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Ionic.Zip;
    using Microsoft.AspNet.SignalR;
    using Models;
    using Models.Backup;
    using Repository.Context;

    public class BackupWorkerHub : Hub
    {
        public void Start(string id, string mandantName)
        {
            Clients.Caller.addNewMessageToPage("Dateien vom Verein "+ mandantName.Trim() + " ermitteln von MandantId: " + id, 10);
            VereinBackup modelBackup = new VereinBackup();
            modelBackup.MandantId = int.Parse(id);
            modelBackup.MandantName = mandantName.Trim();

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
            string fileName = "Verein_Backup_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string pathFile = "C:\\VsProjekte\\www\\VereinDataRoot\\VereinDataRoot\\Backup\\" + fileName + ".xml";
            string pathZipFile = "C:\\VsProjekte\\www\\VereinDataRoot\\VereinDataRoot\\Backup\\" + fileName + ".zip";

            // Insert code to set properties and fields of the object.
            XmlSerializer mySerializer = new XmlSerializer(typeof(VereinBackup));
            // To write to a file, create a StreamWriter object.
            using (StreamWriter myWriter = new StreamWriter(pathFile))
            {
                mySerializer.Serialize(myWriter, modelBackup);
                myWriter.Close();
            }

            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFile(pathFile);
                    zip.Save(pathZipFile);
                }

                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

            }
            catch (IOException ex)
            {
                
                throw;
            }
            
            Clients.Caller.addNewMessageToPage("Vereinsbackup wurde erstellt: ", 99);
            Clients.Caller.addNewMessageToPage("Datei für den Download stet bereit: <a href='http://localhost:50999/backup/" + fileName + ".zip'>DownloadLink</a> ", 100);
        }
    }
}