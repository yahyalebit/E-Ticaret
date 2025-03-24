using ETicaretBusinessKatmanı.Abstract;
using ETicaretDataKatmanı.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretBusinessKatmanı.Concrete
{
	public class GenericRepository<T, TContext> : IGenericRepository<T> where T : class, new()
		where TContext : IdentityDbContext<AppUser, AppRole, int>,new()
	{
		public void Create(T entity)
		{
			using(var db = new TContext())
			{
				db.Set<T>().Add(entity);
				db.SaveChanges();
			}
		}
		public void Delete(T entity)
		{
			using (var db = new TContext())
			{
				db.Entry(entity).State = EntityState.Deleted;
				db.SaveChanges();
				//db.Set<T>().Remove(entity);
				//db.SaveChanges();
			}
		}

		public List<T> GetAll(Expression<Func<T, bool>> filter = null)
		{
			using (var db = new TContext())
			{
				if (filter == null)
				{
					return db.Set<T>().ToList();
				}
				else
				{
					return db.Set<T>().Where(filter).ToList();
				}
			}
		}

		public T GetById(int id)
		{
			using (var db = new TContext())
			{
				var nesne = db.Set<T>().Find(id);
				return nesne;
			}
		}

		public T GetById(Expression<Func<T, bool>> filter)
		{
			using (var db = new TContext())
			{
				var nesne = db.Set<T>().Find(filter);
				return nesne;
			}
		}

		public void Update(T entity)
		{
			using (var db = new TContext())
			{
				db.Entry(entity).State = EntityState.Modified;
				db.SaveChanges();
			}
		}

	}
}
