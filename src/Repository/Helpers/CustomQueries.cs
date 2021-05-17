using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Helpers
{
    public class CustomQueries
    {
        public const string GetByID = "select * from #table where ID = @ID";
        public const string DeleteByID = "Delete from #table where ID = @ID";
        public const string GetAll = "select * from #table";
        public const string GetAllWithOrder = "select * from #table order by #column #sortOrder";
        public const string GetCount = "select count(1) from #table";
        public const string GetAllPaged = "Select * from #table (nolock) ORDER BY #column #sortOrder OFFSET #offset ROWS FETCH NEXT #rows ROWS ONLY";
    }
}
