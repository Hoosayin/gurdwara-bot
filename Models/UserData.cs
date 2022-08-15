using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Models
{
    public class UserData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MealPreference { get; set; }
        public int Attendees { get; set; }
        public int NumberOfRooms { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<string> RoomIds { get; set; }
        public string RowKey { get; set; }
        public bool CanCancelDialog { get; set; } = false;
        public bool DataCollected { get; set; }
    }
}
