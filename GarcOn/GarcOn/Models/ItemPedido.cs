using System;
using System.Collections.Generic;
using System.Text;

namespace GarcOn.Models
{
    public class ItemPedido
    {
        public long ID { get; set; }

        public long ID_Pedido { get; set; }

        public long ID_Produto { get; set; }

        public int Quantidade { get; set; }

        public virtual ICollection<Adicional> AdicionaisSelecionados { get; set; }
    }
}
