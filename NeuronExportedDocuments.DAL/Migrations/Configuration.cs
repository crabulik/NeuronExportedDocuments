using NeuronExportedDocuments.DAL.Seeds;

namespace NeuronExportedDocuments.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NeuronExportedDocuments.DAL.DocumentContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NeuronExportedDocuments.DAL.DocumentContext";
        }

        protected override void Seed(NeuronExportedDocuments.DAL.DocumentContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var serviceMessages = SeedGenerator.ServiceMessageSeed();

            foreach (var serviceMessage in serviceMessages)
            {
                var inDbItem = context.ServiceMessages.SingleOrDefault(s => s.Key == serviceMessage.Key);
                if (inDbItem == null)
                {
                    context.ServiceMessages.Add(serviceMessage);
                }
            }
            context.SaveChanges();
        }
    }
}
