using System;
using System.Collections.Generic;
using System.Text;

namespace GarcOn.Models
{
    public class Categoria
    {
        public long ID { get; set; }

        public int Tipo { get; set; } //CategoryType

        public string Descricao { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; }
    }

    public enum CategoryType
    {
        Salgado = 1,
        Bebida = 2,
        Doce = 3
    }
}