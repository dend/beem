using Beem.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beem.Azure
{
    public static class MobileServiceClientHelper
    {
        public static async Task<List<Station>> GetAllStations()
        {
            IMobileServiceTable<Station> itemsTable = App.AzureClient.GetTable<Station>();

            List<Station> itemsList =
                await itemsTable.Take(100).ToListAsync();

            if (itemsList.Count != 0)
            {
                return itemsList;
            }
            else
            {
                return null;
            }
        }
    }
}
