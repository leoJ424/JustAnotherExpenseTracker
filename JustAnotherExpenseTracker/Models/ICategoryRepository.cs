using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public interface ICategoryRepository
    {
        string GetCategoryName(Guid CategoryID);
    }
}
