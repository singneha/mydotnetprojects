using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using WpfZooManager.Models;

namespace WpfZooManager
{
    class MainViewModel
    {
        public List<Zoo> ZooList { get; set; }

        public List<Animal> AnimalList { get; set; } = new List<Animal>();
        public List<Animal> AssociatedAnimalList { get; set; } = new List<Animal>();
        public List<ZooAnimalMap> ZooAnimalMapList { get; set; } = new List<ZooAnimalMap>();
        public Zoo SelectedZoo { get; set; } = new Zoo();

        // We need a way to tell WPF to refresh the UI screen (explained in Step 2)

        public MainViewModel()
        {
            // 1. Instantiates the connection manager
            using (var context = new DotnettutorialContext())
            {
                // 2. Query your SQL Server table and pull it into application memory
                ZooList = context.Zoos.ToList();
                AnimalList = context.Animals.ToList();
                ZooAnimalMapList = context.ZooAnimalMap.ToList();
            }
        }
        public void UpdateAnimalsList(int zooId)
        {
            using (var context = new DotnettutorialContext())
            {
                // 2. Query your SQL Server table and pull it into application memory
                var query = from za in context.ZooAnimalMap
                            join a in context.Animals on za.AnimalId equals a.Id
                            where za.ZooId == zooId // Filter by selected Zoo
                            select a;

                AssociatedAnimalList = query.ToList();
            }
        }

        public void AddToDatabase<T>(T entity, List<T> collection) where T : class
        {
            using (var context = new DotnettutorialContext())
            {
                // 1. context.Set<T>() dynamically opens the correct DbSet table
                context.Set<T>().Attach(entity);
                context.Set<T>().Add(entity);

                // 2. Commit the insert transaction to SQL Server
                context.SaveChanges();
                collection.Add(entity);
            }
        }

        public void DeleteFromDatabase<T>(T entity, List<T> collection) where T : class
        {
            using (var context = new DotnettutorialContext())
            {

                // 2. Mark it as deleted
                context.Set<T>().Attach(entity);
                context.Set<T>().Remove(entity);

                // 3. Execute the DELETE statement in SQL Server
                context.SaveChanges();
                collection.Remove(entity);
            }
        }
        public void UpdateDatabase<T>(T entity) where T : class
        {
            using (var context = new DotnettutorialContext())
            {
                // 1. Mark it as modified
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                // 2. Execute the UPDATE statement in SQL Server
                context.SaveChanges();
            }
        }
    }
}
