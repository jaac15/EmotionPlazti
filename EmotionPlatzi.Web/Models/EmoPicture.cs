using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{
    public class EmoPicture
    {
        public int Id { get; set; }
        [Display(Name="Nombre")]
        public string Nombre { get; set; }
      
        [Display(Name = "Ruta")]
        public string Path { get; set; }

        public virtual ObservableCollection<EmoFace> Faces { get; set; }
    }
}