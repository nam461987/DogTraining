using DogTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Service(int id)
        {
            News Record = News.SingleOrDefault("Where Id=@0 And TypeId=2 And Status=1", id);

            List<News> Records = News.Query("Where TypeId=2 And Status=1").ToList();

            ViewBag.Service = Record;
            ViewBag.Services = Records;

            return View();
        }
        public ActionResult Video(int page = 1)
        {
            List<Video> Records = DogTraining.Video.Query("Where Status=1").ToList();
            int pSize = 8; // nghia la 8 item 1 page nhe
            var totalPage = Convert.ToInt32(Math.Ceiling((double)Records.Count / 8));
            Records = Records.Skip((page - 1) * pSize).Take(pSize).ToList();

            ViewBag.Videos = Records;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPage;

            return View();
        }
        public ActionResult _GalleryPartial()
        {
            List<DogImageAndVideo_View00> Records = DogImageAndVideo_View00.
                //Query("Select Top 12 * From Dog Where status=1 Order By NEWID()").ToList();
                Query("select * from(select *, r = row_number() over(partition by Type Order By Name)from DogImageAndVideo_View00) a where r <= 4 Order By NEWID()").ToList();

            if (Records.Any())
            {
                foreach (var i in Records.Where(c => c.Type == 1))
                {
                    string avatar = i.Url == null ? "" : i.Url;

                    if (System.IO.File.Exists(Server.MapPath(avatar)))
                    {
                        avatar = CreateThumb.ResizeImage(new[] { avatar, "thumb", "800", "800", "", "4" });
                    }
                    i.Url = i.Url + "|" + avatar;
                }
            }

            ViewBag.Dogs = Records;

            return PartialView();
        }
        public ActionResult _SliderPartial()
        {
            List<Slider> Records = Slider.
                Query("Where Status=1").ToList();

            if (Records.Any())
            {
                foreach (var i in Records)
                {
                    string avatar = i.Image == null ? "" : i.Image;

                    if (System.IO.File.Exists(Server.MapPath(avatar)))
                    {
                        avatar = CreateThumb.ResizeImage(new[] { avatar, "thumb", "800", "1800", "", "4" });
                    }
                    i.Image = avatar;
                }
            }

            ViewBag.Sliders = Records;

            return PartialView();
        }
        public async System.Threading.Tasks.Task<ActionResult> _FeedbackPartial()
        {

            //var client = new Yelp.Api.Client("snexUaK4JORvcTu3g1mIh6Hey2EQ1vcD9CzgOQSm_YSmxT33JvrvnYwXOcB2jSF_zzcB2wVvJ24JYqxTh_ZsYHYiQ0T9R2H_ajjxbJkjf85OaLOkqvnx75t8YAcoW3Yx");
            //var response = client.GetReviewsAsync("vo-thomas-l-dds-seattle").Result;

            var client = new Yelp.Api.Client(Strings.YelpApiKey);
            var results = await client.GetReviewsAsync(Strings.BusinessId);

            ViewBag.ReviewResponse = results;
            return PartialView();
        }
        public JsonResult SendEmail(Email model)
        {
            var body = "<p>{3}</p><p>From: {0} ({1})</p><p>Content:</p><p>{2}</p><p>----------------------------------</p><p>We'll response as soon as we review your message.</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(model.Mail));  // replace with valid value 
            message.From = new MailAddress(Strings.EmailStr);  // replace with valid value
            message.CC.Add(new MailAddress(Strings.EmailStr));  // replace with valid value 
            message.Subject = "[ Dogtor ] Received message";
            message.Body = string.Format(body, model.Name, model.Mail, model.Message, model.Subject);
            message.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com") { Port = 587 })
                {
                    var credential = new NetworkCredential
                    {
                        UserName = Strings.EmailStr,  // replace with valid value
                        Password = Strings.EPassStr  // replace with valid value
                    };
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Send(message);

                    ModelState.Clear();
                    ViewBag.IsSuccess = true;
                }
            }
            catch
            {
                return Json(new { type = "error", text = "Error. Please try again or contact admin." });
            }
            return Json(new { type = "success", text = "Success. We received your message." });
        }
    }
}