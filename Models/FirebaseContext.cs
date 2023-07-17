using Firebase.Database;
public class FirebaseContext
{
    private FirebaseClient _client;

    public FirebaseClient Client
    {
        get
        {
            if (_client == null)
            {
                string apiKey = "AIzaSyCn9b3wV_gXF5sLS_WCbhk01FUZQqdke0w";
                string databaseUrl = "https://tracking-system-9fc3f-default-rtdb.firebaseio.com/";

                _client = new FirebaseClient(databaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(apiKey)
                });
            }
            return _client;
        }
    }
}

