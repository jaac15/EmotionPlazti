using EmotionPlatzi.Web.Models;
using EmotionPlatzi.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoUploaderController : Controller
    {
        string serverfolderpath;
        string key;
        EmotionHelper EmoHelper;
        EmotionPlatziWebContext db =  new EmotionPlatziWebContext();
        public EmoUploaderController()
        {
            serverfolderpath = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            key = ConfigurationManager.AppSettings["Emotion_Key"];
            EmoHelper = new EmotionHelper(key);
        }
        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }


        // posts: EmoUploader
        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {
            if (file?.ContentLength > 0)
            {
                var picturename = Guid.NewGuid().ToString();
                picturename += Path.GetExtension(file.FileName.ToString());
                var route = Server.MapPath(serverfolderpath);
                route += "/" + picturename; 
                file.SaveAs(route);
                

                var EmoPict =  await EmoHelper.DetectAndExtractFacesAsync(file.InputStream);
                EmoPict.Nombre = file.FileName;
                EmoPict.Path = serverfolderpath + "/" + picturename;
                db.EmoPictures.Add(EmoPict);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "EmoPictures", new { Id = EmoPict.Id });


                   

            }
            return View();
        }
    }
}