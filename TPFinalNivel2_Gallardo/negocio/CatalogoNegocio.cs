using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class CatalogoNegocio
    {
		List<Articulo> lista = new List<Articulo>();
		AccesoDatos datos = new AccesoDatos();

        

        public List<Articulo> Listar()
        {
			try
			{
				datos.setearConsulta("SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion, M.Id, C.Descripcion, C.Id, ImagenUrl, Precio FROM ARTICULOS AS A INNER JOIN MARCAS AS M  ON A.IdMarca = M.Id INNER JOIN CATEGORIAS AS C ON A.IdCategoria = C.Id");
				datos.ejecutarLectura();

				while (datos.Lector.Read())
				{
					Articulo aux = new Articulo();

					aux.Id = (int)datos.Lector[0];
					aux.Codigo = (string)datos.Lector[1];
					aux.Nombre = (string)datos.Lector[2];
					aux.Descripcion = (string)datos.Lector[3];
                    aux.url = (string)datos.Lector[8];
                    
                    aux.Precio = (decimal)datos.Lector[9];
                    string precioFormateado = aux.Precio.ToString("N0");
                    aux.Precio = decimal.Parse(precioFormateado);


                    aux.TipoMarca = new Marca();
					aux.TipoMarca.Id = (int)datos.Lector[5];
					aux.TipoMarca.Descripcion = (string)datos.Lector[4];

					aux.TipoCategoria = new Categoria();
					aux.TipoCategoria.Id = (int)datos.Lector[7];
					aux.TipoCategoria.Descripcion = (string)datos.Lector[6];




					lista.Add(aux);
                }
                return lista;
            }
			catch (Exception ex)
			{

				throw ex;
			}
			finally 
			{ 
				datos.cerrarConexion();
			}
        }



        public void agregar(Articulo art)
        {
            AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdCategoria, IdMarca, ImagenUrl) VALUES (@Codigo, @Nombre, @Descripcion, @Precio, @idCategoria, @idMarca, @ImagenUrl )");
				datos.setearParametros("@Codigo", art.Codigo);
                datos.setearParametros("@Nombre", art.Nombre);
                datos.setearParametros("@Descripcion", art.Descripcion);
                datos.setearParametros("@IdMarca", art.TipoMarca.Id);
                datos.setearParametros("@IdCategoria", art.TipoCategoria.Id);
                datos.setearParametros("@ImagenUrl", art.url);
                datos.setearParametros("@Precio", art.Precio);

				datos.ejecutarAccion();
            }
			catch (Exception ex)
			{

				throw ex;
			}
			finally 
			{
				datos.cerrarConexion();
			}
        }

        public List<Marca> listarMarca()
        {
			List<Marca>tipoMarca = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();

            try
			{
				datos.setearConsulta("SELECT * FROM MARCAS");
				datos.ejecutarLectura();

				while (datos.Lector.Read())
				{
					Marca marca = new Marca();

					marca.Id = (int)datos.Lector["Id"];
					marca.Descripcion = (string)datos.Lector["Descripcion"];

					tipoMarca.Add(marca);
				}

				return tipoMarca;
			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally 
			{ 
				datos.cerrarConexion();
			}
        }

        public List<Categoria> listarCategoria()
        {
            List<Categoria> tipoCategoria = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            try
			{
				datos.setearConsulta("SELECT * FROM CATEGORIAS");
				datos.ejecutarLectura();

				while (datos.Lector.Read())
				{
					Categoria categoria = new Categoria();

					categoria.Id = (int)datos.Lector["Id"];
					categoria.Descripcion = (string)datos.Lector["Descripcion"];

					tipoCategoria.Add(categoria);
				}

				return tipoCategoria;
			}
			catch (Exception ex)
			{

				throw ex;
			}
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(Articulo seleccionado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id = @Id");
                datos.setearParametros("@id", seleccionado.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Articulo art)
        {
            AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.setearConsulta("UPDATE ARTICULOS  SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio, IdCategoria = @idCategoria, IdMarca = @idMarca, ImagenUrl = @ImagenUrl WHERE  Id = @Id");
					
                datos.setearParametros("@Codigo", art.Codigo);
                datos.setearParametros("@Nombre", art.Nombre);
                datos.setearParametros("@Descripcion", art.Descripcion);
                datos.setearParametros("@IdMarca", art.TipoMarca.Id);
                datos.setearParametros("@IdCategoria", art.TipoCategoria.Id);
                datos.setearParametros("@ImagenUrl", art.url);
                datos.setearParametros("@Precio", art.Precio);
                datos.setearParametros("@Id", art.Id);

                datos.ejecutarAccion();
            }
			catch (Exception ex)
			{

				throw ex;
			}
            finally
            {
                datos.cerrarConexion();
            }
        }

        
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion, M.Id, C.Descripcion, C.Id, ImagenUrl, Precio " +
                                 "FROM ARTICULOS AS A " +
                                 "INNER JOIN MARCAS AS M ON A.IdMarca = M.Id " +
                                 "INNER JOIN CATEGORIAS AS C ON A.IdCategoria = C.Id ";

                if (campo == "CODIGO" || campo == "PRECIO")
                {
                    switch (criterio)
                    {
                        case "MAYOR A":
                            consulta += $"WHERE {campo} > @filtro";
                            break;
                        case "MENOR A":
                            consulta += $"WHERE {campo} < @filtro";
                            break;
                        default:
                            consulta += $"WHERE {campo} = @filtro";
                            break;
                    }
                }
                else if (campo == "NOMBRE" || campo == "MARCA" || campo == "CATEGORIA")
                {
                    switch (criterio)
                    {
                        case "COMIENZA CON":
                            consulta += $"WHERE Nombre LIKE @filtro + '%'";
                            break;
                        case "TERMINA CON":
                            consulta += $"WHERE Nombre LIKE '%' + @filtro";
                            break;
                        default:
                            consulta += $"WHERE Nombre LIKE '%' + @filtro + '%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "COMIENZA CON":
                            consulta += $"WHERE P.Descripcion LIKE @filtro + '%'";
                            break;
                        case "TERMINA CON":
                            consulta += $"WHERE P.Descripcion LIKE '%' + @filtro";
                            break;
                        default:
                            consulta += $"WHERE P.Descripcion LIKE '%' + @filtro + '%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.setearParametros("@filtro", filtro);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.Lector[0];
                    aux.Codigo = (string)datos.Lector[1];
                    aux.Nombre = (string)datos.Lector[2];
                    aux.Descripcion = (string)datos.Lector[3];
                    aux.url = (string)datos.Lector[8];
                    //aux.Precio = (decimal)datos.Lector[9];
                    aux.Precio = (decimal)datos.Lector[9];
                    string precioFormateado = aux.Precio.ToString("N0");
                    aux.Precio = decimal.Parse(precioFormateado);

                    aux.TipoMarca = new Marca();
                    aux.TipoMarca.Id = (int)datos.Lector[5];
                    aux.TipoMarca.Descripcion = (string)datos.Lector[4];

                    aux.TipoCategoria = new Categoria();
                    aux.TipoCategoria.Id = (int)datos.Lector[7];
                    aux.TipoCategoria.Descripcion = (string)datos.Lector[6];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
