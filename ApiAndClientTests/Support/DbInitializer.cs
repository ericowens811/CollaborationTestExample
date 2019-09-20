using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApiAndClientTests.Support
{
    public class DbInitializer
    {
        public static DbContext InitializeWithReturnContext
        (
            SqliteConnection connection,
            Func<SqliteConnection, DbContext> getContext,
            List<IEnumerable<object>> itemLists
        )
        {
            var context = getContext(connection);

            context.Database.EnsureCreated();

            context.Database.ExecuteSqlCommand(
                @"
                    CREATE TRIGGER SetStatusTimestamp
                    AFTER UPDATE ON Resources
                    BEGIN
                        UPDATE Resources
                        SET Timestamp = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
            ");

            foreach (var itemList in itemLists)
            {
                context.AddRange(itemList);
            }

            context.SaveChanges();
            return context;
        }

        public static void Initialize
        (
            SqliteConnection connection,
            Func<SqliteConnection, DbContext> getContext,
            List<IEnumerable<object>> itemLists
        )
        {
            InitializeWithReturnContext(connection, getContext, itemLists);
        }
    }
}
