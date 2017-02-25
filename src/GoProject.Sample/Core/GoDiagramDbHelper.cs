using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using Dapper;
using GoProject.DataTableHelper;
using GoProject.Extensions;
using GoProject.Nodes;

namespace GoProject.Sample.Core
{
    public static class GoDiagramDbHelper
    {
        internal static readonly Dictionary<string, IEnumerable<string>> TvpBuffer = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        /// Get TVP columns order list by caching list on <see cref="TvpBuffer"/> for next fetch time.
        /// </summary>
        /// <param name="dbConn"></param>
        /// <param name="tvpName"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTableValueParameterColumnsOrder(this DbConnection dbConn, string tvpName)
        {
            var key = $"{dbConn.DataSource}_{dbConn.Database}_{tvpName}";

            if (TvpBuffer.ContainsKey(key)) return TvpBuffer[key];

            TvpBuffer[key] = dbConn.Query<string>(
                "SELECT c.name FROM sys.table_types tt INNER JOIN sys.columns c ON c.object_id = tt.type_table_object_id WHERE tt.name = @tvpName ORDER BY c.column_id;",
                new { tvpName });

            return TvpBuffer[key];
        }

        public static bool StoreOnDb(this DbConnection dbConn, Diagram diagram, int userId = 0)
        {
            try
            {
                if (diagram == null) throw new ArgumentNullException(nameof(diagram));
                if (diagram.Name == null)
                    throw new NullReferenceException("The name of diagram must be none empty!");

                var insertedDiagram = dbConn.Query<Diagram>("sp_InsertDiagramData",
                    new
                    {
                        DiagramId = diagram.Id,
                        DiagramName = diagram.Name,
                        DiagramClass = diagram.Class,
                        DiagramPosition = diagram.Position,
                        diagram.IsReadOnly,
                        CreatorUserId = userId,
                        Nodes = diagram.NodeDataArray.ToDataTable(dbConn.GetTableValueParameterColumnsOrder("Node")).AsTableValuedParameter("Node"),
                        Links = diagram.LinkDataArray.ToDataTable(dbConn.GetTableValueParameterColumnsOrder("Link")).AsTableValuedParameter("Link")
                    },
                    commandType: System.Data.CommandType.StoredProcedure).SingleOrDefault();

                return true;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        public static Diagram LoadFromDb(this DbConnection dbConn, string diagramId, int userId = 0, bool forceReadonly = false)
        {
            try
            {
                var diagram =
                    dbConn.Query<Diagram>("Select * From Diagrams Where Id = @diagramId", new { diagramId }).FirstOrDefault();

                if (diagram == null) return null;

                if (diagram.IsReadOnly == false)
                    diagram.IsReadOnly = forceReadonly; // set force to readonly when from database is false

                diagram.TreeNodes =
                    dbConn.Query<Node>("Select * From Nodes Where DiagramId = @diagramId", new { diagramId })?.ToList().ConvertToTreeNodes();

                diagram.LinkDataArray =
                    dbConn.Query<Link>("Select * From Links Where DiagramId = @diagramId", new { diagramId })?.ToList();

                return diagram;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        public static Diagram LoadFromCasDb(this DbConnection dbConn, string diagramId, int userId = 0, bool forceReadonly = false)
        {
            try
            {
                var diagram =
                    dbConn.Query<Diagram>("SELECT ExpenseCenterId AS Id, [NAME], UserId AS CreatorUserId FROM  dbo.ExpenseCenter Where ExpenseCenterId = @diagramId", new { diagramId }).FirstOrDefault();

                if (diagram == null) return null;

                if (diagram.IsReadOnly == false)
                    diagram.IsReadOnly = forceReadonly; // set force to readonly when from database is false

                using (var res = dbConn.QueryMultiple("sp_GetDiagramNodes", new { DiagramId = diagramId }, commandType: CommandType.StoredProcedure))
                {
                    var nodes = res.Read<Node>()?.ToList();
                    IEnumerable<ExpandoObject> nodeDetails = res.Read<dynamic>()?.ToExpandoObjects();

                    if (nodes != null && nodeDetails != null)
                    {
                        foreach (var node in nodes)
                        {
                            //node.Details = nodeDetails?.FirstOrDefault(d => string.Equals(((IDictionary<string, object>)d)["Key"].ToString(), node.Key))?.Where(x=>x.Key != "Key").Select(x=> (IDictionary<string, object>)x);
                            foreach (IDictionary<string, object> d in nodeDetails)
                            {
                                if (node.Key == d["Key"].ToString())
                                {
                                    d.Remove("Key");
                                    node.Details = d;
                                }
                            }
                        }

                        diagram.TreeNodes = nodes.ConvertToTreeNodes();
                    }

                    diagram.LinkDataArray = res.Read<Link>().ToList();
                }

                return diagram;
            }
            catch (Exception exp)
            {
                throw;
            }
        }


        public static IEnumerable<Node> GetPaletteNodesByUserRole(this DbConnection dbConn, int userId)
        {
            var shapes = dbConn.Query<Node>("SELECT * FROM fn_GetPaletteNodes(@UserId)", new { UserId = userId });

            return shapes;
        }
    }
}