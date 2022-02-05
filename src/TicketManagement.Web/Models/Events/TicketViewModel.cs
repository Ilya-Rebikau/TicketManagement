using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Models.Events
{
    public class TicketViewModel
    {
        public TicketDto Ticket { get; set; }

        public double Price { get; set; }
    }
}
