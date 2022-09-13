using Autofac;
using Manager.DataLayer.Stores;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.WebApp.Helpers;
using Manager.WebApp.Services;

namespace Manager.WebApp.AutofacDI
{
    public class SqlModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CacheProvider>().As<ICacheProvider>();

            builder.RegisterType<StoreSetting>().As<IStoreSetting>();
            builder.RegisterType<SettingsService>().As<ISettingsService>();
            

            builder.RegisterType<StoreUser>().As<IStoreUser>();
            builder.RegisterType<APIStoreUser>().As<IAPIStoreUser>();
            builder.RegisterType<StoreActivity>().As<IStoreActivity>();
            builder.RegisterType<StoreRole>().As<IStoreRole>();
            builder.RegisterType<StoreAccessRoles>().As<IStoreAccessRoles>();
            builder.RegisterType<StoreNotification>().As<IStoreNotification>();

            builder.RegisterType<StoreConversation>().As<IStoreConversation>();
            builder.RegisterType<StoreMessage>().As<IStoreMessage>();
            builder.RegisterType<StoreMessageAttachment>().As<IStoreMessageAttachment>();
            builder.RegisterType<StoreGroup>().As<IStoreGroup>();
            builder.RegisterType<StoreToken>().As<IStoreToken>();

            #region Business
            #endregion
        }
    }
}