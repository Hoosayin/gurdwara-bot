using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class User : TableEntity
    {
        public User() { }

        public User(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string MealPreference { get; set; }
        public int Attendees { get; set; }
    }
}
