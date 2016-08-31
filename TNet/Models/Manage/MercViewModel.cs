using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TNet.EF;

namespace TNet.Models
{
    [NotMapped]
    public class MercViewModel:Merc
    {
        public void CopyFromBase(Merc merc) {
            this.idmerc = merc.idmerc;
            this.idtype = merc.idtype;
            this.merc1 = merc.merc1;
            this.sellpt = merc.sellpt;
            this.baseprice = merc.baseprice;
            this.sellcount = merc.sellcount;
            this.stime = merc.stime;
            this.cretime = merc.cretime;
            this.entime = merc.entime;
            this.netype = merc.netype;
            this.imgs = merc.imgs;
            this.descs = merc.descs;
            this.notes = merc.notes;
            this.inuse = merc.inuse;
        }

        public void CopyToBase(Merc merc) {
            merc.idmerc = this.idmerc;
            merc.idtype = this.idtype;
            merc.merc1 = this.merc1;
            merc.sellpt = this.sellpt;
            merc.baseprice = this.baseprice;
            merc.sellcount = this.sellcount;
            merc.stime = this.stime;
            merc.cretime = this.cretime;
            merc.entime = this.entime;
            merc.netype = this.netype;
            merc.imgs = this.imgs;
            merc.descs = this.descs;
            merc.notes = this.notes;
            merc.inuse = this.inuse;
        }
    }
}