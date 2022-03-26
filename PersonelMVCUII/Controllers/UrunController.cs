using PersonelMVCUII.Models.EntityFramework;
using PersonelMVCUII.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonelMVCUII.Controllers
{
   
    public class UrunController : Controller
    {
        DepoYonetimSistemiEntities db = new DepoYonetimSistemiEntities();
       
        // GET: Departman
        public ActionResult Index()
        {
            var model = db.Urun.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult Yeni()
        {

            return View("UrunForm",new Urun());
        }
        [HttpPost]
        public ActionResult Kaydet(Urun urun)
        {
            if(!ModelState.IsValid)
            {
                return View("UrunForm");
            }
            MesajViewModel model = new MesajViewModel();
            if (urun.Id == 0)
            {
                db.Urun.Add(urun);
                model.Mesaj = urun.UrunAdi + "başarıyla eklendi";
            }
               
            else
            {
                
                var guncellenecekUrun = db.Urun.Find(urun.Id);
                var rafid = db.UrunRafBilgisi.Where(x => x.UrunId == urun.Id).Select(y => y.RafId);
                if (guncellenecekUrun == null || rafid.Count()>1)
                    return HttpNotFound();
                else
                    guncellenecekUrun.UrunAdi = urun.UrunAdi;
                guncellenecekUrun.Kategori = urun.Kategori;
                
                model.Mesaj = urun.UrunAdi + "başarıyla güncellendi";
            }

            db.SaveChanges();
            model.Status = true;
            model.Linktext = "Urun Listesi";
            model.Url = "/Urun";
            //  return RedirectToAction("Index", "Departman");
            return View("_Mesaj",model);        
        }
        public ActionResult Guncelle(int id)
        {
            var model = db.Urun.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("UrunForm",model);
            
        }
        public ActionResult Sil(int id)
        {
            var silinecekUrun = db.Urun.Find(id);
            if (silinecekUrun == null)
            {
                return HttpNotFound();
            }
            db.Urun.Remove(silinecekUrun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}