using MongoDB.Bson;

public class Usuarios{
    public ObjectId Id {get;set;}
    public string CorreoElectronico{get;set;}="";
    public string Nombre{get;set;}="";
    public string Contrasena{get;set;}="";
}