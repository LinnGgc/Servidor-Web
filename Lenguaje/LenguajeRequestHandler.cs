using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;

public class LenguajeRequestHandler{
    public static IResult ListarRegistros(string idCategoria){
        var filterBuilder = new FilterDefinitionBuilder<LenguajeDbMap>();
        var filter = filterBuilder.Eq(x => x.IdCategoria, idCategoria);
        
        BaseDatos bada = new BaseDatos();
        var coleccion = bada.ObtenerCollection<LenguajeDbMap>("Lenguaje");
        var lista = coleccion.Find(filter).ToList();

        return Results.Ok(lista.Select( x => new {
            Id = x.Id.ToString(),
            IdCategoria = x.IdCategoria,
            Titulo = x.Titulo,
            Descripcion = x.Descripcion,
            EsVideo = x.EsVideo,
            Url = x.Url
        }).ToList());
    }
    public static IResult CrearRegistro(LenguajeDTO datos){
         if(string.IsNullOrWhiteSpace(datos.IdCategoria)){
            return Results.BadRequest("El Id de la categoria es requerido");
        }
        if(string.IsNullOrWhiteSpace(datos.Descripcion)){
            return Results.BadRequest("La descripcion es requerido");
        }
        if(string.IsNullOrWhiteSpace(datos.Titulo)){
            return Results.BadRequest("El titulo es requerido");
        }
        if(string.IsNullOrWhiteSpace(datos.Url)){
            return Results.BadRequest("El Url es requerido");
        }
        if(!ObjectId.TryParse(datos.IdCategoria,out ObjectId idCategoria)){
            return Results.BadRequest($"El Id de la categoria ({datos.IdCategoria}) no es válido");
        }
        BaseDatos bada = new BaseDatos();

        var filterBuilderCategorias = new FilterDefinitionBuilder<CategoriaDBMap>();
        var filterCategoria = filterBuilderCategorias.Eq(x => x.Id, idCategoria);
        var coleccionCategoria = bada.ObtenerCollection<CategoriaDBMap>("Categorias");
        var categoria = coleccionCategoria.Find(filterCategoria).FirstOrDefault();

        if(categoria == null){
            return Results.NotFound($"No existe una categoria con ID = '{datos.IdCategoria}'");
        }

        LenguajeDbMap registro = new LenguajeDbMap();
        registro.Titulo = datos.Titulo;
        registro.EsVideo = datos.EsVideo;
        registro.Descripcion = datos.Descripcion;
        registro.Url = datos.Url;
        registro.IdCategoria = datos.IdCategoria;

        var coleccionLenguaje = bada.ObtenerCollection<LenguajeDbMap>("Lenguaje");
        coleccionLenguaje!.InsertOne(registro);

        return Results.Ok(registro.Id.ToString());
    }
    public static IResult Eliminar(string id){
        if(!ObjectId.TryParse(id, out ObjectId idLenguaje)){
            return Results.BadRequest($"El Id proporcionado ({id}) no es válido");
        }

        BaseDatos bada = new BaseDatos();
        var filterBuilder = new FilterDefinitionBuilder<LenguajeDbMap>();
        var filter = filterBuilder.Eq(x => x.Id, idLenguaje);
        var coleccion = bada.ObtenerCollection<LenguajeDbMap>("Lenguaje");
        coleccion!.DeleteOne(filter);

        return Results.NoContent();
    }
    public static IResult Buscar (string texto){
        var queryExpr = new BsonRegularExpression(new Regex(texto, RegexOptions.IgnoreCase));
        var filterBuilder = new FilterDefinitionBuilder<LenguajeDbMap>();
        var filter = filterBuilder.Regex("Titulo",queryExpr);
            filterBuilder.Regex("Descripcion",queryExpr);
        
        BaseDatos bada = new BaseDatos();
        var coleccion = bada.ObtenerCollection<LenguajeDbMap>("Lenguaje");
        var lista = coleccion.Find(filter).ToList();

        return Results.Ok(lista.Select(x => new{
            Id = x.Id.ToString(),
            IdCategoria = x.IdCategoria,
            Titulo = x.Titulo,
            Descripcion = x.Descripcion,
            EsVideo = x.EsVideo,
            Url = x.Url
        }).ToList());
    }
}