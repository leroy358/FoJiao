using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.App_Start
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver()
        {
            this.kernel = new Ninject.StandardKernel();
            this.AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //this.kernel.Bind<IMessageProvider>()
            //    .To<NinjectMessageProvider>();
            //this.kernel.Bind<IPictureService>().To<PictureService>();
            //this.kernel.Bind<IPictureRepository>().To<PictureRepository>();
        }
    }
}