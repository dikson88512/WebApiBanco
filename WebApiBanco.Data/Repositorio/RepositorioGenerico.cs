using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco.Data.Repositorio
{
    public abstract class RepositorioGenerico
    {
        protected SqlParameter CrearParamString(string nombre, string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                return new SqlParameter(nombre, valor);
            }
            else
            {
                return new SqlParameter(nombre, DBNull.Value);
            }
        }
        /// <summary>
        /// Crea un parámetro de tipo entero para la base de datos. En caso de 
        /// ser nulo o cero el valor manda el tipo NULL a la base de datos
        /// </summary> 
        protected SqlParameter CrearParamGuid(string nombre, Guid? valor)
        {
            if (valor.HasValue)
            {
                return new SqlParameter(nombre, valor);
            }
            else
            {
                return new SqlParameter(nombre, DBNull.Value);
            }
        }
        /// <summary>
        /// Crea un parámetro de tipo entero para la base de datos. En caso de 
        /// ser nulo o cero el valor manda el tipo NULL a la base de datos
        /// </summary> 
        protected SqlParameter CrearParamInt(string nombre, int? valor)
        {
            if (valor.HasValue && valor.Value >= 0)
            {
                return new SqlParameter(nombre, valor);
            }
            else
            {
                return new SqlParameter(nombre, DBNull.Value);
            }
        }
        /// <summary>
        /// Crea un parámetro de tipo entero para la base de datos. En caso de 
        /// ser nulo el valor manda el tipo NULL a la base de datos. En caso de 
        /// ser cero , manda el número cero
        /// </summary> 
        protected SqlParameter CrearParamIntWithZero(string nombre, int? valor)
        {
            if (valor.HasValue && valor.Value >= 0)
            {
                return new SqlParameter(nombre, valor);
            }
            else
            {
                return new SqlParameter(nombre, DBNull.Value);
            }
        }
        protected SqlParameter CrearParamDec(string nombre, decimal? valor)
        {
            if (valor.HasValue && valor.Value > 0)
            {
                return new SqlParameter(nombre, valor);
            }
            else
            {
                return new SqlParameter(nombre, DBNull.Value);
            }
        }

        protected SqlParameter CrearParamDecWithZero(string nombre, decimal? valor)
        {
            if (valor.HasValue && valor.Value >= 0)
            {
                return new SqlParameter(nombre, valor);
            }
            else
            {
                return new SqlParameter(nombre, DBNull.Value);
            }
        }

        protected SqlParameter CrearParamDatetime(string nombre, DateTime? valor)
        {
            if (valor.HasValue && valor.Value >= SqlDateTime.MinValue && valor.Value <= SqlDateTime.MaxValue)
            {
                return new SqlParameter(nombre, valor);
            }
            else
            {
                return new SqlParameter(nombre, DBNull.Value);
            }
        }

        protected IEnumerable<SqlParameter> ObtenerParametrosSalida()
        {
            List<SqlParameter> parametrosSalida = new List<SqlParameter>();
            // Parámetros OUTPUT
            SqlParameter pMensaje = new SqlParameter("@mensaje_control", SqlDbType.VarChar, 400);
            pMensaje.Direction = ParameterDirection.Output;
            parametrosSalida.Add(pMensaje);

            SqlParameter pError = new SqlParameter("@error", SqlDbType.Decimal, 50);
            pError.Direction = ParameterDirection.Output;
            parametrosSalida.Add(pError);

            SqlParameter pRespuesta1 = new SqlParameter("@respuesta_1", SqlDbType.VarChar, 400);
            pRespuesta1.Direction = ParameterDirection.Output;
            parametrosSalida.Add(pRespuesta1);

            SqlParameter pRespuesta2 = new SqlParameter("@respuesta_2", SqlDbType.VarChar, 400);
            pRespuesta2.Direction = ParameterDirection.Output;
            parametrosSalida.Add(pRespuesta2);

            return parametrosSalida;
        }

        protected void SetParametrosSalida(List<SqlParameter> parametros)
        {
            // Parámetros OUTPUT
            SqlParameter pMensaje = new SqlParameter("@mensaje_control", SqlDbType.VarChar, 400);
            pMensaje.Direction = ParameterDirection.Output;
            parametros.Add(pMensaje);

            SqlParameter pError = new SqlParameter("@error", SqlDbType.Decimal, 50);
            pError.Direction = ParameterDirection.Output;
            parametros.Add(pError);

            SqlParameter pRespuesta_1 = new SqlParameter("@respuesta_1", SqlDbType.VarChar, 400);
            pRespuesta_1.Direction = ParameterDirection.Output;
            parametros.Add(pRespuesta_1);

            SqlParameter pRespuesta_2 = new SqlParameter("@respuesta_2", SqlDbType.VarChar, 400);
            pRespuesta_2.Direction = ParameterDirection.Output;
            parametros.Add(pRespuesta_2);
        }

    }
}
