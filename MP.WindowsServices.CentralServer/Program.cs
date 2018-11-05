namespace MP.WindowsServices.CentralServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var centralServer = new CentralServerInstance();
            centralServer.Start();

            System.Console.ReadLine();
        }
    }
}
