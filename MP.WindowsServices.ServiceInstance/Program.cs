namespace MP.WindowsServices.ServiceInstance
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ServiceInstance();
            service.Start();

            System.Console.ReadLine();
        }
    }
}
