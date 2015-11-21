using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talent.DataAccess.Ado;
using Talent.Domain;

namespace Talent.Mvc.Models
{
    public class ShowViewModel
    {

        public ShowViewModel()
        {

            var ratingsRepo = new MpaaRatingRepository();
            MpaaRatings = ratingsRepo.Fetch()
                .OrderBy(o => o.DisplayOrder)
                    .ThenBy(o => o.Code)
                    .Select(o => new SelectListItem
                    {
                        Value = o.MpaaRatingId.ToString(),
                        Text = o.Code
                    }).ToList();
        }

        public Show ShowModel { get; set; }

        public List<SelectListItem> MpaaRatings { get; set; }

        public string MpaaRatingName
        {
            get 
            {  
                if(ShowModel == null || ShowModel.MpaaRatingId == null) return String.Empty;
                return MpaaRatings
                        .Where(o => o.Value == ShowModel.MpaaRatingId.ToString())
                        .Select(o => o.Text)
                        .First();
            }
        }


    }
}