namespace PaymentBilling.Domain.Core
{
    public static class DomainEvents
    {
        private static readonly List<Delegate> Handlers = new();

        // Registrar um handler para um evento específico
        public static void Register<T>(Action<T> handler) where T : class
        {
            Handlers.Add(handler);
        }

        // Disparar um evento para todos os handlers registrados
        public static void Raise<T>(T domainEvent) where T : class
        {
            foreach (var handler in Handlers.OfType<Action<T>>())
            {
                handler(domainEvent);
            }
        }

        // Disparar eventos de forma assíncrona
        public static async Task RaiseAsync<T>(T domainEvent) where T : class
        {
            var tasks = Handlers
                .OfType<Func<T, Task>>()
                .Select(handler => handler(domainEvent));

            await Task.WhenAll(tasks);
        }
    }
}
