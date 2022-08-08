using Autofac;
using VnCompany.DataLayer.Stores;
using VnCompany.DataLayer.Stores.Business;
using VnCompany.WebApp.Helpers;
using VnCompany.WebApp.Services;

namespace VnCompany.WebApp.AutofacDI
{
    public class SqlModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CacheProvider>().As<ICacheProvider>();

            builder.RegisterType<StoreSetting>().As<IStoreSetting>();
            builder.RegisterType<SettingsService>().As<ISettingsService>();
            

            builder.RegisterType<StoreUser>().As<IStoreUser>();
            builder.RegisterType<StoreActivity>().As<IStoreActivity>();
            builder.RegisterType<StoreRole>().As<IStoreRole>();
            builder.RegisterType<StoreAccessRoles>().As<IStoreAccessRoles>();
            builder.RegisterType<StoreNotification>().As<IStoreNotification>();

            builder.RegisterType<StoreEmployee>().As<IStoreEmployee>();
            builder.RegisterType<StoreContact>().As<IStoreContact>();

            #region Business
            builder.RegisterType<StoreForm>().As<IStoreForm>();
            builder.RegisterType<StoreMenuService>().As<IStoreMenuService>();
            builder.RegisterType<StoreWorkFlow>().As<IStoreWorkFlow>();
            #endregion
        }
    }
}