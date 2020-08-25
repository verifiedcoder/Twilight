using Autofac;
using Twilight.CQRS.Messaging.Contracts;

namespace Twilight.CQRS.Messaging.InMemory.Autofac
{
    /// <summary>
    ///     Provides an extension that uses Autofac to register in-memory messaging.
    /// </summary>
    public static class AutofacInMemoryMessagingRegistrationExtensions
    {
        /// <summary>
        ///     Adds in-memory messaging using Autofac.
        /// </summary>
        /// <param name="builder">The component registration builder.</param>
        /// <returns>ContainerBuilder.</returns>
        public static ContainerBuilder AddAutofacInMemoryMessaging(this ContainerBuilder builder)
        {
            builder.RegisterType<AutofacInMemoryMessageSender>().As<IMessageSender>();

            return builder;
        }
    }
}
