using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public interface ICardRepository
    {
        List<Guid> ReturnCardIDsofUser(NetworkCredential credential);
        CreditCardModel ReturnCardDetails(Guid cardId);
        CreditCardModel ReturnMaskedCardDetails(Guid cardId);
    }
}
