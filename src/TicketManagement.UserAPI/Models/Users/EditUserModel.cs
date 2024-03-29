﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketManagement.UserAPI.Models.Users
{
    /// <summary>
    /// Edit user model.
    /// </summary>
    public class EditUserModel
    {
        /// <summary>
        /// Get or sets user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets user email.
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
        /// Gets or sets balance.
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Gets or sets time zone.
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
