//using System;
//using System.Linq;
//using Autofac;
//using VnCompany.DataLayer.Entities;
//using VnCompany.DataLayer.Stores;
//using VnCompany.SharedLibs.Extensions;
//using VnCompany.WebApp.Models;
//using System.Collections.Generic;
//using Serilog;
//using VnCompany.WebApp.Settings;

//namespace VnCompany.WebApp.Helpers
//{
//    public class ExamHelpers
//    {
//        public static IdentityExam GetBaseInfo(int id)
//        {
//            var myKey = string.Format(EnumFormatInfoCacheKeys.ExamBase, id);
//            IdentityExam baseInfo = null;
//            try
//            {
//                //Check from cache first
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                baseInfo = cacheProvider.Get<IdentityExam>(myKey);

//                if (baseInfo == null)
//                {
//                    var myStore = Startup.IocContainer.Resolve<IStoreExam>();
//                    baseInfo = myStore.GetById(id);

//                    if (baseInfo != null)
//                    {
//                        //Storage to cache
//                        cacheProvider.Set(myKey, baseInfo, SystemSettings.DefaultCachingTimeInMinutes);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<ExamHelpers>().Error("Could not GetBaseInfo: " + ex.ToString());
//            }

//            return baseInfo;
//        }

//        public static IdentityExamDetail GetDetail(int id)
//        {
//            var myKey = string.Format(EnumFormatInfoCacheKeys.ExamDetail, id);
//            IdentityExamDetail info = null;
//            try
//            {
//                //Check from cache first
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                info = cacheProvider.Get<IdentityExamDetail>(myKey);

//                if (info == null)
//                {
//                    info = new IdentityExamDetail();

//                    info.exam = GetBaseInfo(id);
//                    if (info.exam != null)
//                    {
//                        var myStore = Startup.IocContainer.Resolve<IStoreExam>();
//                        var detail = myStore.GetDetail(id);
//                        if (detail != null)
//                        {
//                            info.sentences = detail.sentences;
//                            info.questions = detail.questions;
//                            info.answers = detail.answers;

//                            var ckMondais = ChoukaiHelpers.GetListChoukaiMondai();
//                            if (ckMondais.HasData())
//                            {
//                                var ckMd = ckMondais.Where(x => x.choukaiLevel == info.exam.examLevel).FirstOrDefault();
//                                if (ckMd != null)
//                                {
//                                    info.choukais = ckMd.choukaiList;
//                                }
//                            }

//                            //Storage to cache
//                            cacheProvider.Set(myKey, detail, SystemSettings.DefaultCachingTimeInMinutes);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<ExamHelpers>().Error("Could not GetDetail: " + ex.ToString());
//            }

//            return info;
//        }

//        public static ExamUserTryHistoryItemModel GetTryExamDetail(int try_id)
//        {
//            var myKey = string.Format(EnumFormatInfoCacheKeys.ExamTryDetail, try_id);
//            ExamUserTryHistoryItemModel info = null;
//            try
//            {
//                //Check from cache first
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                info = cacheProvider.Get<ExamUserTryHistoryItemModel>(myKey);

//                if (info == null)
//                {
//                    var myStore = Startup.IocContainer.Resolve<IStoreExam>();
//                    var detail = myStore.GetUserTryExamHistoryDetail(try_id);
//                    var ex = ExamHelpers.GetDetail(detail.exam);

//                    if (detail != null && ex != null)
//                    {
//                        info = new ExamUserTryHistoryItemModel();
//                        info.id = try_id;
//                        var hasResults = detail.results.HasData();
//                        var rightQRs = (hasResults) ? detail.results.Where(x => x.isTrue).Select(x => x.question).ToList() : new List<int>();
//                        var hasRightQrs = rightQRs.HasData();

//                        var hasSentences = ex.sentences.HasData();
//                        var hasQuestions = ex.questions.HasData();
//                        info.exam = ex.exam;
//                        if (info.exam != null)
//                        {
//                            if (hasSentences && hasQuestions)
//                            {
//                                //Part 1
//                                var sP1 = ex.sentences.Where(x => x.skill <= 3).Select(x => x.sentenceOrder).ToList();
//                                if (sP1.HasData())
//                                {
//                                    var qP1 = ex.questions.Where(x => sP1.Contains(x.sentence)).ToList();
//                                    if (qP1.HasData())
//                                    {
//                                        info.totalMarkPart1 = qP1.Sum(x => x.mark);
//                                        if (hasRightQrs)
//                                        {
//                                            info.markPart1 = qP1.Where(x => rightQRs.Contains(x.questionOrder)).Sum(x => x.mark);
//                                        }
//                                    }
//                                }

//                                //Part 2
//                                var sP2 = ex.sentences.Where(x => x.skill == 4).Select(x => x.sentenceOrder).ToList();
//                                if (sP2.HasData())
//                                {
//                                    var qP2 = ex.questions.Where(x => sP2.Contains(x.sentence)).ToList();
//                                    if (qP2.HasData())
//                                    {
//                                        info.totalMarkPart2 = qP2.Sum(x => x.mark);
//                                        if (hasRightQrs)
//                                        {
//                                            info.markPart2 = qP2.Where(x => rightQRs.Contains(x.questionOrder)).Sum(x => x.mark);
//                                        }
//                                    }
//                                }

//                                //Part 3
//                                var sP3 = ex.sentences.Where(x => x.skill == 5).Select(x => x.sentenceOrder).ToList();
//                                if (sP3.HasData())
//                                {
//                                    var qP3 = ex.questions.Where(x => sP3.Contains(x.sentence)).ToList();
//                                    if (qP3.HasData())
//                                    {
//                                        info.totalMarkPart3 = qP3.Sum(x => x.mark);
//                                        if (hasRightQrs)
//                                        {
//                                            info.markPart3 = qP3.Where(x => rightQRs.Contains(x.questionOrder)).Sum(x => x.mark);
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    info._id = detail.id.ToString();
//                    info.createdAt = detail.createdAt;
//                    info.totalMark = info.markPart1 + info.markPart2 + info.markPart3;
//                    info.totalMarkAllPart = info.totalMarkPart1 + info.totalMarkPart2 + info.totalMarkPart3;

//                    //Storage to cache
//                    cacheProvider.Set(myKey, info, SystemSettings.DefaultCachingTimeInMinutes);
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<ExamHelpers>().Error("Could not GetTryExamDetail: " + ex.ToString());
//            }

//            return info;
//        }

//        public static void ClearExamCache(int id)
//        {
//            var examBaseKey = string.Format(EnumFormatInfoCacheKeys.ExamBase, id);
//            var myKey = string.Format(EnumFormatInfoCacheKeys.ExamDetail, id);
//            try
//            {
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                cacheProvider.Clear(examBaseKey);
//                cacheProvider.Clear(myKey);
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<ExamHelpers>().Error("Could not ClearExamCache: " + ex.ToString());
//            }
//        }
        
//        public static void ClearTryHistoryCache(int id)
//        {
//            var myKey = string.Format(EnumFormatInfoCacheKeys.ExamTryDetail, id);
//            try
//            {
//                var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();
//                cacheProvider.Clear(myKey);
//            }
//            catch (Exception ex)
//            {
//                Log.ForContext<ExamHelpers>().Error("Could not ClearTryHistoryCache: " + ex.ToString());
//            }
//        }
//    }
//}