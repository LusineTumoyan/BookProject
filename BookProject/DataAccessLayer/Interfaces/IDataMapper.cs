using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface IDataMapper
    {
        void MapEntity(SqlDataReader reader);
    }
}