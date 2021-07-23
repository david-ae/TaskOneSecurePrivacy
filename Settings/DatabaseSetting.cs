namespace Task1.Settings
{
    public class DatabaseSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"mongodb://{Host}:{Port}/?readPreference=primary&appname=MongoDB%20Compass&ssl=false";
            }
        }
    }
}