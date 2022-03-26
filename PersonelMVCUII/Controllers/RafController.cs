using PersonelMVCUII.Models.EntityFramework;
using PersonelMVCUII.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonelMVCUII.Controllers
{
    public class RafController : Controller
    {
        DepoYonetimSistemiEntities db = new DepoYonetimSistemiEntities();

        public ActionResult Index()
        {
            return View(db.Raf.ToList());
        }
        public ActionResult Yeni()
        {

            return View("RafForm", new Raf());
        }
        [HttpPost]
        public ActionResult Kaydet(Raf raf)
        {
            if (!ModelState.IsValid)
            {
                return View("RafForm");
            }
            
            MesajViewModel model = new MesajViewModel();
            if (raf.Id == 0)
            {
                db.Raf.Add(raf);
                model.Mesaj = "raf başarıyla eklendi";
            }
            else
            {
                var guncellenecekRaf = db.Raf.Find(raf.Id);
                if (guncellenecekRaf == null)
                    return HttpNotFound();
                else if(guncellenecekRaf.AnlıkKapasite<=raf.Kapasite)
                {
                    guncellenecekRaf.Kapasite = raf.Kapasite;
                    db.Entry(guncellenecekRaf).State = EntityState.Modified;
                    model.Mesaj = "raf başarıyla güncellendi";
                }
                else
                {
                    model.Mesaj = "raf eklenemez kapasite düşük";
                    return View("RafForm");
                }

            }

            db.SaveChanges();
            model.Status = true;
            model.Linktext = "Raf Listesi";
            model.Url = "/Raf";
            //  return RedirectToAction("Index", "Departman");
            return View("_Mesaj", model);
        }

        public ActionResult Guncelle(int id)
        {
            var model = db.Raf.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("RafForm", model);

        }

        public ActionResult Sil(int id)
        {
            MesajViewModel model = new MesajViewModel();
            var silinecekRaf = db.Raf.Find(id);

            if (silinecekRaf == null)
            {
                model.Mesaj = "Raf yok veya dolu";
                return View("_Mesaj",model);
            }
            var raftakiUrunler = db.UrunRafBilgisi.Where(x => x.RafId == id).ToList();


            foreach (var item in raftakiUrunler)
            {
                db.UrunRafBilgisi.Remove(item);
            }
            
            db.Raf.Remove(silinecekRaf);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}