using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talent.Domain;

namespace Talent.Mvc.Models
{
    public class ShowsViewModel
    {
        private IEnumerable<ShowViewModel> _showViewModels;
        public IEnumerable<ShowViewModel> ShowViewModels
        {
            get 
            { 
                if(_showViewModels == null)
                {
                    _showViewModels = Shows
                        .Select(o => new ShowViewModel{ShowModel = o})
                        .ToList();
                }
                return _showViewModels;
            
            }
        }

        public IEnumerable<Show> Shows { get; set; }

    }
}