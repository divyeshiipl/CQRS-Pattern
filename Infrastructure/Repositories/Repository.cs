namespace Infrastructure.Repositories;

public class Repository<T> : Adf.Specification.EntityFrameworkCore.RepositoryBase<T>, IRepository<T> where T : class
{
    //public Repository(ApplicationDbContext dbContext) : base(dbContext)
    //{
    //}

    public Repository(DbContext dbContext) : base(dbContext)
    {
    }
}