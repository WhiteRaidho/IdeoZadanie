namespace IdeoZadanie.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using IdeoZadanie.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<IdeoZadanie.DAL.TreeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IdeoZadanie.DAL.TreeContext context)
        {
            var trees = new List<Tree>
            {
                new Tree{Name="Root", Parent=null},
                new Tree{Name="Another Root", Parent=null}
            };
            trees.ForEach(t => context.Trees.AddOrUpdate(t));
            context.SaveChanges();
            foreach (Tree t in trees)
            {
                t.Parent = t;
            }
            context.SaveChanges();
        }
    }
}
