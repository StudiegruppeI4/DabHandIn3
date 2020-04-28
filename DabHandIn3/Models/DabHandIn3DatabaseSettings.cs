using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DabHandIn3.Models
{
    public class DabHandIn3DatabaseSettings : IDabHandIn3DatabaseSettings
    {
        public string PostsCollectionName { get; set; }
        public string CirclesCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDabHandIn3DatabaseSettings
    {
        string PostsCollectionName { get; set; }
        string CirclesCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}