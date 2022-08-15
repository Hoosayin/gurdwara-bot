using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace GurdwaraBot.Models
{
    public class Room : TableEntity
    {
        public Room() { }

        public Room(string pk, string rk)
        {
            PartitionKey = pk;
            RowKey = rk;
        }

        public bool IsBooked { get; set; } = false;
    }
}
