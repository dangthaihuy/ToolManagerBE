//using Manager.DataLayer.Entities;
//using Manager.DataLayer.Stores;
//using Manager.WebApp.Logging;
//using Manager.WebApp.Settings;
//using Manager.SharedLibs.Extensions;
//using Manager.WebApp.SharedLibs;
//using Autofac;
//using Microsoft.AspNet.SignalR.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Manager.WebApp.Helpers
//{
//    public class AgencyNotificationHelper
//    {
//        private static readonly ILog logger = LogProvider.For<AgencyNotificationHelper>();

//        public static void SendNotifSignToStaffs(string staffIds, int notifId)
//        {
//            try
//            {
//                SignalrConnection._hubName = MyCloudSettings.ManagerHub;
//                HubConnection myCon = SignalrConnection.Instance.SinalrMyConnection;
//                IHubProxy myHub = SignalrConnection.Instance.SignalrMyHub;
//                if (myHub != null)
//                {
//                    //Notif to users 
//                    myHub.Invoke("SendNotifSignToStaffs", staffIds, notifId).Wait();
//                }
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed to SendNotifSignToStaffs because: {0}", ex.ToString());
//                logger.Error(strError);
//            }
//        }

//        //public static void JobSeekerApplyJob(IdentityJob jobInfo, IdentityApplication appInfo)
//        //{
//        //    var actName = "JobSeekerApplyJob";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        notifInfo.action_type = (int)EnumNotifActionTypeForAgency.Application_Apply;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Cv;
//        //        notifInfo.company_id = jobInfo.company_id;
//        //        notifInfo.action_id = jobInfo.id;
//        //        if (jobInfo.company_info != null)
//        //            notifInfo.agency_id = jobInfo.company_info.agency_id;
//        //        var listIds = new List<int>();

//        //        if (jobInfo != null)
//        //        {
//        //            listIds.Add(jobInfo.pic_id);
//        //            if (jobInfo.pic_id != jobInfo.staff_id)
//        //            {
//        //                listIds.Add(jobInfo.staff_id);
//        //            }
//        //        }
//        //        var notifId = _notifStore.MultiplePush(notifInfo, listIds);

//        //        if (notifId > 0&&listIds.HasData())
//        //        {
//        //            foreach (var item in listIds)
//        //            {
//        //                SendNotifSignToStaffs(item.ToString(), notifId);
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void JobSeekerCancelJob(IdentityJob jobInfo, IdentityApplication appInfo)
//        //{
//        //    var actName = "JobSeekerCancelJob";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        notifInfo.action_type = (int)EnumNotifActionTypeForAgency.Application_Cancel;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Cv;

//        //        if (jobInfo != null)
//        //        {
//        //            notifInfo.company_id = jobInfo.company_id;

//        //            if (jobInfo.company_info != null)
//        //                notifInfo.agency_id = jobInfo.company_info.agency_id;
//        //        }

//        //        var listIds = new List<int>();

//        //        if (jobInfo != null)
//        //        {
//        //            listIds.Add(jobInfo.pic_id);
//        //            if (jobInfo.pic_id != jobInfo.staff_id)
//        //            {
//        //                listIds.Add(jobInfo.staff_id);
//        //            }
//        //        }
//        //        var notifId = _notifStore.MultiplePush(notifInfo, listIds);

//        //        if (notifId > 0 && listIds.HasData())
//        //        {
//        //            foreach (var item in listIds)
//        //            {
//        //                SendNotifSignToStaffs(item.ToString(), notifId);
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void AgencyIgnoreApplication(IdentityApplication appInfo, int agency_id)
//        //{
//        //    var actName = "AgencyIgnoreApplication";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Application_Rejected;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.user_id = appInfo.job_seeker_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;
//        //        notifInfo.target_id = appInfo.job_id;

//        //        var notifId = _notifStore.SinglePush(notifInfo);

//        //        if (notifId > 0)
//        //        {
//        //            SendNotifSignToUsers(notifInfo.user_id.ToString(), notifId);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void AgencyInviteApplication(IdentityInvitation info, int agency_id)
//        //{
//        //    var actName = "AgencyInviteApplication";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        var listIds = new List<int>();
//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Invitation_Received;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        //notifInfo.user_id = appInfo.job_seeker_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;
//        //        notifInfo.target_id = info.job_id;
//        //        notifInfo.content = info.note;

//        //        listIds = info.Invitations.Select(x => x.job_seeker_id).ToList();

//        //        if (listIds.HasData())
//        //        {
//        //            var notifId = _notifStore.MultiplePush(listIds, notifInfo);

//        //            if (notifId > 0)
//        //            {
//        //                SendNotifSignToUsers(string.Join(",", listIds), notifId);
//        //            }
//        //        }                
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//    }
//}