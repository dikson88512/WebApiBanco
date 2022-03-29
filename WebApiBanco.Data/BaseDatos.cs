using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;


namespace WebApiBanco.Data
{
    public class BaseDatos : IBaseDatos
    {

        private string Conexion;
        public BaseDatos(IConfiguration configuration)
        {
            Conexion = configuration.GetConnectionString("SkyknoxConn");
        }

        public DbConnection ObtenerConexion()
        {
            DbConnection connection = new SqlConnection(Conexion);
            return connection;
        }


        public async Task<List<T>> ListarData<T>(string SQLQuery)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Conexion))
                {
                    await con.OpenAsync();
                    var List = con.Query<T>(SQLQuery).ToList();
                    return List;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Obtener un Objeto
        public async Task<T> ConsultarEntidad<T>(string SQLQuery)
        {
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                await con.OpenAsync();
                T obj = con.Query<T>(SQLQuery).SingleOrDefault();
                return obj;
            }

        }

        public async Task<Resultado> ActualizarData(string SQLQuery)
        {
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                await con.OpenAsync();

                using (SqlCommand command = new SqlCommand(SQLQuery, con))
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {
                        var Resul = new Resultado();
                        Resul.mensaje_control = dataTable.Rows[0][0].ToString();
                        Resul.error = Int32.Parse(dataTable.Rows[0][1].ToString());
                        Resul.respuesta_1 = dataTable.Rows[0][2].ToString();

                        return Resul;
                    }
                }
            }
            return null;
        }



        //para hacer transacciones anidadas**************************


        public async Task<int> CreateTransactionScope(
            string connectString1, string connectString2,
            string commandText1, string commandText2)
        {
            // Initialize the return value to zero and create a StringWriter to display results.
            int returnValue = 0;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection1 = new SqlConnection(connectString1))
                    {
                        // Opening the connection automatically enlists it in the 
                        // TransactionScope as a lightweight transaction.
                        await connection1.OpenAsync();

                        // Create the SqlCommand object and execute the first command.
                        SqlCommand command1 = new SqlCommand(commandText1, connection1);
                        returnValue = await command1.ExecuteNonQueryAsync();
                        writer.WriteLine("Rows to be affected by command1: {0}", returnValue);

                        // If you get here, this means that command1 succeeded. By nesting
                        // the using block for connection2 inside that of connection1, you
                        // conserve server and network resources as connection2 is opened
                        // only when there is a chance that the transaction can commit.   
                        using (SqlConnection connection2 = new SqlConnection(connectString2))
                        {
                            // The transaction is escalated to a full distributed
                            // transaction when connection2 is opened.
                            await connection2.OpenAsync();

                            // Execute the second command in the second database.
                            returnValue = 0;
                            SqlCommand command2 = new SqlCommand(commandText2, connection2);
                            returnValue = await command2.ExecuteNonQueryAsync();
                            writer.WriteLine("Rows to be affected by command2: {0}", returnValue);
                        }
                    }

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();

                }

            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            // Display messages.
            Console.WriteLine(writer.ToString());

            return returnValue;
        }

        public async Task<Resultado> ActualizarData(string SQLQuery, List<SqlParameter> parameters)
        {

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                await con.OpenAsync();

                using (SqlCommand command = new SqlCommand(SQLQuery, con))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {
                        var Resul = new Resultado();
                        Resul.mensaje_control = dataTable.Rows[0][0].ToString();
                        Resul.error = Int32.Parse(dataTable.Rows[0][1].ToString());
                        Resul.respuesta_1 = dataTable.Rows[0][2].ToString();

                        return Resul;
                    }
                }

            }

            return null;
        }


        public async Task<Resultado> ActualizarData(List<SqlCommand> commands)
        {
            Resultado resultado = new Resultado
            {
                mensaje_control = "No se ejecuto ninguna inserción",
                error = 2
            };

            using (SqlConnection con = new SqlConnection(Conexion))
            {
                try
                {
                    await con.OpenAsync();

                    foreach (var command in commands)
                    {
                        command.Connection = con;

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        if (dataTable.Rows.Count > 0)
                        {
                            resultado = new Resultado();
                            resultado.mensaje_control = dataTable.Rows[0][0].ToString();
                            resultado.error = Int32.Parse(dataTable.Rows[0][1].ToString());
                            resultado.respuesta_1 = dataTable.Rows[0][2].ToString();

                            if (resultado.error == 2)
                            {
                                throw new Exception("Error ejecutando un stored procedure " + command.CommandAsSql());
                            }
                        }
                    }

                }
                catch (Exception)
                {
                }
            }
            //}

            return resultado;
        }

        //Para ejecutar funciones de validación como dbo.digito_verificador_cedula_ecuador('099001094001')
        //que valida el la Cedula o Ruc
        public async Task<string> RespuestaActualizacion(string SQLQuery)
        {
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                await con.OpenAsync();
                try
                {
                    using (SqlCommand command = new SqlCommand(SQLQuery, con))
                    {
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        if (dataTable.Rows.Count > 0)
                        {
                            string resul;
                            resul = dataTable.Rows[0][0].ToString();
                            return resul;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return null;
        }


        public async Task<ResultadoOutput> EjecutarSpConVariablesOutput(string SQLQuery, List<SqlParameter> parameters)
        {
            ResultadoOutput resultado = new ResultadoOutput();
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand(SQLQuery, con))
                {

                    // Parámetros OUTPUT
                    SqlParameter pMensaje = new SqlParameter("@mensaje_control", SqlDbType.VarChar, 400);
                    pMensaje.Direction = ParameterDirection.Output;
                    parameters.Add(pMensaje);

                    SqlParameter pError = new SqlParameter("@error", SqlDbType.Decimal, 50);
                    pError.Direction = ParameterDirection.Output;
                    parameters.Add(pError);

                    //SqlParameter pRespuesta1 = new SqlParameter("@respuesta_1", SqlDbType.VarChar, 400);
                    //pRespuesta1.Direction = ParameterDirection.Output;
                    //parameters.Add(pRespuesta1);

                    //SqlParameter pRespuesta2 = new SqlParameter("@respuesta_2", SqlDbType.VarChar, 400);
                    //pRespuesta2.Direction = ParameterDirection.Output;
                    //parameters.Add(pRespuesta2);


                    command.Parameters.AddRange(parameters.ToArray());
                    command.CommandType = CommandType.StoredProcedure;

                    await command.ExecuteNonQueryAsync();

                    resultado.Mensaje_control = pMensaje.Value.ToString();
                    int error = 0;
                    Int32.TryParse(pError.Value.ToString(), out error);
                    resultado.Error = error;

                    //resultado.Respuesta_1 = pRespuesta1.Value != null ? pRespuesta1.Value.ToString() : null;
                    //resultado.Respuesta_2 = pRespuesta2.Value != null ? pRespuesta2.Value.ToString() : null;
                    return resultado;
                }

            }

        }

        //Para ejecutar Sp de tareas especificas como limpieza, que no devuelven ningun valor o respuesta
        public async Task<Boolean> EJecutaTarea(string SQLQuery)
        {
            using (SqlConnection con = new SqlConnection(Conexion))
            {
                await con.OpenAsync();
                try
                {
                    using (SqlCommand command = new SqlCommand(SQLQuery, con))
                    {
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                }
                catch (Exception)
                {

                }
            }

            return false;
        }

    }
}
