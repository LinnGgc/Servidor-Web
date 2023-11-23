using MongoDB.Driver;
using System.Net.Mail;
public static class UsuariosRequestHandler {
    public static IResult Registro(Usuarios datos) {
        if(string.IsNullOrWhiteSpace(datos.Nombre)){
            return Results.BadRequest("El nombre es requerido");
        }
         if(string.IsNullOrWhiteSpace(datos.CorreoElectronico)){
            return Results.BadRequest("El correo es requerido");
        }
        if(string.IsNullOrWhiteSpace(datos.Contrasena)){
            return Results.BadRequest("La contraseña es requerida");
        }
        BaseDatos bada = new BaseDatos();
        var coleccion = bada.ObtenerCollection<Usuarios>("Usuarios");
        if(coleccion == null){
            throw new Exception("No existe la coleccion Usuarios");

        }
        FilterDefinitionBuilder<Usuarios> filterBuilder = new FilterDefinitionBuilder<Usuarios>();
        var filter = filterBuilder.Eq(x =>x.CorreoElectronico, datos.CorreoElectronico);
         
         Usuarios? usuarioExistente = coleccion.Find(filter).FirstOrDefault();
         if(usuarioExistente != null){
            return Results.BadRequest($"Ya existe un usuario con el correo {datos.CorreoElectronico}");
         }
         coleccion.InsertOne(datos);

         return Results.Ok();
    }
   
    public static IResult Ingresar(IniciarSesion datos) {
        if(string.IsNullOrWhiteSpace(datos.CorreoElectronico)){
            return Results.BadRequest("El correo es requerido");
        }
        if(string.IsNullOrWhiteSpace(datos.Contrasena)){
            return Results.BadRequest("La contraseña es requerida");
        }
        BaseDatos bada = new BaseDatos();
        var coleccion = bada.ObtenerCollection<Usuarios>("Usuarios");
        if(coleccion == null){
            throw new Exception("No existe la coleccion Usuarios");
        }
        FilterDefinitionBuilder<Usuarios> filterBuilder = new FilterDefinitionBuilder<Usuarios>();
        var filter = filterBuilder.Eq(x =>x.CorreoElectronico, datos.CorreoElectronico);
         
        Usuarios? usuarioExistente = coleccion.Find(filter).FirstOrDefault();
         if(usuarioExistente == null){
            return Results.NotFound($"No existe un usuario con el correo proporcionado");
         }
          if(usuarioExistente.Contrasena != datos.Contrasena){
            return Results.BadRequest($"El correo o contraseña son incorrectos");
          }
        return Results.Ok("Se a ingresado a la aplicacion");
    }

    public static IResult Recuperar(Contrasena datos) {
        if(string.IsNullOrWhiteSpace(datos.CorreoElectronico)){
            return Results.BadRequest("El correo es requerido");
        }
        BaseDatos bada = new BaseDatos();
        var coleccion = bada.ObtenerCollection<Usuarios>("Usuarios");
        if(coleccion == null){
            throw new Exception("No existe la coleccion Usuarios");
        }
        FilterDefinitionBuilder<Usuarios> filterBuilder = new FilterDefinitionBuilder<Usuarios>();
        var filter = filterBuilder.Eq(x =>x.CorreoElectronico, datos.CorreoElectronico);
         
        Usuarios? usuarioExistente = coleccion.Find(filter).FirstOrDefault();
         if(usuarioExistente == null){
            return Results.NotFound($"No existe un usuario con el correo proporcionado: {datos.CorreoElectronico}");
         } else if(usuarioExistente.CorreoElectronico==datos.CorreoElectronico){
            Correos c = new Correos();
            c.Destinatario = usuarioExistente.CorreoElectronico;
            c.Asunto = "Recuperacion de la contraseña";
            c.Mensaje = "Tu contraseña es: "+usuarioExistente.Contrasena;
            c.Enviar();
         }

         return Results.Ok("Se envio un correo de recuperacion");
    }
}
