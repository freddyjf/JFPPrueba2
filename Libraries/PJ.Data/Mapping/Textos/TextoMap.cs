using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JFP.Core.Domain.Text;

namespace JFP.Data.Mapping.Textos
{
    public class TextoMap : PJEntityTypeConfiguration<Texto>
    {
    
    
        public TextoMap()
        {
            this.ToTable("Texto");
            this.HasKey(p => p.Id);
            this.Property(p => p.NomTexto).IsRequired();
            this.Property(p => p.CodIdioma);
            this.Property(p => p.Text).IsRequired();
            this.Property(p => p.Descripcion);           
        }
    
    
    }
}
