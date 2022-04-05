// See https://aka.ms/new-console-template for more information

using EFCoreTest.Model;
using Microsoft.EntityFrameworkCore;

//InitDb();
//InitBooks();

//AsNoTrackingTest();
//AsNoTrackingWithIdentityResolutionTest();
//await GetUser(1);
//GetUserWithCompiledQuery(1);

Console.ReadLine();

#region InitUsers

static void InitDb()
{
    using ApplicationDbContext db=new ApplicationDbContext();
    var user = new User() { UserName = "Babak" };
    var user2 = new User() { UserName = "Ali" };

    db.Users.AddRange(user,user2);

    db.SaveChanges();
}

#endregion

#region First Level Cache

static async Task GetUser(int id)
{
    using ApplicationDbContext db=new ApplicationDbContext();

    var user = await db.Users.FindAsync(id);

    var user2 = await db.Users.FindAsync(id);

    var isEqual=  ReferenceEquals(user, user2);
}

#endregion

#region Init Books

static void InitBooks()
{
    using ApplicationDbContext db=new ApplicationDbContext();
    var book = new Book() { Name = "Common Book" };
    db.Books.Add(book);
   
    var users=db.Users.ToList();

    foreach (var user in users)
    {
        user.Books.Add(book);
    }

    db.SaveChanges();
}

#endregion

#region AsNoTracking Test

static void AsNoTrackingTest()
{
    using ApplicationDbContext db = new ApplicationDbContext();

    var users = db.Users.Include(u => u.Books).AsNoTracking().ToList();

    var book1 = users.Where(c => c.Id == 1).SelectMany(c => c.Books).FirstOrDefault();
    var book2 = users.Where(c => c.Id == 2).SelectMany(c => c.Books).FirstOrDefault();

    var isEqual= ReferenceEquals(book1, book2); //false
}

#endregion

#region With IdentityResolution Test

static void AsNoTrackingWithIdentityResolutionTest()
{
    using ApplicationDbContext db = new ApplicationDbContext();

    var users = db.Users.Include(u => u.Books).AsNoTrackingWithIdentityResolution().ToList();

    var book1 = users.Where(c => c.Id == 1).SelectMany(c => c.Books).FirstOrDefault();
    var book2 = users.Where(c => c.Id == 2).SelectMany(c => c.Books).FirstOrDefault();


    var isEqual = ReferenceEquals(book1, book2); //true
}

#endregion


#region Compiled Query Test

static void GetUserWithCompiledQuery(int id)
{
    using ApplicationDbContext db = new ApplicationDbContext();

    var user=CompiledQueries.GetUserWithId(db, id);
}

#endregion

#region CompiledQuery

internal static class CompiledQueries
{
    internal static Func<ApplicationDbContext, int, User> GetUserWithId =
        EF.CompileQuery((ApplicationDbContext context, int id) =>
            context.Users.Include(c => c.Books).AsNoTrackingWithIdentityResolution().FirstOrDefault(c => c.Id == id));
}

#endregion