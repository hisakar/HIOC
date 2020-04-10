using System;

namespace HIOC
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = MiniContainer.GetInstance();
            container.RegisterAsTransient<IUserService, UserManager>();
            container.RegisterAsTransient<IFoo, Foo>();
            container.RegisterAsSingleton<IFooService, FooService>();

            var fooService = container.Resolve<IFooService>();

            var fooService2 = container.Resolve<IFooService>();

            Console.WriteLine(fooService2.SayHi());
            Console.WriteLine(fooService.SayHi());

            var firstUserService = container.Resolve<IUserService>();
            var secondUserService = container.Resolve<IUserService>();
            var thirdUserService = container.Resolve<IUserService>();

            var firstUserName = firstUserService.GetUserName();
            var secondUserName = secondUserService.GetUserName();
            var thirdUserName = thirdUserService.GetUserName();

            Console.WriteLine($"{firstUserName}");
            Console.WriteLine($"{secondUserName}");
            Console.WriteLine($"{thirdUserName}");
        }
    }

    public interface IUserService
    {
        string GetUserName();
    }

    public class UserManager : IUserService
    {
        public string ProofStateForSingleton { get; set; }

        public string GetUserName()
        {
            if (string.IsNullOrEmpty(ProofStateForSingleton))
            {
                ProofStateForSingleton = GenerateRandomName();
            }

            return ProofStateForSingleton;
        }

        private string GenerateRandomName()
        {
            var randomInt = new Random().Next(0, 5);

            switch (randomInt)
            {
                case 0: return "Pilli böcük";
                case 1: return "Pilli horoz";
                case 2: return "Pilsiz solucan";
                case 3: return "Pilli eşek";
                case 4: return "Pilsiz sinek";
                case 5: return "Pilli sincap";
                default: return "Pılli ayı";
            }
        }
    }
}
