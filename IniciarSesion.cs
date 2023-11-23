using MongoDB.Bson;

public class IniciarSesion{
    public ObjectId Id {get;set;}
    public string CorreoElectronico{get;set;}="";
    public string Contrasena{get;set;}="";
}
