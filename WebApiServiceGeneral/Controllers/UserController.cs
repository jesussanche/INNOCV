using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DataAccessINNOCV;


namespace WebApiServiceGeneral.Controllers
{
    public class UserController : ApiController
    {
         
        // GET api/<controller>
        public IEnumerable<Users> Get()
        {
            
            try
            {
                DataAccess access = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DataAccess>();

                return access.getAll();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

         

        // GET api/<controller>/5
        public Users Get(int id)
        {
            
            try
            {
                DataAccess access = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DataAccess>();


                return access.get(id);

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        // POST api/<controller>
        public string Create([FromBody]Users value)
        {
            try
            {
               string mensaje = string.Empty;

               DataAccess access = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DataAccess>();


                int idMax = access.getLast();

                Users user = new Users() { Id = idMax + 1, Name = "User " + (idMax + 1).ToString(), Birthdate = new DateTime(1960, 2, 23) };
                 
                DataAccessINNOCV.Result result = access.Create(user);

                mensaje = result.Error;

                if (string.IsNullOrEmpty(mensaje) == false) throw new Exception(mensaje);

                return result.Id.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        // PUT api/<controller>/5
        public string Put(int id, [FromBody]Users value)
        {
            try
            {
                string mensaje = string.Empty;
                DataAccess access = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DataAccess>();

                Users user = access.get(id);
                 
                DataAccessINNOCV.Result result = access.Update(user);

                mensaje = result.Error;

                if (string.IsNullOrEmpty(mensaje) == false) throw new Exception(mensaje);

                
                return id.ToString();

            }
            catch (Exception ex)
            {
               return ex.Message;
            }
            
        }

        // DELETE api/<controller>/5
        public string Delete(int id)
        {
            try
            {
                string mensaje = string.Empty;
                DataAccess access = FactoriaIOC.Patterns.IoCFactoryBase.Resolve<DataAccess>();

                Users user = access.get(id);

                DataAccessINNOCV.Result result =  access.Delete(user);

                mensaje = result.Error;

                if (string.IsNullOrEmpty(mensaje) == false) throw new Exception(mensaje);

                return id.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}