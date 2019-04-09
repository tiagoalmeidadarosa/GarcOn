using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarcOn.Pages
{

    public class MenuPageMenuItem
    {
        public MenuPageMenuItem()
        {
            TargetType = typeof(FoodsPage);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        public string LabelVisible { get; set; }

        public Type TargetType { get; set; }
    }
}