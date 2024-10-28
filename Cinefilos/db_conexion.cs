using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //clase para conexion a base de datos
using System.Data.SqlClient; //clase para conectarnos a SQL Server;

namespace academica {
    internal class db_conexion {
        SqlConnection miConexion = new SqlConnection();//para conectarnos a la base de datos
        SqlCommand miComando = new SqlCommand(); //para ejecutar comandos SQL en la base de datos
        SqlDataAdapter miAdaptador = new SqlDataAdapter(); //Intermediario entre la base de datos y la aplicacion
        DataSet ds = new DataSet(); //Ariquitectura de la base de datos en memoria RAM.

        public db_conexion() {
            miConexion.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db_academica.mdf;Integrated Security=True";
            miConexion.Open(); //abrimos la conexion
        }
        public DataSet obtenerDatos() {
            ds.Clear(); //limpiamos el dataset
            miComando.Connection = miConexion; //asignamos la conexion al comando para pueda ejecutar las consultas
            miComando.CommandText = "SELECT * FROM Peliculas"; //consulta SQL

            miAdaptador.SelectCommand = miComando; //asignamos el comando al adaptador
            miAdaptador.Fill(ds, "Peliculas");

            return ds;
        }
        public String administrarPeliculas(String[] peliculas) {
            String sql = "";
            if (peliculas[0] == "nuevo") {//accion nuevo
                sql = "INSERT INTO peliculas(Titulo, Director, Sinopsis, Duracion, Clasificacion, Genero) VALUES(" +
                    "'" + peliculas[2] + "'," +
                    "'" + peliculas[3] + "'," +
                    "'" + peliculas[4] + "'," +
                    "'" + peliculas[5] + "'," +
                    "'" + peliculas[6] + "'," +
                    "'" + peliculas[7] + "')" ;

            }
            else if (peliculas[0] == "modificar") {
                sql = "UPDATE peliculas SET Titulo='" + peliculas[2] + "', Director='" + peliculas[3] + "', " +
                    "Sinopsis='" + peliculas[4] + "', Duracion='" + peliculas[5] + "', Clasificacion='" + peliculas[6] + "', Genero='" + peliculas[7] + "' WHERE idPelicula=" + peliculas[1];
            } else if (peliculas[0] == "eliminar") { 
                sql = "DELETE FROM peliculas WHERE idPeliculas='" + peliculas[1] + "'";
            }
            return ejecutarSQL(sql);
        }
        private string ejecutarSQL(String sql) {
            try {
                miComando.Connection = miConexion;
                miComando.CommandText = sql;
                return miComando.ExecuteNonQuery().ToString();
            } catch (Exception ex) {
                return ex.Message;
            }
        }
    }
}
