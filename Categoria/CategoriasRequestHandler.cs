using MongoDB.Driver;

public class CategoriaRequestHanlder{
    public static IResult Crear (CategoriaDTO datos){
        if (string.IsNullOrWhiteSpace(datos.Nombre)){
            return Results.BadRequest ("Es necesario un nombre");
        }
         if (string.IsNullOrWhiteSpace(datos.UrlIcono)){
            return Results.BadRequest ("Es necesario un url");
        }
    var filterBuilder= new FilterDefinitionBuilder<CategoriaDBMap>();
    var filter = filterBuilder.Eq(x=>x.Nombre,datos.Nombre);

    BaseDatos db= new BaseDatos();
    var coleccion = db.ObtenerCollection<CategoriaDBMap>("Categorias");
    CategoriaDBMap? registro = coleccion.Find(filter).FirstOrDefault();
if (registro !=null){
    return Results.BadRequest($"Ya existe la carpeta {datos.Nombre}");
}
registro = new CategoriaDBMap();
registro.Nombre= datos.Nombre;
registro.UrlIcono=datos.UrlIcono;
coleccion!.InsertOne(registro);
string nuevoId = registro.Id.ToString();
return Results.Ok(nuevoId);
    }
    public static IResult Listar(){
        var filterBuilder = new FilterDefinitionBuilder<CategoriaDBMap>();
        var filter = filterBuilder.Empty;

        BaseDatos bd= new BaseDatos();
        var coleccion=  bd.ObtenerCollection<CategoriaDBMap>("Categorias");
        List<CategoriaDBMap> mongoDBlist= coleccion.Find(filter).ToList();

        var lista = mongoDBlist.Select(x=> new{
            Id=x.Id.ToString(),
            Nombre= x.Nombre,
            UrlIcono= x.UrlIcono
        }).ToList();
        return Results.Ok (lista);
    }
}