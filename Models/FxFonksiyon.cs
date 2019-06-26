using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoDesign.Models
{
    public static class FxFonksiyon
    {
        public static string GetInformation(MessageFormat information)
        {
            string bilgi = null;
            switch (information)
            {
                case MessageFormat.OK:
                    bilgi = "Başarıyla tamamlanmıştır."; break;
                case MessageFormat.Err:
                    bilgi = "Bir hata oluştu."; break;
                case MessageFormat.Val:
                    bilgi = "Lütfen tüm alanları doğru formatta doldurunuz.";
                    break;
            }
            return bilgi;
        }

        public static string ImageUpload(HttpPostedFileBase img, string klasorAdi, out bool sonuc)
        {
            if (img != null)
            {
                string path = string.Format("{0}/{1}.{2}", klasorAdi, Guid.NewGuid().ToString().Replace('-', '_'), img.ContentType.Split('/')[1]);
                img.SaveAs(HttpContext.Current.Server.MapPath(string.Format("~/Content/images/{0}", path)));
                sonuc = true;
                return path;
            }
            else
            {
                sonuc = false;
                return "Resim yükleme işlemi başarısız oldu.";
            }
        }

    }
}