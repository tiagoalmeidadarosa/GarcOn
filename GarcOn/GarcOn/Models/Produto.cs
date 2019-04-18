using System;
using System.Collections.Generic;
using System.Text;

namespace GarcOn.Models
{
    public class Produto
    {
        public long ID { get; set; }

        public long ID_Categoria { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public double Valor { get; set; }

        public byte[] Foto { get; set; }
    }
}