using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using GoProject.DataTableHelper;

namespace GoProject.Sample.Core
{
    public static class GoDiagramDbHelper
    {
        internal static readonly Dictionary<string, IEnumerable<string>> TvpBuffer = new Dictionary<string, IEnumerable<string>>();

        public static IEnumerable<string> GetTableValueParameterColumnsOrder(this SqlConnection sqlConn, string tvpName)
        {
            var key = $"{sqlConn.DataSource}_{sqlConn.Database}_{tvpName}";

            if (TvpBuffer.ContainsKey(key)) return TvpBuffer[key];

            TvpBuffer[key] = sqlConn.Query<string>(
                "SELECT c.name FROM sys.table_types tt INNER JOIN sys.columns c ON c.object_id = tt.type_table_object_id WHERE tt.name = @tvpName ORDER BY c.column_id;",
                new { tvpName });

            return TvpBuffer[key];
        }

        public static bool StoreOnDb(this Diagram diagram, int userId = 0)
        {
            try
            {
                if (diagram == null) throw new ArgumentNullException(nameof(diagram));
                if (diagram.Name == null)
                    throw new NullReferenceException("The name of diagram must be none empty!");

                var res = Connections.GoProjectDb.SqlConn.Query<Diagram>("sp_InsertDiagramData",
                    new
                    {
                        DiagramId = diagram.Id,
                        DiagramName = diagram.Name,
                        DiagramClass = diagram.Class,
                        DiagramPosition = diagram.Position,
                        diagram.IsReadonly,
                        CreatorUserId = userId,
                        Nodes = diagram.NodeDataArray.ToDataTable(Connections.GoProjectDb.SqlConn.GetTableValueParameterColumnsOrder("Node")).AsTableValuedParameter("Node"),
                        Links = diagram.LinkDataArray.ToDataTable(Connections.GoProjectDb.SqlConn.GetTableValueParameterColumnsOrder("Link")).AsTableValuedParameter("Link")
                    },
                    commandType: System.Data.CommandType.StoredProcedure);

                return true;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
    }
}