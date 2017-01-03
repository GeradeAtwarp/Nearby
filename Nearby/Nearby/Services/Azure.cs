using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Nearby.Models;
using Nearby.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureService))]

namespace Nearby.Services
{
    public class AzureService
    {
        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<FavPlaceItem> favourites;

        public async Task Init()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "http://generalapp.azurewebsites.net/";

            //Create our client
            Client = new MobileServiceClient(appUrl);

            //InitialzeDatabase for path
            var path = "nearbysync.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);


            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<FavPlaceItem>();

            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            favourites = Client.GetSyncTable<FavPlaceItem>();
        }

        public async Task<IEnumerable<FavPlaceItem>> GetAllFavourites()
        {
            await Init();
            await SyncSpeakers();
            return await favourites.OrderBy(s => s.Name).ToEnumerableAsync();
        }

        public async Task SaveNewFav(FavPlaceItem fav)
        {
            try
            {
                await Init();
                await favourites.InsertAsync(fav);
                await SyncSpeakers();
            }
            catch (Exception ex) {
                Debug.WriteLine("Unable to insert new fav: " + ex);
            }
        }


        public async Task SyncSpeakers()
        {
            try
            {
                await Client.SyncContext.PushAsync();
                await favourites.PullAsync("FavPlaceItem", favourites.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync speakers, that is alright as we have offline capabilities: " + ex);
            }

        }

    }
}
