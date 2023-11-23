using MongoDB.Driver;

public class BaseDatos {
    private string conexion ="mongodb+srv://LINGC:TEAMOTAYTAY@cluster0.p8ppjb5.mongodb.net/?retryWrites=true&w=majority"; 
    private string baseDatos ="Proyecto";
    public IMongoCollection<T>? ObtenerCollection<T>(string coleccion){
        MongoClient client = new MongoClient(this.conexion);
        IMongoCollection<T>? collection = client.GetDatabase(this.baseDatos).GetCollection<T>(coleccion);

        return collection;
    }
}