using System;
using Dapper;
using GoProject.DataTableHelper;

namespace GoProject.Sample.Core
{
    public static class GoDiagramDbHelper
    {
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
                        Nodes = diagram.NodeDataArray.ToDataTable(),
                        Links = diagram.LinkDataArray.ToDataTable()
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