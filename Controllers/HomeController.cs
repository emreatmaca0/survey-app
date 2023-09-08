using anket_kazan.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Runtime.ConstrainedExecution;

namespace anket_kazan.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataContext context;


        public HomeController(ILogger<HomeController> logger, DataContext _context)
        {
            _logger = logger;
            context = _context;
        }


        //**--------------------------------------SİTE TANITIM SAYFALARI-------------------------------------------------------------------------------------**


        [Route("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                return RedirectToAction("survey");
            }
            else
            {
                //Veritabanındaki tüm tabloları dolaşarak kayıtları sil ve id değerlerini sıfırla
                //foreach (var entityType in context.Model.GetEntityTypes())
                //{
                //    var tableName = entityType.GetTableName();
                //    var resetIdentitySqlanswer = $"DBCC CHECKIDENT ('Answer', RESEED, 0); DELETE FROM Answer";
                //    var resetIdentitySqlquestion = $"DBCC CHECKIDENT ('Question', RESEED, 0); DELETE FROM Question";
                //    var resetIdentitySqlsurvey = $"DBCC CHECKIDENT ('Survey', RESEED, 0); DELETE FROM Survey";

                //    context.Database.ExecuteSqlRaw(resetIdentitySqlanswer);
                //    context.Database.ExecuteSqlRaw(resetIdentitySqlquestion);
                //    context.Database.ExecuteSqlRaw(resetIdentitySqlsurvey);
                //}

                return View();
            }
        }
        [Route("about")]
        public IActionResult about()
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                return RedirectToAction("survey");
            }
            else
                return View();
        }
        [Route("contact")]
        public IActionResult contact()
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                return RedirectToAction("survey");
            }
            else
                return View();
        }

        [Route("earning_system")]
        public IActionResult earning_system()
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                return RedirectToAction("survey");
            }
            else
                return View();
        }

        //*****************************************************************************************************************************************

        //**--------------------------------------GİRİŞ, KAYIT OLMA, ŞİFREMİ UNUTTUM SAYFALARI-------------------------------------------------------------------------------------**

        [Route("login")]
        [HttpPost]
        public IActionResult login(Users model)
        {

            string email = Request.Form["email"];
            string password = Request.Form["password"];

            var user = context.Users
        .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                if (user.Email == "admin@anket")
                {
                    HttpContext.Session.SetString("info", "admin");
                    return RedirectToAction("admin");
                }
                else
                {
                    List<string> data = new List<string>() { user.Name, user.Email, user.Balance, user.Password };
                    HttpContext.Session.SetString("info", JsonConvert.SerializeObject(data));
                    HttpContext.Session.SetString("balance", user.Balance);
                    HttpContext.Session.SetString("name", user.Name);
                    HttpContext.Session.SetString("email", user.Email);
                    HttpContext.Session.SetString("id", user.Id.ToString());

                    if (!string.IsNullOrEmpty(user.Phone))
                    {
                        HttpContext.Session.SetString("phone", user.Phone);
                    }

                    HttpContext.Session.SetString("password", user.Password);
                    return RedirectToAction("survey");
                }
            }
            else
            {
                ViewBag.error = "E-posta veya şifre yanlış";
                return View();
            }




        }

        [HttpGet]
        [Route("login")]
        public IActionResult login()
        {
            HttpContext.Session.Remove("code");
            if (HttpContext.Session.GetString("info") != null)
            {
                return RedirectToAction("survey");
            }
            else
                return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult register(Users model)
        {
            string email = Request.Form["email"];
            var usercheck = context.Users.FirstOrDefault(u => u.Email == email);
            if (usercheck != null)
            {
                ViewBag.error = "Kullanıcı zaten kayıtlı.";
                return View();
            }
            else
            {



                var user = new Users
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Phone = "0000",
                    Balance = model.Balance,
                    Password = model.Password,

                };
                context.Users.Add(user);
                context.SaveChanges();
                List<string> data = new List<string>() { user.Name, user.Email, user.Phone, user.Balance, user.Password };
                HttpContext.Session.SetString("info", JsonConvert.SerializeObject(data));
                HttpContext.Session.SetString("balance", user.Balance);
                HttpContext.Session.SetString("name", user.Name);
                HttpContext.Session.SetString("email", user.Email);
                HttpContext.Session.SetString("id", user.Id.ToString());
                HttpContext.Session.SetString("phone", user.Phone);
                HttpContext.Session.SetString("password", user.Password);

                return RedirectToAction("survey");
            }
        }

        [Route("register")]
        [HttpGet]
        public IActionResult register()
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                return RedirectToAction("survey");
            }
            else
                return View();
        }

        [Route("forgot_password")]
        [HttpGet]
        public IActionResult forgot_password()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("info")))
            {
                return View();
            }
            else
            {
                return RedirectToAction("error404");
            }
        }


        [Route("forgot_password")]
        [HttpPost]
        public IActionResult forgot_password(IFormCollection form)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("info")))
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("code")))
                {
                    if (HttpContext.Session.GetString("code") == "Password")
                    {
                        var user = context.Users.FirstOrDefault(u => u.Id == Convert.ToInt32(HttpContext.Session.GetString("userid")));
                        user.Password = form["password"].ToString();
                        context.SaveChanges();
                        HttpContext.Session.SetString("code", "Over");
                        HttpContext.Session.Remove("userid");
                        return View();

                    }
                    else
                    {
                        if (HttpContext.Session.GetString("code") == form["key"].ToString())
                        {

                            HttpContext.Session.SetString("code", "Password");
                            ViewBag.error = "";
                            ViewBag.success = "Lütfen yeni şifrenizi giriniz.";
                            return View();
                        }
                        else
                        {
                            ViewBag.success = "";
                            ViewBag.error = "Doğrulama kodu yanlış girildi.";
                            return View();
                        }
                    }
                }
                else
                {


                    var user = context.Users.FirstOrDefault(u => u.Email == form["email"].ToString());
                    if (user != null)
                    {


                        HttpContext.Session.SetString("userid", user.Id.ToString());
                        Random r = new Random();
                        int random = r.Next(100000, 999999);
                        HttpContext.Session.SetString("code", random.ToString());
                        MailMessage ePosta = new MailMessage();
                        ePosta.From = new MailAddress("2019212013@cumhuriyet.edu.tr");
                        ePosta.To.Add(form["email"]);

                        ePosta.Subject = "Anket Kazan Şifre Sıfırlama";
                        ePosta.IsBodyHtml = true;
                        ePosta.Body = "\r\n<!DOCTYPE HTML PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>\r\n<!--[if gte mso 9]>\r\n<xml>\r\n  <o:OfficeDocumentSettings>\r\n    <o:AllowPNG/>\r\n    <o:PixelsPerInch>96</o:PixelsPerInch>\r\n  </o:OfficeDocumentSettings>\r\n</xml>\r\n<![endif]-->\r\n  <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n  <meta name=\"x-apple-disable-message-reformatting\">\r\n  <!--[if !mso]><!--><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><!--<![endif]-->\r\n  <title></title>\r\n  \r\n    <style type=\"text/css\">\r\n      @media only screen and (min-width: 520px) {\r\n  .u-row {\r\n    width: 500px !important;\r\n  }\r\n  .u-row .u-col {\r\n    vertical-align: top;\r\n  }\r\n\r\n  .u-row .u-col-100 {\r\n    width: 500px !important;\r\n  }\r\n\r\n}\r\n\r\n@media (max-width: 520px) {\r\n  .u-row-container {\r\n    max-width: 100% !important;\r\n    padding-left: 0px !important;\r\n    padding-right: 0px !important;\r\n  }\r\n  .u-row .u-col {\r\n    min-width: 320px !important;\r\n    max-width: 100% !important;\r\n    display: block !important;\r\n  }\r\n  .u-row {\r\n    width: 100% !important;\r\n  }\r\n  .u-col {\r\n    width: 100% !important;\r\n  }\r\n  .u-col > div {\r\n    margin: 0 auto;\r\n  }\r\n}\r\nbody {\r\n  margin: 0;\r\n  padding: 0;\r\n}\r\n\r\ntable,\r\ntr,\r\ntd {\r\n  vertical-align: top;\r\n  border-collapse: collapse;\r\n}\r\n\r\np {\r\n  margin: 0;\r\n}\r\n\r\n.ie-container table,\r\n.mso-container table {\r\n  table-layout: fixed;\r\n}\r\n\r\n* {\r\n  line-height: inherit;\r\n}\r\n\r\na[x-apple-data-detectors='true'] {\r\n  color: inherit !important;\r\n  text-decoration: none !important;\r\n}\r\n\r\ntable, td { color: #000000; } </style>\r\n  \r\n  \r\n\r\n</head>\r\n\r\n<body class=\"clean-body u_body\" style=\"margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #ffffff;color: #000000\">\r\n  <!--[if IE]><div class=\"ie-container\"><![endif]-->\r\n  <!--[if mso]><div class=\"mso-container\"><![endif]-->\r\n  <table style=\"border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #ffffff;width:100%\" cellpadding=\"0\" cellspacing=\"0\">\r\n  <tbody>\r\n  <tr style=\"vertical-align: top\">\r\n    <td style=\"word-break: break-word;border-collapse: collapse !important;vertical-align: top\">\r\n    <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td align=\"center\" style=\"background-color: #ffffff;\"><![endif]-->\r\n    \r\n\r\n<div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\">\r\n  <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 500px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\">\r\n    <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\">\r\n      <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:500px;\"><tr style=\"background-color: transparent;\"><![endif]-->\r\n      \r\n<!--[if (mso)|(IE)]><td align=\"center\" width=\"500\" style=\"width: 500px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;\" valign=\"top\"><![endif]-->\r\n<div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 500px;display: table-cell;vertical-align: top;\">\r\n  <div style=\"height: 100%;width: 100% !important;\">\r\n  <!--[if (!mso)&(!IE)]><!--><div style=\"box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;\"><!--<![endif]-->\r\n  \r\n<table style=\"font-family:helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">\r\n  <tbody>\r\n    <tr>\r\n      <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:helvetica,sans-serif;\" align=\"left\">\r\n        \r\n  <h1 style=\"margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: helvetica,sans-serif; font-size: 34px; font-weight: 700;\">Anket Kazan</h1>\r\n\r\n      </td>\r\n    </tr>\r\n  </tbody>\r\n</table>\r\n\r\n<table style=\"font-family:helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">\r\n  <tbody>\r\n    <tr>\r\n      <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:helvetica,sans-serif;\" align=\"left\">\r\n        \r\n  <div style=\"font-size: 22px; line-height: 140%; text-align: center; word-wrap: break-word;\">\r\n    <p style=\"line-height: 140%;\">Doğrulama Kodu</p>\r\n  </div>\r\n\r\n      </td>\r\n    </tr>\r\n  </tbody>\r\n</table>\r\n\r\n<table style=\"font-family:helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">\r\n  <tbody>\r\n    <tr>\r\n      <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:helvetica,sans-serif;\" align=\"left\">\r\n        \r\n  <table height=\"0px\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 1px solid #BBBBBB;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%\">\r\n    <tbody>\r\n      <tr style=\"vertical-align: top\">\r\n        <td style=\"word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%\">\r\n          <span>&#160;</span>\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table>\r\n\r\n      </td>\r\n    </tr>\r\n  </tbody>\r\n</table>\r\n\r\n<table style=\"font-family:helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">\r\n  <tbody>\r\n    <tr>\r\n      <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:helvetica,sans-serif;\" align=\"left\">\r\n        \r\n  <div style=\"font-size: 39px; font-weight: 700; line-height: 140%; text-align: center; word-wrap: break-word;\">\r\n    <p style=\"line-height: 140%;\"><span style=\"background-color: #ba372a; line-height: 19.6px; color: #ecf0f1;\">" + random + "</span></p>\r\n  </div>\r\n\r\n      </td>\r\n    </tr>\r\n  </tbody>\r\n</table>\r\n\r\n  <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]-->\r\n  </div>\r\n</div>\r\n<!--[if (mso)|(IE)]></td><![endif]-->\r\n      <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n\r\n    <!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n    </td>\r\n  </tr>\r\n  </tbody>\r\n  </table>\r\n  <!--[if mso]></div><![endif]-->\r\n  <!--[if IE]></div><![endif]-->\r\n</body>\r\n\r\n</html>\r\n";

                        SmtpClient client = new SmtpClient();
                        client.Credentials = new System.Net.NetworkCredential("2019212013@cumhuriyet.edu.tr", "YxP@2q8:a#V?E/?");
                        client.Port = 587;
                        client.Host = "smtp-mail.outlook.com";
                        client.EnableSsl = true;
                        object userState = true;
                        string msg = "";

                        try
                        {
                            client.Send(ePosta);
                            ViewBag.error = "";
                            ViewBag.success = "Doğrulama kodu e-posta adresinize gönderildi.";
                            return View();
                        }
                        catch (SmtpException ex)
                        {
                            msg = ex.Message;
                            ViewBag.error = "Bir sorun oluştu.--" + msg + "--";
                            ViewBag.success = "";
                            return View();
                        }



                    }
                    else
                    {
                        ViewBag.error = "Kullanıcı bulunamadı.";
                        return View();
                    }
                }







            }
            else
            {
                return RedirectToAction("error404");
            }
        }

        //*****************************************************************************************************************************************

        //**--------------------------------------ADMİN PANELİ SAYFALARI-------------------------------------------------------------------------------------**

        [Route("admin")]
        public IActionResult admin()
        {
            if (HttpContext.Session.GetString("info") == "admin")
            {
                return View();
            }
            else
                return RedirectToAction("error404");
        }

        [Route("admin/surveylist")]
        public IActionResult surveylist()
        {
            if (HttpContext.Session.GetString("info") == "admin")
            {
                return View(context.Survey);
            }
            else
                return RedirectToAction("error404");
        }

        [Route("admin/surveyanalysis")]
        [HttpPost]
        public IActionResult surveyanalysis(Survey survey)
        {
            if (HttpContext.Session.GetString("info") == "admin")
            {
                string id = Request.Form["Id"];
                var surve = context.Survey.FirstOrDefault(u => u.Id == Convert.ToInt32(id));
                ViewBag.Survey = surve;
                var questions = context.Question.Where(u => u.SurveyId == Convert.ToInt32(id)).ToList();
                ViewBag.Questions = questions;
                ViewBag.Answers = new List<Answer>();
                foreach (var item in questions)
                {
                    var ans = context.Answer.Where(u => u.QuestionId == item.Id).ToList();
                    if (ans != null)
                    {
                        foreach (var answ in ans)
                        {
                            ViewBag.Answers.Add(answ);
                        }

                    }
                }
                return View();
            }
            else
                return RedirectToAction("error404");

        }

        [Route("admin/EditSurvey")]
        [HttpPost]
        public IActionResult EditSurvey(Survey survey)
        {
            string id = Request.Form["Id"];
            var surve = context.Survey.FirstOrDefault(u => u.Id == Convert.ToInt32(id));
            ViewBag.Survey = surve;
            var questions = context.Question.Where(u => u.SurveyId == Convert.ToInt32(id)).ToList();
            ViewBag.Questions = questions;


            return View(surve);
        }

        [Route("admin/DeleteSurvey")]
        [HttpPost]
        public IActionResult DeleteSurvey(Survey survey)
        {
            string id = Request.Form["Id"];
            var surve = context.Survey.FirstOrDefault(u => u.Id == Convert.ToInt32(id));
            if (surve != null)
            {
                context.Survey.Remove(surve);
                context.SaveChanges();
            }
            ViewBag.Survey = surve;
            var questions = context.Question.Where(u => u.SurveyId == Convert.ToInt32(id)).ToList();
            foreach (var item in questions)
            {
                if (item != null)
                {
                    context.Question.Remove(item);
                }
            }
            context.SaveChanges();
            ViewBag.Questions = questions;


            return RedirectToAction("surveylist");
        }

        [Route("admin/EditSave")]
        [HttpPost]
        public IActionResult EditSave(Survey survey, IFormCollection form)
        {
            string id = Request.Form["Id"];
            var surve = context.Survey
        .FirstOrDefault(u => u.Id == Convert.ToInt32(id));
            surve.Survey_Name = survey.Survey_Name;
            surve.Survey_sender = survey.Survey_sender;
            surve.Survey_balance = survey.Survey_balance;
            context.SaveChanges();
            var questions = context.Question.Where(u => u.SurveyId == Convert.ToInt32(id)).ToList();
            int a = questions.Count;
            for (int i = 1; i < 11; i++)
            {
                if (i < a + 1)
                {
                    foreach (var key in form.Keys)
                    {
                        if (key.StartsWith("Question_Text" + i))
                        {
                            questions[i - 1].Question_Text = form[key];
                        }
                        if (key.StartsWith("Question" + i + "-Answer_One"))
                        {
                            questions[i - 1].Answer_One = form[key];
                        }
                        if (key.StartsWith("Question" + i + "-Answer_Two"))
                        {
                            questions[i - 1].Answer_Two = form[key];
                        }
                        if (key.StartsWith("Question" + i + "-Answer_Three"))
                        {
                            questions[i - 1].Answer_Three = form[key];
                        }
                        if (key.StartsWith("Question" + i + "-Answer_Four"))
                        {
                            questions[i - 1].Answer_Four = form[key];
                        }
                    }
                }
                if (i >= a + 1)
                {
                    foreach (var ke in form.Keys)
                    {
                        if (ke.StartsWith("Question_Text" + i))
                        {
                            var Question = new Question
                            {
                                Question_Text = form["Question_Text" + i],
                                Answer_One = form["Question" + i + "-Answer_One"],
                                Answer_Two = form["Question" + i + "-Answer_Two"],
                                Answer_Three = form["Question" + i + "-Answer_Three"],
                                Answer_Four = form["Question" + i + "-Answer_Four"],
                                SurveyId = Convert.ToInt32(id)
                            };
                            context.Question.Add(Question);
                            context.SaveChanges();
                        }
                    }
                }
            }
            context.SaveChanges();
            ViewBag.Questions = questions;


            return RedirectToAction("surveylist");
        }

        [HttpPost]
        [Route("AddSurvey")]
        public IActionResult AddSurvey(Survey survey, IFormCollection form)
        {

            context.Survey.Add(survey);
            context.SaveChanges();


            var Ans_One = form[""];
            var Ans_Two = form[""];
            var Ans_Three = form[""];
            var Ans_Four = form[""];
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("Question_Text"))
                {
                    var sorumetni = form[key];
                    for (int i = 1; i < 11; i++)
                    {
                        if (key.EndsWith(i.ToString()))
                        {
                            foreach (var ke in form.Keys)
                            {
                                if (ke.StartsWith("Question" + i))
                                {
                                    if (ke.EndsWith("Answer_One"))
                                    {
                                        Ans_One = form[ke];
                                    }

                                    if (ke.EndsWith("Answer_Two"))
                                    {
                                        Ans_Two = form[ke];
                                    }

                                    if (ke.EndsWith("Answer_Three"))
                                    {
                                        Ans_Three = form[ke];
                                    }

                                    if (ke.EndsWith("Answer_Four"))
                                    {
                                        Ans_Four = form[ke];
                                    }
                                }
                            }
                            var Question = new Question
                            {
                                Question_Text = sorumetni,
                                Answer_One = Ans_One,
                                Answer_Two = Ans_Two,
                                Answer_Three = Ans_Three,
                                Answer_Four = Ans_Four,
                                SurveyId = survey.Id,
                            };
                            context.Question.Add(Question);
                            context.SaveChanges();
                            break;
                        }
                    }
                }
            }

            return RedirectToAction("admin");
        }

        //*****************************************************************************************************************************************

        //**--------------------------------------KULLANICI SAYFALARI-------------------------------------------------------------------------------------**

        [Route("survey")]
        public IActionResult survey()
        {

            int a;
            ViewBag.Solved = new List<Survey>();
            ViewBag.Unsolved = new List<Survey>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("info")))
            {

                return RedirectToAction("error404");
            }
            else
            {
                var survey = context.Survey.ToList();
                foreach (var item in survey)
                {
                    a = 0;
                    var question = context.Question.First(u => u.SurveyId == item.Id);
                    var de = context.Answer.Where(u => u.QuestionId == question.Id).ToList();
                    if (de != null)
                    {
                        foreach (var ans in de)
                        {
                            if (ans.UserId == Convert.ToInt32(HttpContext.Session.GetString("id")))
                            {
                                ViewBag.Solved.Add(item);
                                a = a + 1;
                                break;
                            }
                        }
                        if (a == 0)
                        {
                            ViewBag.Unsolved.Add(item);
                        }
                    }

                }

                return View(context.Survey);
            }

        }



        [Route("error404")]
        public IActionResult error404()
        {
            return View();
        }

        [Route("earn")]
        [HttpPost]
        public IActionResult earn(Survey survey)
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                string id = Request.Form["Id"];
                var surve = context.Survey.FirstOrDefault(u => u.Id == Convert.ToInt32(id));
                ViewBag.Survey = surve;
                var questions = context.Question.Where(u => u.SurveyId == Convert.ToInt32(id)).ToList();
                ViewBag.Questions = questions;
                return View();
            }
            else
            {
                return RedirectToAction("error404");
            }
        }

        [Route("earnsave")]
        [HttpPost]
        public IActionResult earnsave(Survey survey, IFormCollection form)
        {
            string id = Request.Form["Id"];
            string userid = Request.Form["userid"];
            var surve = context.Survey
        .First(u => u.Id == Convert.ToInt32(id));
            ViewBag.Survey = surve;
            var questions = context.Question.Where(u => u.SurveyId == Convert.ToInt32(id)).ToList();
            ViewBag.Questions = questions;


            for (int i = 1; i < 11; i++)
            {
                if (i > questions.Count)
                {
                    break;
                }
                var Answer = new Answer
                {
                    Answer_Text = form["Question" + i + "-Answer"],
                    UserId = Convert.ToInt32(userid),
                    QuestionId = Convert.ToInt32(form["Question" + i + "Id"])
                };
                context.Answer.Add(Answer);
                context.SaveChanges();
            }




            var user = context.Users.First(u => u.Id == Convert.ToInt32(userid));
            string total = (Convert.ToInt32(user.Balance) + Convert.ToInt32(surve.Survey_balance)).ToString();
            user.Balance = total;
            context.SaveChanges();
            HttpContext.Session.SetString("balance", user.Balance);
            return RedirectToAction("survey");
        }


        [Route("settings")]
        public IActionResult settings()
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                return View();
            }
            else
                return RedirectToAction("error404");
        }

        [Route("settings/SettingsSave")]
        [HttpPost]
        public IActionResult SettingsSave(IFormCollection form)
        {
            string userid = HttpContext.Session.GetString("id");
            var user = context.Users.First(u => u.Id == Convert.ToInt32(userid));
            if (form.ContainsKey("email"))
            {
                user.Email = form["email"];
                context.SaveChanges();
                HttpContext.Session.SetString("email", form["email"]);
                HttpContext.Session.SetString("settings_message", "E-posta adresiniz başarıyla değiştirildi.");
                ViewBag.error = "E-posta adresiniz başarıyla değiştirildi.";
            }
            else if (form.ContainsKey("password"))
            {
                user.Password = form["password"];
                context.SaveChanges();
                HttpContext.Session.SetString("password", form["password"]);
                ViewBag.error = "Şifreniz başarıyla değiştirildi.";
            }
            else if (form.ContainsKey("tel"))
            {
                user.Phone = form["tel"];
                context.SaveChanges();
                HttpContext.Session.SetString("phone", form["tel"]);
                ViewBag.error = "Telefon numaranız başarıyla değiştirildi.";
            }
            return RedirectToAction("settings");
        }

        [Route("withdrawal")]
        public IActionResult withdrawal()
        {
            if (HttpContext.Session.GetString("info") != null)
            {
                return View();
            }
            else
                return RedirectToAction("error404");
        }

        [Route("logout")]
        public IActionResult logout()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("info")))
            {
                return RedirectToAction("error404");
            }
            else
            {
                return View();
            }
        }

        //*******************************************************************************************





        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}