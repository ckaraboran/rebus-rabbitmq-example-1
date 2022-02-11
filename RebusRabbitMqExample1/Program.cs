using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;

namespace RebusRabbitMqExample1
{
    class Program
    {
        public const string ConnectionString = "amqp://guest:guest@localhost:5672";
        public const string QueueName = "rebus-rabbitmq-test-1";

        static void Main(string[] args)
        {
            using var activator = new BuiltinHandlerActivator();
            activator.Register(() => new PrintDateTime());
            Configure.With(activator)
                .Transport(t =>
                    t.UseRabbitMq(ConnectionString, QueueName))
                .Start();

            Console.WriteLine("Press enter to quit");
            var timer = new System.Timers.Timer();
            timer.Elapsed += delegate { activator.Bus.SendLocal(DateTime.Now).Wait(); };
            timer.Interval = 1000;
            timer.Start();
            Console.ReadLine();
        }

        public class PrintDateTime : IHandleMessages<DateTime>
        {
            public async Task Handle(DateTime currentDateTime)
            {
                Console.WriteLine("The time is {0}", currentDateTime);
            }
        }
    }
}