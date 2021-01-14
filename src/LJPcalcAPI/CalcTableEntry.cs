using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace LJPcalc.API
{
    class CalcTableEntry : TableEntity
    {
        public string Description { get; set; }
        public double Result { get; set; }
        public double ExecutionTime { get; set; }
        public string IP { get; set; }

        public CalcTableEntry(string description, double result, double executionTime, string ip)
        {
            PartitionKey = "CalcEntry";
            RowKey = Guid.NewGuid().ToString();
            Description = description;
            Result = result;
            ExecutionTime = executionTime;
            IP = ip;
        }
    }
}
