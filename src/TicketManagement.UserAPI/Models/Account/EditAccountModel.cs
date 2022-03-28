using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketManagement.UserAPI.Models.Account
{
    /// <summary>
    /// Edit account model.
    /// </summary>
    public class EditAccountModel
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets timezone.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets all time zones.
        /// </summary>
        public SelectList TimeZones { get; } = new SelectList(GetStandartTimeZones());

        /// <summary>
        /// Get list with all standart time zones.
        /// </summary>
        /// <returns>List with time zones.</returns>
        private static List<string> GetStandartTimeZones()
        {
            var stringTimeZones = new List<string>();
            var list = TimeZoneInfo.GetSystemTimeZones();
            foreach (var item in list)
            {
                stringTimeZones.Add(item.Id);
            }

            return stringTimeZones;
        }
    }
}
