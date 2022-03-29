using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreTest.Model
{
    public class User
    {
        public User()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
