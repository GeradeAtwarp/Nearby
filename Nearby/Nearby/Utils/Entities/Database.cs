using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Utils.Entities
{
    public class Database
    {
        private SQLiteConnection _connection;
        public static string root;

        public Database()
        {
            var location = "Nearby.db3";
            location = Path.Combine(root, location);

            _connection = new SQLiteConnection(location);

            //Create tables
            _connection.CreateTable<UserPreferences>();
            _connection.CreateTable<FavoritePlaces>();
        }

        public T GetItem<T>(int? id) where T : IBaseEntity, new()
        {
            if (id != null)
                return _connection.Table<T>().Where(x => x.Id == id.Value).FirstOrDefault();
            else
                return _connection.Table<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetItems<T>() where T : IBaseEntity, new()
        {
            return (from i in _connection.Table<T>()
                    select i);
        }

        public int SaveItem<T>(T item) where T : IBaseEntity
        {
            if (item.Id != 0)
            {
                _connection.Update(item);
                return item.Id;
            }

            return _connection.Insert(item);
        }

        public void SaveItems<T>(IEnumerable<T> items) where T : IBaseEntity
        {
            _connection.BeginTransaction();

            foreach (T i in items)
            {
                SaveItem(i);
            }

            _connection.Commit();
        }


        public void RemoveItem<T>(T item) where T : IBaseEntity
        {
            _connection.Delete(item);
        }

        public void RemoveItems<T>(IEnumerable<T> items) where T : IBaseEntity
        {
            _connection.BeginTransaction();

            foreach (T i in items)
            {
                RemoveItem(i);
            }

            _connection.Commit();
        }
    }
}
