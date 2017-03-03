using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessINNOCV
{
    public class DataAccess
    {
        
        public List<Users> getAll()
        {
            List<Users> listUsers = null;

            try
            {

                using (DatabaseINNOCVEntities context = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DatabaseINNOCVEntities>())
                {
                    if (context.Users.Count() == 0)
                    {
                        List<Users> users = new List<Users>();
                        users.Add(new Users() { Id = 1, Name = "User 1", Birthdate = new DateTime(1960, 2, 23) });
                        users.Add(new Users() { Id = 2, Name = "User 2", Birthdate = new DateTime(1965, 8, 11) });
                        users.Add(new Users() { Id = 3, Name = "User 3", Birthdate = new DateTime(1975, 5, 1) });
                        users.Add(new Users() { Id = 4, Name = "User 4", Birthdate = new DateTime(2005, 1, 15) });
                        users.Add(new Users() { Id = 5, Name = "User 5", Birthdate = new DateTime(1985, 10, 5) });

                        context.Users.AddRange(users);

                        context.SaveChanges();
                    }

                    listUsers = context.Users.ToList();

                    context.Dispose();

                }


            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (UpdateException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationResult result = ex.EntityValidationErrors.FirstOrDefault();

                if (result != null && result.ValidationErrors.Count > 0)
                {
                    string mensaje = string.Empty;

                    foreach (var ve in result.ValidationErrors)
                    {
                        mensaje += ve.ErrorMessage + Environment.NewLine;
                    }

                    throw new Exception(mensaje);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
            return listUsers;
             
        }

        public  Users get(int id)
        {
            Users user = null;

               
            try
            {
                using (DatabaseINNOCVEntities context = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DatabaseINNOCVEntities>())
                {
                    if (context.Users.Count() == 0)
                    {
                        List<Users> users = new List<Users>();
                        users.Add(new Users() { Id = 1, Name = "User 1", Birthdate = new DateTime(1960, 2, 23) });
                        users.Add(new Users() { Id = 2, Name = "User 2", Birthdate = new DateTime(1965, 8, 11) });
                        users.Add(new Users() { Id = 3, Name = "User 3", Birthdate = new DateTime(1975, 5, 1) });
                        users.Add(new Users() { Id = 4, Name = "User 4", Birthdate = new DateTime(2005, 1, 15) });
                        users.Add(new Users() { Id = 5, Name = "User 5", Birthdate = new DateTime(1985, 10, 5) });

                        context.Users.AddRange(users);

                        context.SaveChanges();
                    }

                    user = context.Users.Where(x => x.Id == id).FirstOrDefault();

                    context.Dispose();

                }

            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (UpdateException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationResult result = ex.EntityValidationErrors.FirstOrDefault();

                if (result != null && result.ValidationErrors.Count > 0)
                {
                    string mensaje = string.Empty;

                    foreach (var ve in result.ValidationErrors)
                    {
                        mensaje += ve.ErrorMessage + Environment.NewLine;
                    }

                    throw new Exception(mensaje);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


            return user;
        }

        public  int getLast()
        {
            int idMax = 0;

            try
            {
                
                using (DatabaseINNOCVEntities context = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DatabaseINNOCVEntities>())
                {
                    if (context.Users.Count() == 0)
                    {
                        List<Users> users = new List<Users>();
                        users.Add(new Users() { Id = 1, Name = "User 1", Birthdate = new DateTime(1960, 2, 23) });
                        users.Add(new Users() { Id = 2, Name = "User 2", Birthdate = new DateTime(1965, 8, 11) });
                        users.Add(new Users() { Id = 3, Name = "User 3", Birthdate = new DateTime(1975, 5, 1) });
                        users.Add(new Users() { Id = 4, Name = "User 4", Birthdate = new DateTime(2005, 1, 15) });
                        users.Add(new Users() { Id = 5, Name = "User 5", Birthdate = new DateTime(1985, 10, 5) });

                        context.Users.AddRange(users);

                        context.SaveChanges();
                    }

                    idMax = context.Users.Max(x => x.Id);

                    context.Dispose();

                }

            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (UpdateException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationResult result = ex.EntityValidationErrors.FirstOrDefault();

                if (result != null && result.ValidationErrors.Count > 0)
                {
                    string mensaje = string.Empty;

                    foreach (var ve in result.ValidationErrors)
                    {
                        mensaje += ve.ErrorMessage + Environment.NewLine;
                    }

                    throw new Exception(mensaje);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


            return idMax;
        }

        public  Result Create(Users user)
        {
            Result result = new Result();

            if (user == null) return result;

            try
            {
                using (DatabaseINNOCVEntities context = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DatabaseINNOCVEntities>())
                {
                    user = context.Users.Add(user);

                    context.SaveChanges();

                    context.Dispose();

                }

                result.Id = user.Id;
            }
            catch (OptimisticConcurrencyException ex)
            {
                result.Error = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                result.Error = ex.Message;
            }
            catch (UpdateException ex)
            {
                result.Error = ex.Message;
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationResult validationResult = ex.EntityValidationErrors.FirstOrDefault();

                if (validationResult != null && validationResult.ValidationErrors.Count > 0)
                {
                    result.Error = string.Empty;

                    foreach (var ve in validationResult.ValidationErrors)
                    {
                        result.Error += ve.ErrorMessage + Environment.NewLine;
                    }

                    throw new Exception(result.Error);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public  Result Update(Users user)
        {
            Result result = new Result();

            if (user == null) return result;

            try
            {
                using (DatabaseINNOCVEntities context = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DatabaseINNOCVEntities>())
                {
                    Users userNew = context.Users.Where(x => x.Id == user.Id).FirstOrDefault();

                    userNew.Id = user.Id;
                    userNew.Name = user.Name;
                    userNew.Birthdate = user.Birthdate;

                    context.SaveChanges();

                    context.Dispose();

                }
            }
            catch (OptimisticConcurrencyException ex)
            {
                result.Error = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                result.Error = ex.Message;
            }
            catch (UpdateException ex)
            {
                result.Error = ex.Message;
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationResult validationResult = ex.EntityValidationErrors.FirstOrDefault();

                if (validationResult != null && validationResult.ValidationErrors.Count > 0)
                {
                    result.Error = string.Empty;

                    foreach (var ve in validationResult.ValidationErrors)
                    {
                        result.Error += ve.ErrorMessage + Environment.NewLine;
                    }

                    throw new Exception(result.Error);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        public  Result Delete(Users user)
        {
            Result result = new Result();

            try
            {
                if (user == null) return result;

                using (DatabaseINNOCVEntities context = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DatabaseINNOCVEntities>())
                {
                    Users userNew = context.Users.Where(x => x.Id == user.Id).FirstOrDefault();

                    userNew.Id = user.Id;
                    userNew.Name = user.Name;
                    userNew.Birthdate = user.Birthdate;

                    context.Users.Remove(userNew);

                    context.SaveChanges();

                    context.Dispose();
                }
            }
            catch (OptimisticConcurrencyException ex)
            {
                result.Error = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                result.Error = ex.Message;
            }
            catch (UpdateException ex)
            {
                result.Error = ex.Message;
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationResult validationResult = ex.EntityValidationErrors.FirstOrDefault();

                if (validationResult != null && validationResult.ValidationErrors.Count > 0)
                {
                    result.Error = string.Empty;

                    foreach (var ve in validationResult.ValidationErrors)
                    {
                        result.Error += ve.ErrorMessage + Environment.NewLine;
                    }

                    throw new Exception(result.Error);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

    }
}
