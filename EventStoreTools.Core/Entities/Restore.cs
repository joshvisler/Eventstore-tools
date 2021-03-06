﻿using System;

namespace EventStoreTools.Core.Entities
{
    public class Restore
    {
        public Restore(int restoreId, int backupId, DateTime date, DateTime executedDate, string client, string status)
        {
            RestoreId = restoreId;
            BackupId = backupId;
            Date = date;
            ExecutedDate = executedDate;
            Client = client;
            Status = status;
        }

        public int RestoreId { get; private set; }
        public int BackupId { get; private set; }
        public DateTime Date { get; private set; } //start create  backup
        public DateTime ExecutedDate { get; private set; }// backup created date
        public string Client { get; private set; }
        public string Status { get; private set; }

    }
}

