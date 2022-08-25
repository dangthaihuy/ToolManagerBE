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
//    public class NotificationHelper
//    {
//        private static readonly ILog logger = LogProvider.For<NotificationHelper>();

//        public static void SendNotifSignToUsers(string listUsers, int notifId)
//        {
//            try
//            {
//                SignalrConnection._hubName = MyCloudSettings.CommonHub;
//                HubConnection myCon = SignalrConnection.Instance.SinalrMyConnection;
//                IHubProxy myHub = SignalrConnection.Instance.SignalrMyHub;
//                if (myHub != null)
//                {
//                    //Notif to users 
//                    myHub.Invoke("SendNotifSignToUsers", listUsers, notifId).Wait();
//                }
//            }
//            catch (Exception ex)
//            {
//                var strError = string.Format("Failed to SendNotifSignToUsers because: {0}", ex.ToString());
//                logger.Error(strError);
//            }
//        }

//        //public static void AgencyAcceptApplication(IdentityApplication appInfo, int agency_id)
//        //{
//        //    var actName = "AgencyAcceptApplication";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();

//        //        var notifInfo = new IdentityNotification();

//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Application_Accepted;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.user_id = appInfo.job_seeker_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;
//        //        //notifInfo.target_id = appInfo.job_id;

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

//        //public static void Invitation_Accepted(int pic_id, int staff_id, int cv_id, int job_id)
//        //{
//        //    var actName = "Invitation_Accepted";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Invitation_Accepted;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.staff_id = pic_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Cv;
//        //        notifInfo.target_id = cv_id;
//        //        notifInfo.action_id = job_id;

//        //        var listIds = new List<int>();

//        //        listIds.Add(staff_id);
//        //        if (staff_id != pic_id)
//        //        {
//        //            listIds.Add(pic_id);
//        //        }

//        //        var notifId = _notifStore.MultiplePush(notifInfo, listIds);

//        //        if (notifId > 0)
//        //        {
//        //            SendNotifSignToUsers(string.Format(",", listIds), notifId);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void Invitation_Canceled(int pic_id, int staff_id, int job_id, int job_seeker_id)
//        //{
//        //    var actName = "Invitation_Canceled";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Invitation_Canceled;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.staff_id = pic_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Cv;
//        //        notifInfo.target_id = job_seeker_id;
//        //        notifInfo.action_id = job_id;

//        //        var listIds = new List<int>();

//        //        listIds.Add(staff_id);
//        //        if (staff_id != pic_id)
//        //        {
//        //            listIds.Add(pic_id);
//        //        }

//        //        var notifId = _notifStore.MultiplePush(notifInfo, listIds);
//        //        if (notifId > 0)
//        //        {
//        //            SendNotifSignToUsers(string.Format(",", listIds), notifId);
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
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();

//        //        var notifInfo = new IdentityNotification();

//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Application_Rejected;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.user_id = appInfo.job_seeker_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;

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
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();

//        //        var notifInfo = new IdentityNotification();

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

//        //public static void AgencyInviteJobs(IdentityInvitation info, int agency_id)
//        //{
//        //    var actName = "AgencyInviteJobs";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();

//        //        var notifInfo = new IdentityNotification();

//        //        var listTargetIds = new List<int>();
//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Invitation_Received;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        //notifInfo.user_id = appInfo.job_seeker_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;
//        //        //notifInfo.target_id = info.job_id;
//        //        notifInfo.content = info.note;

//        //        if (!string.IsNullOrEmpty(info.job_ids))
//        //        {
//        //            var results = _notifStore.MultiplePush(info.job_ids, notifInfo, info.job_seeker_id);

//        //            if (!string.IsNullOrEmpty(results))
//        //            {
//        //                var notifIds = results.Split(',');
//        //                if (notifIds.Length > 0)
//        //                {
//        //                    foreach (var item in notifIds)
//        //                    {
//        //                        SendNotifSignToUsers(info.job_seeker_id.ToString(), Utils.ConvertToInt32(item));
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void ApprovalJob(IdentityInvitation info, int agency_id)
//        //{
//        //    var actName = "AgencyApprovalJob";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();

//        //        var notifInfo = new IdentityNotification();

//        //        var listTargetIds = new List<int>();
//        //        notifInfo.action_type = (int)EnumNotifActionTypeForJobSeeker.Invitation_Received;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        //notifInfo.user_id = appInfo.job_seeker_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;
//        //        //notifInfo.target_id = info.job_id;
//        //        notifInfo.content = info.note;

//        //        if (!string.IsNullOrEmpty(info.job_ids))
//        //        {
//        //            var results = _notifStore.MultiplePush(info.job_ids, notifInfo, info.job_seeker_id);

//        //            if (!string.IsNullOrEmpty(results))
//        //            {
//        //                var notifIds = results.Split(',');
//        //                if (notifIds.Length > 0)
//        //                {
//        //                    foreach (var item in notifIds)
//        //                    {
//        //                        SendNotifSignToUsers(info.job_seeker_id.ToString(), Utils.ConvertToInt32(item));
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void Job_AcceptedPublic(int pic_id, int staff_id, int job_id)
//        //{
//        //    var actName = "Job_AcceptedPublic";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        notifInfo.action_type = (int)EnumNotifJob.Accepted;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.staff_id = pic_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;
//        //        notifInfo.target_id = job_id;
//        //        notifInfo.action_id = job_id;

//        //        var listIds = new List<int>();

//        //        listIds.Add(pic_id);

//        //        if (staff_id != pic_id)
//        //        {
//        //            listIds.Add(staff_id);
//        //        }

//        //        var notifId = _notifStore.MultiplePush(notifInfo, listIds);
//        //        if (notifId > 0)
//        //        {
//        //            SendNotifSignToUsers(string.Format(",", listIds), notifId);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void Job_CanceledPublic(int pic_id, int staff_id, int job_id)
//        //{
//        //    var actName = "Job_CanceledPublic";
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreAgencyNotification>();

//        //        var notifInfo = new IdentityAgencyNotification();

//        //        notifInfo.action_type = (int)EnumNotifJob.Canceled;
//        //        notifInfo.sender_id = 0; //Sender - system
//        //        notifInfo.staff_id = pic_id; //Receiver
//        //        notifInfo.target_type = (int)EnumNotifTargetType.Job;
//        //        notifInfo.target_id = job_id;
//        //        notifInfo.action_id = job_id;

//        //        var listIds = new List<int>();

//        //        listIds.Add(pic_id);
//        //        if (staff_id != pic_id)
//        //        {
//        //            listIds.Add(staff_id);
//        //        }

//        //        var notifId = _notifStore.MultiplePush(notifInfo, listIds);
//        //        if (notifId > 0)
//        //        {
//        //            SendNotifSignToUsers(string.Format(",", listIds), notifId);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotif - {0} because: {1}", actName, ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void CommentPostAction(int actorId, int postId, string des = "")
//        //{
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();
//        //        var postInfo = PostHelper.GetBaseInfo(postId);
//        //        var notifId = 0;
//        //        var notifInfo = new IdentityNotification();
//        //        if (postInfo != null)
//        //        {
//        //            notifInfo.ActionType = CommonAction.Comment;
//        //            notifInfo.OwnerId = postInfo.UserId; //Receiver
//        //            notifInfo.ActorId = actorId;
//        //            notifInfo.ObjectId = postId;
//        //            notifInfo.ObjectType = CommonObject.Post;
//        //            notifInfo.Description = des;

//        //            notifId = _notifStore.Insert(notifInfo);

//        //            if (notifId > 0)
//        //            {
//        //                SendNotifSignToUsers(notifInfo.OwnerId.ToString());
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotifCommentPostAction because: {0}", ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void CommentReplyPostAction(int actorId, int postId, int ownerId, string des = "")
//        //{
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();
//        //        var notifId = 0;
//        //        var notifInfo = new IdentityNotification();
//        //        notifInfo.ActionType = CommonAction.CommentReply;
//        //        notifInfo.OwnerId = ownerId; //Receiver
//        //        notifInfo.ActorId = actorId;
//        //        notifInfo.ObjectId = postId;
//        //        notifInfo.ObjectType = CommonObject.Post;
//        //        notifInfo.Description = des;

//        //        if (notifId > 0)
//        //        {
//        //            SendNotifSignToUsers(notifInfo.OwnerId.ToString());
//        //        }

//        //        notifId = _notifStore.Insert(notifInfo);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotifCommentReplyPostAction because: {0}", ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void FollowAction(int ownerId, int actorId)
//        //{
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();
//        //        var notifId = 0;
//        //        var notifInfo = new IdentityNotification();

//        //        notifInfo.ActionType = CommonAction.Follow;
//        //        notifInfo.OwnerId = ownerId; //Receiver
//        //        notifInfo.ActorId = actorId;
//        //        notifInfo.ObjectId = actorId;
//        //        notifInfo.ObjectType = CommonObject.Member;

//        //        notifId = _notifStore.Insert(notifInfo);

//        //        if (notifId > 0)
//        //        {
//        //            SendNotifSignToUsers(notifInfo.OwnerId.ToString());
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotifFollowAction because: {0}", ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void SharePostAction(int actorId, int postId)
//        //{
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();
//        //        var postInfo = PostHelper.GetBaseInfo(postId);
//        //        var notifId = 0;
//        //        var notifInfo = new IdentityNotification();
//        //        if (postInfo != null)
//        //        {
//        //            notifInfo.ActionType = CommonAction.Share;
//        //            notifInfo.OwnerId = postInfo.UserId; //Receiver
//        //            notifInfo.ActorId = actorId;
//        //            notifInfo.ObjectId = postId;
//        //            notifInfo.ObjectType = CommonObject.Post;

//        //            notifId = _notifStore.Insert(notifInfo);

//        //            if (notifId > 0)
//        //            {
//        //                SendNotifSignToUsers(notifInfo.OwnerId.ToString());
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotifRatingPostAction because: {0}", ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void LikeCommentAction(int actorId, int commentId)
//        //{
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();
//        //        var _commentStore = GlobalContainer.IocContainer.Resolve<IStoreComment>();
//        //        var commentInfo = _commentStore.GetDetail(commentId);
//        //        var notifId = 0;
//        //        var notifInfo = new IdentityNotification();
//        //        if (commentInfo != null)
//        //        {
//        //            notifInfo.ActionType = CommonAction.Like;
//        //            notifInfo.OwnerId = commentInfo.UserId; //Receiver
//        //            notifInfo.ActorId = actorId;
//        //            notifInfo.ObjectId = commentInfo.PostId;
//        //            notifInfo.ObjectType = CommonObject.Comment;
//        //            notifInfo.Description = commentInfo.Content;

//        //            notifId = _notifStore.Insert(notifInfo);

//        //            if (notifId > 0)
//        //            {
//        //                SendNotifSignToUsers(notifInfo.OwnerId.ToString());
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotifCommentPostAction because: {0}", ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}

//        //public static void LikeCommentReplyAction(int actorId, int commentReplyId)
//        //{
//        //    try
//        //    {
//        //        var _notifStore = GlobalContainer.IocContainer.Resolve<IStoreNotification>();
//        //        var _commentStore = GlobalContainer.IocContainer.Resolve<IStoreCommentReply>();
//        //        var commentInfo = _commentStore.GetDetail(commentReplyId);
//        //        var notifId = 0;
//        //        var notifInfo = new IdentityNotification();
//        //        if (commentInfo != null)
//        //        {
//        //            notifInfo.ActionType = CommonAction.Like;
//        //            notifInfo.OwnerId = commentInfo.UserId; //Receiver
//        //            notifInfo.ActorId = actorId;
//        //            notifInfo.ObjectId = commentInfo.PostId;
//        //            notifInfo.ObjectType = CommonObject.Comment;
//        //            notifInfo.Description = commentInfo.Content;

//        //            notifId = _notifStore.Insert(notifInfo);

//        //            if (notifId > 0)
//        //            {
//        //                SendNotifSignToUsers(notifInfo.OwnerId.ToString());
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        var strError = string.Format("Failed to CreateNotifCommentPostAction because: {0}", ex.ToString());
//        //        logger.Error(strError);
//        //    }
//        //}
//    }
//}