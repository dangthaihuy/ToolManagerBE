using VnCompany.DataLayer;
using VnCompany.DataLayer.Entities;
using VnCompany.SharedLibs;
using VnCompany.SharedLibs.Exceptions;
using VnCompany.SharedLibs.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace VnCompany.Datalayer.Repositories
{
    public class RpsMigration
    {
        private readonly string _conStr;

        public RpsMigration(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsMigration()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }

        #region Common

        public bool MigrationExam(List<MigrationEntityExamModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Exam_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            var tryId = string.Empty;
                            var sentences = string.Empty;
                            if (item.tryId.HasData())
                                tryId = JsonConvert.SerializeObject(item.tryId);
                            if(item.sentences.HasData())
                                sentences = JsonConvert.SerializeObject(item.sentences);

                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@viewCount", item.viewCount },
                                { "@tryId", tryId },
                                { "@sentences", sentences },
                                { "@examNumber", item.examNumber },
                                { "@examTitle", item.examTitle },
                                { "@examLevel", item.examLevel },
                                { "@totalMark", item.totalMark },
                                { "@examDurationPart1", item.examDurationPart1 },
                                { "@examDurationPart2", item.examDurationPart2 },
                                { "@passMark", item.passMark },
                                { "@examType", item.examType },
                                { "@examSkill", item.examSkill },
                                { "@examSoundUrl", item.examSoundUrl },
                                { "@createdAt", item.createdAt },
                                { "@updatedAt", item.updatedAt },
                                { "@author", item.author }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationGrammar(List<MigrationEntityGrammarModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Grammar_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            var detail = string.Empty;
                            if (item.detail.HasData())
                            {
                                detail = JsonConvert.SerializeObject(item.detail);
                            }

                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@category", item.category },
                                { "@detail", detail },
                                { "@level", item.level },
                                { "@struct", item.@struct },
                                { "@struct_vi", item.struct_vi }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationGrammarExample(List<MigrationEntityGrammarExampleModel> list)
        {
            //Common syntax           
            var sqlCmd = @"GrammarExample_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@content", item.content },
                                { "@content_html", item.content_html },
                                { "@mean", item.mean },
                                { "@trans", item.trans }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationKanji(List<MigrationEntityKanjiModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Kanji_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@level", item.level },
                                { "@word", item.word },
                                { "@cn_mean", item.cn_mean },
                                { "@vi_mean", item.vi_mean },
                                { "@radical", item.radical },
                                { "@detail", item.detail },
                                { "@onjomi", item.onjomi },
                                { "@kunjomi", item.kunjomi },
                                { "@numstroke", item.numstroke },
                                { "@note", item.note },
                                { "@remember", item.remember },
                                { "@remember_jp", item.remember_jp },
                                { "@image", item.image }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationQuestion(List<MigrationEntityQuestionModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Question_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@questionOrder", item.questionOrder },
                                { "@mark", item.mark },
                                { "@questionDetail", item.questionDetail },
                                { "@questionSoundUrl", item.questionSoundUrl },
                                { "@sentence", item.sentence },
                                { "@soundDuration", item.soundDuration },
                                { "@time", item.time }                                
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationAnswer(List<MigrationEntityAnswerModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Answer_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@answerOrder", item.answerOrder },
                                { "@answerDetail", item.answerDetail },
                                { "@isTrue", item.isTrue },
                                { "@question", item.question }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationSentence(List<MigrationEntitySentenceModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Sentence_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@sentenceOrder", item.sentenceOrder },
                                { "@level", item.level },
                                { "@mondai", item.mondai },
                                { "@part", item.part },
                                { "@sentenceDetail", item.sentenceDetail },
                                { "@sentenceSoundUrl", item.sentenceSoundUrl },
                                { "@skill", item.skill },
                                { "@soundDuration", item.soundDuration },
                                { "@totalMark", item.totalMark },
                                { "@totalQuestion", item.totalQuestion },
                                { "@totalTime", item.totalTime },
                                { "@grammar", item.grammar }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationUser(List<MigrationEntityUserModel> list)
        {
            //Common syntax           
            var sqlCmd = @"User_Migration_Insert";
            var sqlTryExCmd = @"User_Migration_TryExam_Insert";           
            var sqlTryExRsCmd = @"User_Migration_TryExamResult_Insert";           
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@password", item.password },
                                { "@isAdmin", item.isAdmin },
                                { "@username", item.username },
                                { "@fullname", item.fullname },
                                { "@email", item.email },
                                { "@ssoId", item.ssoId },
                                { "@ssoToken", item.ssoToken },
                                { "@contactNumber", item.contactNumber },
                                { "@avatar", item.avatar },
                                //{ "@tryExams", tryExams },
                                { "@birthday", item.birthday },
                                { "@createdAt", item.createdAt },
                                { "@updatedAt", item.updatedAt }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);

                            var returnId = Utils.ConvertToInt32(returnObj);
                            if(returnId > 0)
                            {
                                if (item.tryExams.HasData())
                                {
                                    StringBuilder tryExResultCmd = new StringBuilder();
                                    foreach (var ex in item.tryExams)
                                    {
                                        //For parameters
                                        var pe = new Dictionary<string, object>
                                        {
                                            { "@user_id", item.ssoId },
                                            { "@exam", ex.exam },
                                            { "@createdAt", ex.createdAt },
                                            { "@updatedAt", ex.updatedAt }                                           
                                        };

                                        var exObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlTryExCmd, pe);
                                        var newTryExId = Utils.ConvertToInt32(exObj);

                                        if (newTryExId > 0 && ex.results.HasData())
                                        {
                                            foreach (var r in ex.results)
                                            {
                                                //var rCmd = string.Format("INSERT INTO user_try_exam_results(try_id,user_id,exam,question,answer,isTrue,mark) VALUES({0},{1},{2},{3},{4},{5},{6}); "
                                                //    , newTryExId, item.ssoId, ex.exam, r.question, r.answer, (r.isTrue) ? 1 : 0, r.mark);

                                                //tryExResultCmd.Append(rCmd);    

                                                //For parameters
                                                var pr = new Dictionary<string, object>
                                                {
                                                    { "@try_id", newTryExId },
                                                    { "@user_id", item.ssoId },
                                                    { "@exam", ex.exam },
                                                    { "@question", r.question },
                                                    { "@answer", r.answer },
                                                    { "@isTrue", r.isTrue },
                                                    { "@mark", r.mark }
                                                };

                                                MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlTryExRsCmd, pr);
                                            }
                                        }
                                    }

                                    //if (tryExResultCmd.Length > 0)
                                    //    //Insert results
                                    //    MsSqlHelper.ExecuteScalar(conn, CommandType.Text, tryExResultCmd.ToString(), null);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationWord(List<MigrationEntityWordModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Word_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@example_a", item.example_a },
                                { "@example_b", item.example_b },
                                { "@example_mean", item.example_mean },
                                { "@hira", item.hira },
                                { "@kanji", item.kanji },
                                { "@mean", item.mean },
                                { "@duration", item.duration },
                                { "@level", item.level },
                                { "@chap", item.chap },
                                { "@section", item.section },                                
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationWordChap(List<MigrationEntityWordChapModel> list)
        {
            //Common syntax           
            var sqlCmd = @"WordChap_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@title", item.title },
                                { "@description", item.description },
                                { "@level", item.level }                                
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationWordSection(List<MigrationEntityWordSectionModel> list)
        {
            //Common syntax           
            var sqlCmd = @"WordSection_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@title", item.title },
                                { "@description", item.description },
                                { "@level", item.level },
                                { "@audio_id", item.audio_id },
                                { "@chap", item.chap }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationTrack(List<MigrationEntityTrackModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Track_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@hash", item.hash },
                                { "@email", item.email },
                                { "@reference_link", item.reference_link },
                                { "@ip", item.ip },
                                { "@device_type", item.device_type },
                                { "@clicked_date", item.clicked_date },
                                { "@createdAt", item.createdAt },
                                { "@updatedAt", item.updatedAt }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationSession(List<MigrationEntitySessionModel> list)
        {
            //Common syntax           
            var sqlCmd = @"Session_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            var viewExams = string.Empty;
                            if (item.viewExams.HasData())
                            {
                                viewExams = JsonConvert.SerializeObject(item.viewExams);
                            }

                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@tryExamLeft", item.tryExamLeft },
                                { "@ipAddress", item.ipAddress },
                                { "@userAgent", item.userAgent },
                                { "@viewExams", viewExams },                             
                                { "@devicePlatform", item.devicePlatform },                             
                                { "@deviceUid", item.deviceUid }                          
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        public bool MigrationPushToken(List<MigrationEntityPushTokenModel> list)
        {
            //Common syntax           
            var sqlCmd = @"PushToken_Migration_Insert";
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    if (list.HasData())
                    {
                        foreach (var item in list)
                        {
                            //For parameters
                            var p = new Dictionary<string, object>
                            {
                                { "@_id", item._id },
                                { "@pushToken", item.pushToken },
                                { "@userId", item.userId },
                                { "@user", item.user },
                                { "@createdAt", item.createdAt },
                                { "@updatedAt", item.updatedAt }
                            };

                            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }

            return true;
        }

        //public bool UpdateTranslation(IdentityJobTranslation identity)
        //{
        //    //Common syntax           
        //    var sqlCmd = @"Job_Translation_Insert";
        //    //For parameters
        //    var parameters = new Dictionary<string, object>
        //    {
        //        {"@title", identity.title },
        //        {"@subsidy", identity.subsidy },
        //        {"@paid_holiday", identity.paid_holiday },
        //        {"@bonus", identity.bonus },
        //        {"@certificate", identity.certificate },
        //        {"@work_content", identity.work_content },
        //        {"@requirement", identity.requirement },
        //        {"@plus", identity.plus },
        //        {"@welfare", identity.welfare },
        //        {"@training", identity.training },
        //        {"@recruitment_procedure", identity.recruitment_procedure },
        //        {"@remark", identity.remark },
        //        {"@language_code", identity.language_code },
        //        {"@job_id", identity.job_id },
        //        {"@translate_status", identity.translate_status },
        //        {"@staff_id", identity.staff_id },
        //    };

        //    try
        //    {
        //        using (var conn = new SqlConnection(_conStr))
        //        {
        //            MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
        //        throw new CustomSQLException(strError);
        //    }

        //    return true;
        //}

        //public int Update(IdentityJob identity)
        //{
        //    //Common syntax           
        //    var sqlCmd = @"Job_Update";
        //    var newId = identity.id;
        //    var status_return = 0;
        //    //For parameters
        //    var parameters = new Dictionary<string, object>
        //    {
        //        {"@id", identity.id },
        //        {"@company_id", identity.company_id },
        //        {"@quantity", identity.quantity },
        //        {"@age_min", identity.age_min },
        //        {"@age_max", identity.age_max },
        //        {"@salary_min", identity.salary_min },
        //        {"@salary_max", identity.salary_max },
        //        {"@salary_type_id", identity.salary_type_id },
        //        {"@work_start_time", identity.work_start_time },
        //        {"@work_end_time", identity.work_end_time },
        //        {"@probation_duration", identity.probation_duration },
        //        {"@employment_type_id", identity.employment_type_id },
        //        {"@flexible_time", identity.flexible_time },
        //        {"@language_level", identity.language_level },
        //        {"@work_experience_doc_required", identity.work_experience_doc_required },
        //        {"@closed_time", identity.closed_time },
        //        {"@view_company", identity.view_company },
        //        {"@qualification_id", identity.qualification_id },
        //        {"@japanese_level_number", identity.japanese_level_number },
        //        {"@sub_field_id", identity.sub_field_id },
        //        {"@sub_industry_id", identity.sub_industry_id },               
        //        {"@pic_id", identity.pic_id },
        //        {"@status", identity.status },
        //        {"@japanese_only", identity.japanese_only }
        //    };

        //    StringBuilder addressInsertCmd = new StringBuilder();
        //    StringBuilder stationInsertCmd = new StringBuilder();
        //    StringBuilder translationInsertCmd = new StringBuilder();
        //    StringBuilder subFieldInsertCmd = new StringBuilder();
        //    StringBuilder tagInsertCmd = new StringBuilder();
        //    StringBuilder newTagInsertCmd = new StringBuilder();

        //    try
        //    {
        //        using (var conn = new SqlConnection(_conStr))
        //        {
        //            var returnObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, sqlCmd, parameters);
        //            status_return = Utils.ConvertToInt32(returnObj);
        //            if(status_return != 3)
        //            {
        //                return status_return;
        //            }

        //            //var jobClearCmd = @"Job_ClearBeforeUpdate";
        //            //var clearParms = new Dictionary<string, object>
        //            //{
        //            //    {"@job_id", identity.id },
        //            //    {"@language_code", identity.language_code }
        //            //};

        //            //MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, jobClearCmd, clearParms);

        //            //if (identity.Addresses.HasData())
        //            //{


        //            //    foreach (var item in identity.Addresses)
        //            //    {
        //            //        var cmdAddress = string.Format("INSERT INTO job_address(job_id,country_id,region_id,prefecture_id,city_id,detail,furigana,note,lat,lng,train_line_id) VALUES({0},{1},{2},{3},{4},N'{5}',N'{6}',N'{7}','{8}','{9}','{10}'); SELECT SCOPE_IDENTITY();"
        //            //       , newId, item.country_id, item.region_id, item.prefecture_id, item.city_id, item.detail, item.furigana, item.note, item.lat, item.lng, item.train_line_id);

        //            //        var newJobAddressIdObj = MsSqlHelper.ExecuteScalar(conn, CommandType.Text, cmdAddress, null);
        //            //        var newJobAddressId = Utils.ConvertToInt32(newJobAddressIdObj);

        //            //        if (item.Stations.HasData())
        //            //        {
        //            //            foreach (var st in item.Stations)
        //            //            {
        //            //                var cmdStation = string.Format("INSERT INTO job_address_station(job_id,station_id,job_address_id) VALUES({0},{1},{2});"
        //            //                , newId, st.id, newJobAddressId);

        //            //                stationInsertCmd.Append(cmdStation);
        //            //            }
        //            //        }

        //            //        //addressInsertCmd.Append(cmdAddress);                                
        //            //    }
        //            //}

        //            //if (identity.Job_translations.HasData())
        //            //{
        //            //    foreach (var item in identity.Job_translations)
        //            //    {
        //            //        var job_translations_Insert = @"Job_Translation_Insert";
        //            //        var translartionParms = new Dictionary<string, object>
        //            //        {
        //            //            {"@title", item.title },
        //            //            {"@subsidy", item.subsidy },
        //            //            {"@paid_holiday", item.paid_holiday },
        //            //            {"@bonus", item.bonus },
        //            //            {"@certificate", item.certificate },
        //            //            {"@work_content", item.work_content },
        //            //            {"@requirement", item.requirement },
        //            //            {"@plus", item.plus },
        //            //            {"@welfare", item.welfare },
        //            //            {"@training", item.training },
        //            //            {"@recruitment_procedure", item.recruitment_procedure },
        //            //            {"@remark", item.remark },
        //            //            {"@language_code", identity.language_code },
        //            //            {"@job_id", newId },
        //            //            {"@translate_status", -99 },
        //            //            {"@staff_id", identity.staff_id },
        //            //        };

        //            //        MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, job_translations_Insert, translartionParms);
        //            //        // var cmdTranslation = string.Format("INSERT INTO job_translation(title,subsidy,paid_holiday,bonus,certificate,work_content,requirement,plus,welfare,training,recruitment_procedure,remark,language_code,job_id) VALUES(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',{13}); "
        //            //        //, item.title, item.subsidy, item.paid_holiday, item.bonus, item.certificate, item.work_content, item.requirement, item.plus, item.welfare, item.training, item.recruitment_procedure, item.remark, item.language_code, newId);

        //            //        // translationInsertCmd.Append(cmdTranslation);
        //            //    }
        //            //}

        //            //if (identity.Sub_fields.HasData())
        //            //{
        //            //    foreach (var item in identity.Sub_fields)
        //            //    {
        //            //        var cmdSubField = string.Format("INSERT INTO job_sub_field(job_id,sub_field_id) VALUES({0},{1}); "
        //            //       , newId, item.id);

        //            //        subFieldInsertCmd.Append(cmdSubField);
        //            //    }
        //            //}

        //            //if (identity.Tags.HasData())
        //            //{
        //            //    foreach (var item in identity.Tags)
        //            //    {
        //            //        var cmdTag = string.Empty;

        //            //        if (item.id > 0)
        //            //        {
        //            //            cmdTag = string.Format("INSERT INTO job_tag(job_id, tag_id) VALUES({0},{1}); "
        //            //            , newId, item.id);
        //            //        }
        //            //        else
        //            //        {
        //            //            var tagParms = new Dictionary<string, object>
        //            //                {
        //            //                    {"@tag", item.tag}
        //            //                };

        //            //            var newTagIdObj = MsSqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "Tag_Insert", tagParms);
        //            //            var newTagId = Utils.ConvertToInt32(newTagIdObj);

        //            //            cmdTag = string.Format("INSERT INTO job_tag(job_id, tag_id) VALUES({0},{1});"
        //            //           , newId, newTagId);
        //            //        }

        //            //        tagInsertCmd.Append(cmdTag);
        //            //    }
        //            //}

        //            //Begin executing
        //            //if (addressInsertCmd.Length > 0)
        //            //    //Insert addresses
        //            //    MsSqlHelper.ExecuteScalar(conn, CommandType.Text, addressInsertCmd.ToString(), null);

        //            if (stationInsertCmd.Length > 0)
        //                //Insert job address station
        //                MsSqlHelper.ExecuteScalar(conn, CommandType.Text, stationInsertCmd.ToString(), null);

        //            //if (translationInsertCmd.Length > 0)
        //            //    //Insert translations
        //            //    MsSqlHelper.ExecuteScalar(conn, CommandType.Text, translationInsertCmd.ToString(), null);

        //            if (subFieldInsertCmd.Length > 0)
        //                //Insert sub fields
        //                MsSqlHelper.ExecuteScalar(conn, CommandType.Text, subFieldInsertCmd.ToString(), null);

        //            if (tagInsertCmd.Length > 0)
        //                //Insert tags
        //                MsSqlHelper.ExecuteScalar(conn, CommandType.Text, tagInsertCmd.ToString(), null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
        //        throw new CustomSQLException(strError);
        //    }

        //    return status_return;
        //}


        #endregion
    }
}
