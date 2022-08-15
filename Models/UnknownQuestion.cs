using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class UnknownQuestion : TableEntity
    {
        public UnknownQuestion() { }

        public UnknownQuestion(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string Question { get; set; }
    }
}
