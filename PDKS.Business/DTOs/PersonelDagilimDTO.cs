using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class PersonelDagilimDTO
    {
        public List<DepartmanDagilimDTO> DepartmanBazinda { get; set; }
        public List<UnvanDagilimDTO> UnvanBazinda { get; set; }
        public List<YasDagilimDTO> YasBazinda { get; set; }
    }
}
