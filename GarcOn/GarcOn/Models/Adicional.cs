using System;
using System.Collections.Generic;
using System.Text;

namespace GarcOn.Models
{
    public class Adicional
    {
        public long ID { get; set; }

        public string Descricao { get; set; }

        public double Valor { get; set; }

        public virtual ICollection<Produto> ProdutosAdicional { get; set; }
    }
}