using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.Framework.Repository.Data;

namespace HelloWorld.Service
{
    public class HServices
    {
        IRepository<HelloWorld.Model.TableH> TableHRepository;
        public HServices(IRepository<HelloWorld.Model.TableH> TableHRepository)
        {
            this.TableHRepository = TableHRepository;
        }


        public IList<HelloWorld.Model.TableH> GetAllData()
        {
            return TableHRepository.Get().ToList();
        }


        public void AddAModel(Model.TableH model)
        {
            this.TableHRepository.Add(model);
        }
    }
}
