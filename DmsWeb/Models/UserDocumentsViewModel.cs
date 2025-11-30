// Models/ViewModels/UserDocumentsViewModel.cs
using System.Collections.Generic;

namespace DmsWeb.Models
{
    public class UserDocumentsViewModel
    {
        public AppUser User { get; set; } = null!;
        public List<Document> Documents { get; set; } = new();
    }
}
