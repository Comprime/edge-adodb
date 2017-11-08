using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;

namespace edge_adodb
{
    public class Startup
    {
        public async Task<object> Invoke(object input)
        {
            return new
            {
                Query = (Func<object,Task<object>>)this.QueryAsync,
                Execute = (Func<object,Task<object>>)this.ExecuteAsync
            };
            /*
            string connStr = (string)input;
            Connection cn = new Connection();
            Recordset rs = new Recordset();
            cn.Open(connStr, null, null, 0);
            cn.Close();
            return input;
            */
        }
        public async Task<object> QueryAsync(object _t){
            IDictionary<string,object> arg = _t as IDictionary<string,object>;
            string connection = arg["connection"].ToString();
            string query = arg["query"].ToString();
            return await Task.Run(()=>this.Query(connection, query));
        }

        public object Query(string conn, string query){
            var cn = new Connection();
            var rs = new Recordset();
            var ret = new List<IDictionary<string, object>>();
            cn.Open(conn, null, null, -1);
            rs.Open(query, cn, ADODB.CursorTypeEnum.adOpenKeyset);
            rs.MoveFirst();
            while(!rs.EOF){
                var o = new Dictionary<string, object>();
                foreach(Field entry in rs.Fields){
                    o[entry.Name] = entry.Value;
                }
                ret.Add(o);
                rs.MoveNext();
            }
            rs.Close();
            cn.Close();
            return ret;
        }

        public async Task<object> ExecuteAsync(object _t){
            IDictionary<string,object> arg = _t as IDictionary<string,object>;
            string connection = arg["connection"].ToString();
            string query = arg["query"].ToString();
            return await Task.Run(()=>this.Execute(connection, query));
        }
        public object Execute(string conn, string query){
            var cn = new Connection();
            cn.Open(conn, null, null, -1);
            object affected = Type.Missing;
            var rs = cn.Execute(query, out affected, -1);
            rs.MoveFirst();
            var o = new Dictionary<string, object>();
            foreach(Field entry in rs.Fields){
                o[entry.Name] = entry.Value;
            }
            rs.Close();
            cn.Close();
            return new {
                affected = affected,
                result = o
            };
        }
    }
}