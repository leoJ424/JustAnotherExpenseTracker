using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    /// <summary>
    /// To pass data from the CardsView to DetailedTransactionsView
    /// </summary>
    public class PassDataModel_DetailedTransactionsView
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public Guid CurrentCard;
        public List<Guid> CardIDs;
    }
}
