using GurdwaraBot.AzureCosmosDB;
using GurdwaraBot.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class CosmosDBFactory
    {
        private static readonly string _roomsTable = "GurdwaraRooms";
        private static readonly string _usersTable = "GurdwaraUsers";
        private static readonly string _userRoomsTable = "GurdwaraUserRooms";
        private static readonly string _unknownQuestionsTable = "UnknownQuestions";
        private static readonly int _numberOfRooms = 367;

        private static async Task<Room> RetrieveGurdwaraRoomAsync(CloudTable table, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<Room>(partitionKey, rowKey);
                TableResult tableResult = await table.ExecuteAsync(retrieveOperation);
                Room room = tableResult.Result as Room;
                return room;
            }
            catch (StorageException)
            {
                throw;
            }
        }

        private static async Task<TableEntity> InsertOrMergeGurdwaraEntityAsync(CloudTable table, TableEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException("Entity not Found.");
            }

            try
            {
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                TableEntity insertedEntity = result.Result as TableEntity;
                return insertedEntity;
            }
            catch (StorageException)
            {
                throw;
            }
        }

        public static async Task<bool> AreRoomsAvailableAsync(int numberOfRooms)
        {
            int count = 0;
            CloudTable table = await Common.CreateTableAsync(_roomsTable);
            Room room;

            for (int i = 1; i <= _numberOfRooms && count < numberOfRooms; i++)
            {
                room = await RetrieveGurdwaraRoomAsync(table, "Room", i.ToString());

                if (!room.IsBooked)
                {
                    count++;
                }
            }

            return count == numberOfRooms ? true : false;
        }

        public static async Task<UserData> AssignGurdwaraRoomsAsync(UserData userData)
        {
            List<string> roomIds = new List<string>();
            CloudTable table = await Common.CreateTableAsync(_roomsTable);
            Room room;

            for (int i = 1; i <= _numberOfRooms && roomIds.Count < userData.NumberOfRooms; i++)
            {
                room = await RetrieveGurdwaraRoomAsync(table, "Room", i.ToString());

                if (!room.IsBooked)
                {
                    room.IsBooked = true;
                    await InsertOrMergeGurdwaraEntityAsync(table, room);
                    roomIds.Add(room.RowKey);
                }
            }

            userData.RoomIds = roomIds;
            return userData;
        }

        public static async Task<UserData> InsertGurdwaraUserAsync(UserData userData)
        {
            CloudTable table = await Common.CreateTableAsync(_usersTable);
            string rowKey = Guid.NewGuid().ToString().Substring(0, 10).ToUpper();

            User user = new User("User", rowKey)
            {
                Name = userData.Name,
                Email = userData.Email,
                Attendees = userData.Attendees,
                MealPreference = userData.MealPreference,
            };

            userData.RowKey = await InsertOrMergeGurdwaraEntityAsync(table, user) != null ? rowKey : string.Empty;
            return userData;
        }

        public static async Task<bool> InsertUserRoomsAsync(UserData userData)
        {
            CloudTable table = await Common.CreateTableAsync(_userRoomsTable);

            foreach (string roomId in userData.RoomIds)
            {
                UserRoom userRoom = new UserRoom(userData.RowKey, roomId)
                {
                    CheckInDate = userData.CheckInDate,
                    CheckOutDate = userData.CheckOutDate,
                    ExpirationDate = userData.ExpirationDate,
                };

                if (await InsertOrMergeGurdwaraEntityAsync(table, userRoom) == null)
                {
                    return false;
                }
            }

            return true;
        }

        public static async Task InsertUnknownQuestionAsync(string question)
        {
            CloudTable table = await Common.CreateTableAsync(_unknownQuestionsTable);
            string rowKey = Guid.NewGuid().ToString().Substring(0, 10).ToUpper();

            UnknownQuestion unknownQuestion = new UnknownQuestion("Question", rowKey)
            {
                Question = question,
            };

            await InsertOrMergeGurdwaraEntityAsync(table, unknownQuestion);
        }

        public static async Task<bool> IsEmailEligibleAsync(string email)
        {
            CloudTable table = await Common.CreateTableAsync(_usersTable);

            try
            {
                TableQuery<User> partitionScanQuery = new TableQuery<User>().Where(TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));
                TableContinuationToken token = null;

                do
                {
                    TableQuerySegment<User> segment = await table.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                    token = segment.ContinuationToken;

                    if (segment.Count<User>() == 0)
                    {
                        return true;
                    }
                }
                while (token != null);
            }
            catch (StorageException)
            {
                throw;
            }

            return false;
        }

        public static bool IsAzureCosmosdbTable()
        {
            string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;
            return !string.IsNullOrEmpty(storageConnectionString) && (storageConnectionString.Contains("table.cosmosdb") || storageConnectionString.Contains("table.cosmos"));
        }
    }
}
