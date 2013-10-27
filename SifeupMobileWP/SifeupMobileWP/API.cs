using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace SifeupMobileWP
{
    public class API
    {
        public delegate void handleJSON(string x);

        private static CookieContainer cContainer = new CookieContainer();

        private static bool settingsLoaded = false;
        public static IsolatedStorageSettings userSettings
        {
            get {
                if(!settingsLoaded)
                    loadSettings();

                return IsolatedStorageSettings.ApplicationSettings;
            }
        }

        private readonly static String WEBSERVICE = "https://sigarra.up.pt/feup/";
        private readonly static String EQUALS = "=";
        private readonly static String LINK_SEP = "&";
        private readonly static String WEBSERVICE_SEP = "?";


        struct WebServices {
            static public String MOBILE = "mobc_geral.";
            static public String FACILITIES_IMG = "img.";
            static public String PEOPLE_PIC = "fotografias_service.";
            static public String SUBJECT_CONTENTS = "conteudos_service.";
            static public String SUBJECT_SIGARA_LINK = "disciplinas_geral.";
            static public String NOTIFICATION_SIGARRA = "wf_geral.";
            static public String ROOM_FINDER = "salas_geral.";
            static public String ACADEMIC_PATH_SIGARRA = "alunos_ficha.";
        }

        private struct BuildingPic {
            static public String NAME = "edi_img_grande";
            static public String BUILDING = "p_edi";
            static public String FLOOR = "p_piso";
            static public String BLOCK = "p_bloco";
        }


        private struct RoomPic {
            static public String NAME = "sala_img";
            static public String BUILDING = "p_edi";
            static public String ROOM = "p_sala";
        }


        private struct RoomFinder {
            static public String NAME = "click";
            static public String BUILDING = "p_edi_cod_edi";
            static public String BLOCK = "p_edi_cod_bloco";
            static public String FLOOR = "p_piso";
            static public String X = "x";
            static public String Y = "y";
        }


        private struct Student {
            static public String NAME = "aluno";
            static public String CODE = "pv_codigo";
        }


        private struct Employee {
            static public String NAME = "ficha_func";
            static public String CODE = "pv_codigo";
        }


        private struct PersonPic {
            static public String NAME = "foto";
            static public String CODE = "pct_cod";
        }


        private struct Tuition {
            static public String NAME = "propinas";
            static public String CODE = "pv_codigo";
        }


        private struct Exams {
            static public String NAME = "exames";
            static public String CODE = "pv_codigo";
        }


        private struct Authentication {
            static public String NAME = "autentica";
            static public String CODE = "pv_login";
            static public String PASSWORD = "pv_password";
        }


        private struct Printing {
            static public String NAME = "saldo_imp";
            static public String CODE = "pv_codigo";
        }


        private struct Schedule {
            static public String NAME = "horario_aluno";
            static public String CODE = "pv_codigo";
            static public String BEGIN = "pv_semana_ini";
            static public String END = "pv_semana_fim";
        }


        private struct Subjects {
            static public String NAME = "inscricoes";
            static public String CODE = "pv_codigo";
            /** Not mandatory - if lacking assumed current. */
            static public String YEAR = "pv_a_lectivo";
        }


        private struct StudentsSearch {
            static public String NAME = "alunos_pesquisa";
            static public String QUERY = "pv_nome";
            static public String PAGE = "pv_primeiro";
        }


        private struct PrintingRef {
            static public String NAME = "gerar_propinas_mb";
            static public String VALUE = "pv_valor";
        }


        private struct AcademicPath {
            static public String NAME = "ficha_aluno";
            static public String CODE = "pv_codigo";
        }


        private struct AcademicPathSigarra {
            static public String NAME = "ficha";
            static public String CODE = "p_cod";
        }


        private struct SubjectDescription {
            static public String NAME = "ficha_uc";
            static public String CODE = "pv_cad_codigo";
            /** Not mandatory - if lacking assumed current. */
            static public String YEAR = "pv_ano_lectivo";
            /** Not mandatory - if lacking assumed current. */
            static public String PERIOD = "pv_periodo";
        }


        private struct SubjectContent {
            static public String NAME = "conteudos_uc";
            static public String CODE = "pv_uc_codigo";
            /** Not mandatory - if lacking assumed current. */
            static public String YEAR = "pv_ano_lectivo";
            /** Not mandatory - if lacking assumed current. */
            static public String PERIOD = "pv_periodo";
        }


        private struct SubjectSigarraContent {
            static public String NAME = "formview";
            static public String CODE = "p_cad_codigo";
                /** Not mandatory - if lacking assumed current. */
            static public String YEAR = "p_ano_lectivo";
                /** Not mandatory - if lacking assumed current. */
            static public String PERIOD = "p_periodo";
        }


        private struct SubjectFilesContent {
            static public String NAME = "conteudos_cont";
            static public String ID = "pct_id";
        }


        private struct UcSchedule {
            static public String NAME = "horario_uc";
            static public String CODE = "pv_uc_codigo";
        }


        private struct TeacherSchedule {
            static public String NAME = "horario_docente";
        }


        private struct RoomSchedule {
            static public String NAME = "horario_sala";
            static public String BUILDING_CODE = "pv_cod_edi";
            static public String ROOM_CODE = "pv_cod_sala";
        }


        private struct Park {
            static public String NAME = "ocupacao_parque";
            static public String CODE = "pv_parque";
        }


        private struct Notifications {
            static public String NAME = "notificacoes";
        }


        private struct NotificationsSigarra {
            static public String NAME = "not_form_view";
            static public String CODE = "pv_not_id";
        }


        private struct Canteens {
           static public String NAME = "cantinas";
        }


        private struct SetPassword {
            static public String NAME = "mudar_password";
            static public String LOGIN = "pv_login";
            static public String ACTUAL_PASSWORD = "pv_password_actual";
            static public String NEW_PASSWORD = "pv_password_nova";
            static public String CONFIRM_NEW_PASSWORD_ = "pv_password_nova_conf";
            static public String SYSTEM = "pv_sistema";
        }
        
        /**
         * The student type returned by the authenticator
         */
        public readonly static String STUDENT_TYPE = "A";

        public static String getAuthenticationUrl(String code, String password)
        {
            return WEBSERVICE + WebServices.MOBILE + Authentication.NAME
                            + WEBSERVICE_SEP + Authentication.CODE + EQUALS + code
                            + LINK_SEP + Authentication.PASSWORD + EQUALS + password;
        }




        /**
         * Schedule Url for Web Service
         * 
         * @param code
         * @param begin
         * @param end
         * @return Schedule Url
         */
        public static String getScheduleUrl(String code, String begin, String end)
        {
            return WEBSERVICE + WebServices.MOBILE + Schedule.NAME + WEBSERVICE_SEP
                            + Schedule.CODE + EQUALS + code + LINK_SEP + Schedule.BEGIN
                            + EQUALS + begin + LINK_SEP + Schedule.END + EQUALS + end;
        }


        /**
         * Notifications Url for Web Service
         * 
         * @return Notifications Url
         */
        public static String getNotificationsUrl()
        {
            return WEBSERVICE + WebServices.MOBILE + Notifications.NAME;
        }



        /**
         * Notifications Url for Web Service
         * 
         * @return Notifications Url
         */
        public static String getNotificationsSigarraUrl(String code)
        {
            return WEBSERVICE + WebServices.NOTIFICATION_SIGARRA + NotificationsSigarra.NAME + WEBSERVICE_SEP +
            NotificationsSigarra.CODE + EQUALS + code;
        }




        /**
         * Park Occupation Url for Web Service
         * 
         * @param code
         * @return Park Occupation Url
         */
        public static String getParkOccupationUrl(String code)
        {
            return WEBSERVICE + WebServices.MOBILE + Park.NAME + WEBSERVICE_SEP
                            + Park.CODE + EQUALS + code;
        }


        /**
         * Exams Url for Web Service
         * 
         * @param code
         * @return Exams Url
         */
        public static String getExamsUrl(String code)
        {
            return WEBSERVICE + WebServices.MOBILE + Exams.NAME + WEBSERVICE_SEP
                            + Exams.CODE + EQUALS + code;
        }


        /**
         * Tuition Url for Web Service
         * 
         * @param code
         * @return Tuition Url
         */
        public static String getTuitionUrl(String code)
        {
            return WEBSERVICE + WebServices.MOBILE + Tuition.NAME + WEBSERVICE_SEP
                            + Tuition.CODE + EQUALS + code;
        }


        /**
         * Student Url for Web Service
         * 
         * @param code
         * @return Student Url
         */
        public static String getStudentUrl(String code)
        {
            return WEBSERVICE + WebServices.MOBILE + Student.NAME + WEBSERVICE_SEP
                            + Student.CODE + EQUALS + code;
        }


        /**
         * Set Password Url for Web Service
         * 
         * @param code
         * @return Student Url
         */
        public static String getSetPasswordUrl(String login, String actualPassword,
                        String newPassword, String confirmNewPassword, String system)
        {
            return WEBSERVICE + WebServices.MOBILE + SetPassword.NAME
                            + WEBSERVICE_SEP + SetPassword.LOGIN + EQUALS + login
                            + LINK_SEP + SetPassword.ACTUAL_PASSWORD + EQUALS
                            + actualPassword + LINK_SEP + SetPassword.NEW_PASSWORD + EQUALS
                            + newPassword + LINK_SEP + SetPassword.CONFIRM_NEW_PASSWORD_
                            + EQUALS + confirmNewPassword + LINK_SEP + SetPassword.SYSTEM
                            + EQUALS + system;
        }


        /**
         * Employee Url for Web Service
         * 
         * @param code
         * @return Student Url
         */
        public static String getEmployeeUrl(String code)
        {
            return WEBSERVICE + WebServices.MOBILE + Employee.NAME + WEBSERVICE_SEP
                            + Employee.CODE + EQUALS + code;
        }



        /**
         * Pic Url for Web Service
         * 
         * @param code
         * @return Student Url
         */
        public static String getPersonPicUrl(String code)
        {
            return WEBSERVICE + WebServices.PEOPLE_PIC + PersonPic.NAME + WEBSERVICE_SEP
                            + PersonPic.CODE + EQUALS + code;
        }


        /**
         * Printing Url for Web Service
         * 
         * @param code
         * @return
         */
        public static String getPrintingUrl(String code)
        {
            return WEBSERVICE + WebServices.MOBILE + Printing.NAME + WEBSERVICE_SEP
                            + Printing.CODE + EQUALS + code;
        }

        /**
         * Printing Url for Web Service
         * 
         * @param id
         * @return url
         */
        public static String getSubjectFileContents(String id)
        {
            return WEBSERVICE + WebServices.SUBJECT_CONTENTS + SubjectFilesContent.NAME + WEBSERVICE_SEP
                            + SubjectFilesContent.ID + EQUALS + id;
        }


        /**
         * Subjects Url for Web Service
         * 
         * @param code
         * @return
         */
        public static String getSubjectsUrl(String code, String year)
        {
            return WEBSERVICE
                            + WebServices.MOBILE
                            + Subjects.NAME
                            + WEBSERVICE_SEP
                            + Subjects.CODE
                            + EQUALS
                            + code
                            + (year == null ? ""
                                            : (LINK_SEP + Subjects.YEAR + EQUALS + year));
        }


        /**
         * Subject Description Url for Web Service
         * 
         * @param code
         * @param year 
         * @param per 
         * @return url
         */
        public static String getSubjectDescUrl(String code, String year, String per)
        {
            return WEBSERVICE
                            + WebServices.MOBILE
                            + SubjectDescription.NAME
                            + WEBSERVICE_SEP
                            + SubjectDescription.CODE
                            + EQUALS
                            + code
                            + (year == null ? "" : (LINK_SEP + SubjectDescription.YEAR
                                            + EQUALS + year))
                            + (per == null ? "" : (LINK_SEP + SubjectDescription.PERIOD
                                            + EQUALS + per));
        }

        /**
         * Subject Description Url for Web Service
         * 
         * @param code
         * @param year 
         * @param per 
         * @return url
         */
        public static String getSubjectSigarraUrl(String code, String year, String per)
        {
            return WEBSERVICE
                            + WebServices.SUBJECT_SIGARA_LINK
                            + SubjectSigarraContent.NAME
                            + WEBSERVICE_SEP
                            + SubjectSigarraContent.CODE
                            + EQUALS
                            + code
                            + (year == null ? "" : (LINK_SEP + SubjectSigarraContent.YEAR
                                            + EQUALS + year))
                            + (per == null ? "" : (LINK_SEP + SubjectSigarraContent.PERIOD
                                            + EQUALS + per));
        }




        /**
         * Subject Content Url for Web Service
         * 
         * @param code
         * @return
         */
        public static String getSubjectContentUrl(String code, String year,
                        String per)
        {
            return WEBSERVICE + WebServices.MOBILE + SubjectContent.NAME
                            + WEBSERVICE_SEP + SubjectContent.YEAR + EQUALS + year
                            + LINK_SEP + SubjectContent.CODE + EQUALS + code + LINK_SEP
                            + SubjectContent.PERIOD + EQUALS + per;
        }


        /**
         * Printing MB Url for Web Service
         * 
         * @param code
         * @return
         */
        public static String getPrintingRefUrl(String value)
        {
            return WEBSERVICE + WebServices.MOBILE + PrintingRef.NAME
                            + WEBSERVICE_SEP + PrintingRef.VALUE + EQUALS + value;
        }


        /**
         * Academic Path Url for Web Service
         * 
         * @param code
         * @return url
         */
        public static String getAcademicPathUrl(String code)
        {
            return WEBSERVICE + WebServices.MOBILE + AcademicPath.NAME
                            + WEBSERVICE_SEP + AcademicPath.CODE + EQUALS + code;
        }


        /**
         * Academic Path Url for Web Service
         * 
         * @param code
         * @return url
         */
        public static String getAcademicPathSigarraUrl(String code)
        {
            return WEBSERVICE + WebServices.ACADEMIC_PATH_SIGARRA + AcademicPathSigarra.NAME
                            + WEBSERVICE_SEP + AcademicPathSigarra.CODE + EQUALS + code;
        }



        /**
         * Students Search Url for Web Service
         * 
         * @param query
         * @param numPage
         * @return
         */
        public static String getStudentsSearchUrl(String query, int numPage)
        {
            return WEBSERVICE + WebServices.MOBILE + StudentsSearch.NAME
                            + WEBSERVICE_SEP + StudentsSearch.QUERY + EQUALS
                            + Uri.EscapeUriString(query)//.Replace("+", "%20")
                            + LINK_SEP + StudentsSearch.PAGE + EQUALS + numPage;
        }


        /**
         * Canteens Url for Web Service
         * 
         * @return
         */
        public static String getCanteensUrl()
        {
            return WEBSERVICE + WebServices.MOBILE + Canteens.NAME;
        }


        /**
         * Building pics Url for Web Service
         * @param building 
         * @param floor 
         * 
         * @return
         */
        public static String getBuildingPicUrl(String building, String floor)
        {
            return WEBSERVICE + WebServices.FACILITIES_IMG + BuildingPic.NAME + WEBSERVICE_SEP
                            + BuildingPic.BUILDING + EQUALS + building + LINK_SEP
                            + BuildingPic.FLOOR + EQUALS + floor;
        }


        /**
         * Room pics Url for Web Service
         * @param building 
         * @param room 
         * 
         * @return
         */
        public static String getRoomPicUrl(String building, String room)
        {
            return WEBSERVICE + WebServices.FACILITIES_IMG + RoomPic.NAME + WEBSERVICE_SEP
                            + RoomPic.BUILDING + EQUALS + building + LINK_SEP
                            + RoomPic.ROOM + EQUALS + room;
        }

        /**
         * UC Schedule Url for Web Service
         * 
         * @param code
         * @param begin
         * @param end
         * @return Schedule Url
         */
        public static String getUcScheduleUrl(String code, String begin, String end)
        {
            return WEBSERVICE + WebServices.MOBILE + UcSchedule.NAME
                            + WEBSERVICE_SEP + UcSchedule.CODE + EQUALS + code + LINK_SEP
                            + Schedule.BEGIN + EQUALS + begin + LINK_SEP + Schedule.END
                            + EQUALS + end;
        }


        /**
         * Teacher Schedule Url for Web Service
         * 
         * @param code
         * @param begin
         * @param end
         * @return Schedule Url
         */
        public static String getTeacherScheduleUrl(String code, String begin,
                        String end)
        {
            return WEBSERVICE + WebServices.MOBILE + TeacherSchedule.NAME
                            + WEBSERVICE_SEP + Schedule.CODE + EQUALS + code + LINK_SEP
                            + Schedule.BEGIN + EQUALS + begin + LINK_SEP + Schedule.END
                            + EQUALS + end;
        }


        /**
         * Room Schedule Url for Web Service
         * 
         * @param code
         * @param begin
         * @param end
         * @return Schedule Url
         */
        public static String getRoomScheduleUrl(String code, String roomCode,
                        String begin, String end)
        {
            return WEBSERVICE + WebServices.MOBILE + RoomSchedule.NAME
                            + WEBSERVICE_SEP + RoomSchedule.BUILDING_CODE + EQUALS + code
                            + LINK_SEP + RoomSchedule.ROOM_CODE + EQUALS + roomCode
                            + LINK_SEP + Schedule.BEGIN + EQUALS + begin + LINK_SEP
                            + Schedule.END + EQUALS + end;
        }

        public static string getNewsUrl()
        {
            return "http://www.fe.up.pt/si/noticias_web.rss";
        }

        public static int secondYearOfSchoolYear()
        {
            DateTime nowT = DateTime.Now;
            if (nowT.Month >= 8)
                return nowT.Year + 1;

            return nowT.Year;
        }
        
        public static void performLogin(String user, String password, handleJSON ascResult)
        {
            getReply(API.getAuthenticationUrl(user, password), ascResult);
        }

        public static void getReply(String url, handleJSON ascResult)
        {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.CookieContainer = API.cContainer;

                // start the asynchronous operation
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback),
                                              new HttpWebRequestCallBack(request, ascResult));
            }
            catch
            {
                ascResult(null);
                return;
            }
        }

        private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            String postData = "";
            HttpWebRequestCallBack requestCallback = (HttpWebRequestCallBack) asynchronousResult.AsyncState;
            HttpWebRequest request = requestCallback.request;

            try
            {
                // End the operation
                Stream postStream = request.EndGetRequestStream(asynchronousResult);

                // Convert the string into a byte array.
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Write to the request stream.
                postStream.Write(byteArray, 0, postData.Length);
                postStream.Close();

                // Start the asynchronous operation to get the response
                request.BeginGetResponse(new AsyncCallback(GetResponseCallback), requestCallback);
            }
            catch
            {
                ((HttpWebRequestCallBack)asynchronousResult.AsyncState).aCallback(null);
                return;
            }
        }
        
        private static void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequestCallBack requestCallback = (HttpWebRequestCallBack)asynchronousResult.AsyncState;
            HttpWebRequest request = requestCallback.request;
            string responseString = "";

            // End the operation
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse, Encoding.GetEncoding("ISO-8859-1"));
                responseString = streamRead.ReadToEnd();

                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();
            }
            catch {
                ((HttpWebRequestCallBack)asynchronousResult.AsyncState).aCallback(null);
                return;
            }

            ((HttpWebRequestCallBack)asynchronousResult.AsyncState).aCallback(responseString.Replace(" ''", " \"\""));
        }

        public static AuthenticateResponse auth;


        internal static void handleError(JSONObjects.ErrorResponse error)
        {
            Debug.Assert(error.erro != null, "API.handleError invocado sem antes ser verificado se continha erro != null.");
            switch (error.erro)
            {
                case "Autorização":
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("You don't have access to this information.");
                    });
                    break;
                case "Autenticação":
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Previous session expired. Please login again.");
                    });
                    break;
                default:
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(error.erro_msg);
                    });
                    break;
            }
        }

        public enum SettingLogin {
            RememberEverything,
            RememberUsername,
            RememberNothing
        }

        private static void loadSettings()
        {
            settingsLoaded = true;
            if (!API.userSettings.Contains("settingLogin"))
                userSettings.Add("settingLogin", SettingLogin.RememberEverything);

            if (!API.userSettings.Contains("settingInstantSearch"))
                userSettings.Add("settingInstantSearch", true);
        }
    }
}